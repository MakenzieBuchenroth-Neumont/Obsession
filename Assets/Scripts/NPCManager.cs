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

	public float walkingSpeed = 1f;
	public float runningSpeed = 3f;
	public int maxAcceptableLateness = 5;

	private void Start() {
		TimeManager.Instance.registerTracker(this);

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
		if (scheduleEvent.ignoreDayOfTheWeek || scheduleEvent.dayOfTheWeek == currentTime.GetDayOfTheWeek()) {
			GameObject npcInstance = npcInstances[student];
			NavMeshAgent navAgent = npcInstance?.GetComponent<NavMeshAgent>();
			Vector3 currentPosition = navAgent.transform.position;
			Vector3 targetPosition = scheduleEvent.coord;

			float distance = Vector3.Distance(currentPosition, targetPosition);
			float travelTimeInSeconds = distance / walkingSpeed;
			float travelTimeInMinutes = travelTimeInSeconds / 60;

			int currentMinutes = currentTime.hour * 60 + currentTime.minute;
			int eventMinutes = scheduleEvent.time.hour * 60 + scheduleEvent.time.minute;
			
			int startMovingMinutes = eventMinutes - (int)travelTimeInMinutes;

			// Log details for debugging
			Debug.Log($"NPC {student.name} should start moving to event '{scheduleEvent.name}'");
			Debug.Log($"Current Time: {currentTime.hour}:{currentTime.minute}");
			Debug.Log($"Event Time: {scheduleEvent.time.hour}:{scheduleEvent.time.minute}");
			Debug.Log($"Distance: {distance} units, Travel Time: {travelTimeInMinutes} minutes");
			Debug.Log($"Start Moving Time: {startMovingMinutes / 60}:{startMovingMinutes % 60}");

			if (currentMinutes >= startMovingMinutes) {
				return true;
			}
		}
		return false;
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
			Debug.Log("$\"NPC {student.name} cannot start moving yet. Waiting for the event time.\");");
		}
	}

	private void OnDestroy() {
		TimeManager.Instance.unregisterTracker(this);
	}
}