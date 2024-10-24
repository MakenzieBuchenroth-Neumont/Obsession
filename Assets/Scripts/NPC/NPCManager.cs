using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.VolumeComponent;

public class NPCManager : MonoBehaviour, ITimeTracker {

	private Dictionary<Student, GameObject> npcInstances = new Dictionary<Student, GameObject>();
	private Dictionary<Student, Vector3> npcDestinations = new Dictionary<Student, Vector3>();
	private Dictionary<Student, float> npcSpeeds = new Dictionary<Student, float>();
	private Dictionary<Student, bool> isWeaponPickupInProgress = new Dictionary<Student, bool>();

	public QuestManager questManager;

	public float walkingSpeed = 1f;
	public float runningSpeed = 3f;
	public int maxAcceptableLateness = 5;

	#region Start & Update
	private void Start() {
		TimeManager.Instance.registerTracker(this);

		DontDestroyOnLoad(this.gameObject);

	}

	public void Update() {
	}
	#endregion

	public void clockUpdate(GameTimestamp timestamp) {
		foreach(KeyValuePair<Student, GameObject> student in npcInstances)
		{
			ScheduleEvent scheduleEvent = new ScheduleEvent();
			if (ShouldNPCStartMoving(student.Key, student.Key.myScheduleData, timestamp, out scheduleEvent)){
				PrepareNPCForNextEvent(student.Key, scheduleEvent, timestamp);
			}
		}
	}

	#region NPC Movement
	private bool ShouldNPCStartMoving(Student student, NPCScheduleData scheduleData, GameTimestamp currentTime, out ScheduleEvent scheduleEvent) {
		scheduleEvent = new ScheduleEvent();

        if (student == null) {
			Debug.LogWarning("One of the required objects in 'ShouldNPCStartMoving' is null.");
			return false;
		}
		if (!npcInstances.ContainsKey(student)) {
			Debug.LogWarning($"No NPC instance found for student: {student}. Ensure they are properly registered.");
			return false;
		}

		GameObject npcInstance = npcInstances[student];
		if (npcInstance == null) {
			Debug.LogWarning($"NPC instance for student: {student} is null.");
			return false;
		}
		NavMeshAgent navAgent = npcInstances[student].GetComponent<NavMeshAgent>();
		if (navAgent == null) {
			Debug.LogWarning($"NavMeshAgent is missing for student: {student}.");
			return false;
		}

		if (student.IsDead) {
			Debug.LogWarning($"{student} is dead. Can't move!");
			return false;
		}
		foreach (ScheduleEvent testScheduleEvent in scheduleData.npcScheduleList)
		{
			if (GameTimestamp.Compare(currentTime, testScheduleEvent.time)) {
				scheduleEvent = testScheduleEvent;
				break;
			}
		}
		if (scheduleEvent.time == null)
		{
			return false;
		}
			Vector3 currentPosition = navAgent.transform.position;
			Vector3 targetPosition = student.getTargetPosition(scheduleEvent.location);

        float distance = Vector3.Distance(currentPosition, targetPosition);
			float travelTimeInSeconds = distance / walkingSpeed;
			float travelTimeInMinutes = travelTimeInSeconds / 60;

			int currentMinutes = currentTime.hour * 60 + currentTime.minute;
			int eventMinutes = scheduleEvent.time.hour * 60 + scheduleEvent.time.minute;

			int startMovingMinutes = eventMinutes - (int)travelTimeInMinutes;

			return currentMinutes >= startMovingMinutes;
	}

	private void PrepareNPCForNextEvent(Student student, ScheduleEvent scheduleEvent, GameTimestamp currentTime) {
		GameObject npcInstance = npcInstances[student];
		NavMeshAgent navAgent = npcInstance.GetComponent<NavMeshAgent>();
		Vector3 targetPosition = student.getTargetPosition(scheduleEvent.location)  ;

		int currentMinutes = currentTime.hour * 60 + currentTime.minute;
		int eventMinutes = scheduleEvent.time.hour * 60 + scheduleEvent.time.minute;
		int availableMinutes = eventMinutes - currentMinutes;

		if (availableMinutes >= 0 && !student.IsDead) {
			navAgent.SetDestination(targetPosition);

			navAgent.speed = walkingSpeed;
			npcInstance.transform.eulerAngles = scheduleEvent.facing;
		}
		else {
			//Debug.Log("$\"NPC {student.name} cannot start moving yet. Waiting for the event time.\");");
		}
	}
	#endregion

	#region Pickup Weapons
	public void handleWeaponPickup(GameObject weapon) {
		foreach (var npc in npcInstances) {
			if (!isWeaponPickupInProgress[npc.Key] && !npc.Key.IsDead) {
				if (!npcDestinations.ContainsKey(npc.Key)) {
					isWeaponPickupInProgress[npc.Key] = true;
					StartCoroutine(PickupAndDeliverWeapon(npc.Key, weapon));
					break;
				}
			}
		}
	}

	private IEnumerator PickupAndDeliverWeapon(Student student, GameObject weapon) {
		GameObject npcInstance = npcInstances[student];
		NavMeshAgent navAgent = npcInstance.GetComponent<NavMeshAgent>();

		navAgent.ResetPath();

		while (navAgent.remainingDistance > 0.1f || navAgent.pathPending) {
			yield return null;
		}

		Debug.Log($"{student.name} picked up the weapon {weapon.name}.");
		Destroy(weapon); // Destroy or remove the weapon from the ground

		GameObject nearestClassroom = findNearestClassroom(npcInstance.transform.position);
		if (nearestClassroom != null && !student.IsDead) {
			// Route through the nearest door if applicable
			navAgent.SetDestination(nearestClassroom.transform.position);

			// Wait until NPC reaches the door
			while (navAgent.remainingDistance > 0.1f || navAgent.pathPending && !student.IsDead) {
				yield return null;
			}
		}

		Debug.Log($"{student.name} delivered the weapon to the classroom.");
		// Reset weapon pickup state and resume normal schedule
		isWeaponPickupInProgress[student] = false;
	}

	public GameObject findNearestClassroom(Vector3 npcPosition) {
		GameObject[] classrooms = GameObject.FindGameObjectsWithTag("Classroom");
		GameObject nearestClassroom = null;
		float minDistance = Mathf.Infinity;

		foreach (GameObject classroom in classrooms) {
			float distance = Vector3.Distance(npcPosition, classroom.transform.position);
			if (distance < minDistance) {
				minDistance = distance;
				nearestClassroom = classroom;
			}
		}
		
		return nearestClassroom;
	}
	#endregion

	public void RegisterNPC(Student student, GameObject npcInstance) {
		if (!npcInstances.ContainsKey(student) && !student.IsDead) {
			npcInstances[student] = npcInstance;
			var navAgent = npcInstance.GetComponent<NavMeshAgent>();
			if (navAgent == null) {
				navAgent = npcInstance.AddComponent<NavMeshAgent>();
			}
			navAgent.speed = walkingSpeed;

			isWeaponPickupInProgress[student] = false;
		}
		else {
			Debug.Log($"NPC Instance for student {student.name} is already registered.");
		}
	}

	public void giveQuest(Student student) {
		if (student.quest != null && !student.IsDead) {
			questManager.addQuest(student.quest);
			Debug.Log($"{student.name} has give you a quest: {student.quest.objective}");
		}
		else {
			Debug.Log($"{student.name} has no quest available.");
			// TODO: tell the player there is no available quest
		}
	}
		
	public void handleNPCStab(Student stabbedStudent) {
		Debug.Log($"{stabbedStudent.name} was stabbed.");
	}

	public void handleWeaponThreat(GameObject npc) {
		Student student = findStudentByNPC(npc);
		if (student != null) {
			student.reactToWeapon();
		}
	}

	public Student findStudentByNPC(GameObject npc) {
		foreach (var kvp in npcInstances) {
			if (kvp.Value == npc) {
				return kvp.Key;
			}
		}
		return null;
	}

	private void OnDestroy() {
		TimeManager.Instance.unregisterTracker(this);
	}
}