using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.VolumeComponent;

public class NPCManager : MonoBehaviour, ITimeTracker {

	public List<NPCScheduleData> npcSchedules;
	private Dictionary<Student, GameObject> npcInstances = new Dictionary<Student, GameObject>();
	private Dictionary<Student, Vector3> npcDestinations = new Dictionary<Student, Vector3>();
	private Dictionary<Student, float> npcSpeeds = new Dictionary<Student, float>();
	private Dictionary<Student, bool> isWeaponPickupInProgress = new Dictionary<Student, bool>();

	public float walkingSpeed = 1f;
	public float runningSpeed = 3f;
	public int maxAcceptableLateness = 5;

	private void Start() {
		TimeManager.Instance.registerTracker(this);

		DontDestroyOnLoad(this.gameObject);

		foreach (var schedule in npcSchedules) {
			if (schedule.student != null && schedule.student.prefab != null) {
				GameObject npcInstance = Instantiate(schedule.student.prefab);
				npcInstance.transform.position = schedule.student.spawnPoint;
				npcInstances[schedule.student] = npcInstance;

				var navAgent = npcInstance.GetComponent<NavMeshAgent>();
				if (navAgent == null) {
					navAgent = npcInstance.AddComponent<NavMeshAgent>();
				}
				navAgent.speed = walkingSpeed;

				isWeaponPickupInProgress[schedule.student] = false;
			}
		}
	}

	public void Update() {
		List<Student> arrivedStudents = new List<Student>();
		foreach (var kvp in npcDestinations) {
			Student student = kvp.Key;
			Vector3 destination = kvp.Value;
			GameObject npcInstance = npcInstances[student];
			NavMeshAgent navAgent = npcInstance.GetComponent<NavMeshAgent>();

			if (navAgent.remainingDistance < 0.1f && !navAgent.pathPending) {
				arrivedStudents.Add(student);
			}
		}

		foreach (Student student in arrivedStudents) {
			npcDestinations.Remove(student);
		}
	}

	public void clockUpdate(GameTimestamp timestamp) {
		foreach (var npcSchedule in npcSchedules) {
			foreach (var scheduleEvent in npcSchedule.npcScheduleList) {
				if (ShouldNPCStartMoving(npcSchedule.student, scheduleEvent, timestamp)) {
					PrepareNPCForNextEvent(npcSchedule.student, scheduleEvent, timestamp);
				}
			}
		}
	}

	private bool ShouldNPCStartMoving(Student student, ScheduleEvent scheduleEvent, GameTimestamp currentTime) {
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
			Vector3 currentPosition = navAgent.transform.position;
			Vector3 targetPosition = scheduleEvent.coord;

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
		Vector3 targetPosition = scheduleEvent.coord;

		int currentMinutes = currentTime.hour * 60 + currentTime.minute;
		int eventMinutes = scheduleEvent.time.hour * 60 + scheduleEvent.time.minute;
		int availableMinutes = eventMinutes - currentMinutes;

		if (availableMinutes >= 0) {
			navAgent.SetDestination(targetPosition);

			navAgent.speed = walkingSpeed;
			npcInstance.transform.eulerAngles = scheduleEvent.facing;
		}
		else {
			//Debug.Log("$\"NPC {student.name} cannot start moving yet. Waiting for the event time.\");");
		}
	}

	public void handleWeaponPickup(GameObject weapon) {
		foreach (var npc in npcInstances) {
			if (!isWeaponPickupInProgress[npc.Key]) {
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
		if (nearestClassroom != null) {
			// Route through the nearest door if applicable
			navAgent.SetDestination(nearestClassroom.transform.position);

			// Wait until NPC reaches the door
			while (navAgent.remainingDistance > 0.1f || navAgent.pathPending) {
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

	public void RegisterNPC(Student student, GameObject npcInstance) {
		if (!npcInstances.ContainsKey(student)) {
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

	private void OnDestroy() {
		TimeManager.Instance.unregisterTracker(this);
	}
}