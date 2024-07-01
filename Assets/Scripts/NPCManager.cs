using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
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
				npcSpeeds[schedule.student] = walkingSpeed;
			}
		}
	}

	public void Update() {
		List<Student> arrivedStudents = new List<Student>();
		foreach (var kvp in npcDestinations) {
			Student student = kvp.Key;
			Vector3 destination = kvp.Value;
			GameObject npcInstance = npcInstances[student];

			float step = npcSpeeds[student] * Time.deltaTime;
			npcInstance.transform.position = Vector3.MoveTowards(npcInstance.transform.position, destination, step);

			if (Vector3.Distance(npcInstance.transform.position, destination) < 0.001f) {
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
			Vector3 currentPosition = npcInstance.transform.position;
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

			if (currentMinutes >= startMovingMinutes - 3) {
				return true;
			}
		}
		return false;
	}

	private void PrepareNPCForNextEvent(Student student, ScheduleEvent scheduleEvent, GameTimestamp currentTime) {
		GameObject npcInstance = npcInstances[student];
		Vector3 currentPosition = npcInstance.transform.position;
		Vector3 targetPosition = scheduleEvent.coord;

		float distance = Vector3.Distance(currentPosition, targetPosition);
		float travelTimeInSeconds = distance / walkingSpeed;
		float travelTimeInMinutes = travelTimeInSeconds / 60;

		int currentMinutes = currentTime.hour * 60 + currentTime.minute;
		int eventMinutes = scheduleEvent.time.hour * 60 + scheduleEvent.time.minute;
		int availableMinutes = eventMinutes - currentMinutes;

		Debug.Log($"NPC {student.name} preparing for event '{scheduleEvent.name}'");
		Debug.Log($"Current Position: {currentPosition}, Target Position: {targetPosition}");
		Debug.Log($"Available Minutes: {availableMinutes}, Required Travel Time: {travelTimeInMinutes} minutes");

		// Check if it's time to start moving
		if (availableMinutes >= 0) {
			if (availableMinutes * 60 >= travelTimeInSeconds) {
				npcDestinations[student] = targetPosition;
				npcSpeeds[student] = walkingSpeed;
				npcInstance.transform.eulerAngles = scheduleEvent.facing;
				Debug.Log($"NPC {student.name} is walking to {targetPosition} from {currentPosition}.");
			}
			else {
				float maxLatenessInSeconds = maxAcceptableLateness * 60;
				float runTravelTimeInSeconds = distance / runningSpeed;
				if (availableMinutes * 60 + maxLatenessInSeconds >= runTravelTimeInSeconds) {
					npcDestinations[student] = targetPosition;
					npcSpeeds[student] = runningSpeed;
					npcInstance.transform.eulerAngles = scheduleEvent.facing;
					Debug.Log($"NPC {student.name} is running to {targetPosition} from {currentPosition}.");
				}
				else {
					Debug.LogError($"NPC {student.name} does not have enough time to walk or run to the next event.");
				}
			}
		}
		else {
			Debug.Log($"NPC {student.name} cannot start moving yet. Waiting for the event time.");
		}
	}

	private void OnDestroy() {
		TimeManager.Instance.unregisterTracker(this);
	}
}
