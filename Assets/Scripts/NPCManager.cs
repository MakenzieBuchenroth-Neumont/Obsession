using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;

public class NPCManager : MonoBehaviour, ITimeTracker {
	public List<NPCScheduleData> npcSchedules;
	private Dictionary<Student, GameObject> npcInstances = new Dictionary<Student, GameObject>();

	private void Start() {
		TimeManager.Instance.registerTracker(this);

		foreach( var schedule in npcSchedules) {
			if (schedule.student != null && schedule.student.prefab != null) {
				GameObject npcInstance = Instantiate(schedule.student.prefab);
				npcInstances[schedule.student] = npcInstance;
			}
		}
	}

	public void clockUpdate(GameTimestamp timestamp) {
		foreach (var npcSchedule in npcSchedules) {
			foreach (var scheduleEvent in npcSchedule.npcScheduleList) {
				if (IsScheduleEventActive(scheduleEvent, timestamp)) {
					UpdateNPCState(npcSchedule.student, scheduleEvent);
				}
			}
		}
	}

	private bool IsScheduleEventActive(ScheduleEvent scheduleEvent, GameTimestamp currentTime) {
		if (scheduleEvent.ignoreDayOfTheWeek || scheduleEvent.dayOfTheWeek == currentTime.GetDayOfTheWeek()) {
			return currentTime.hour == scheduleEvent.time.hour && currentTime.minute == scheduleEvent.time.minute;
		}
		return false;
	}

	private void UpdateNPCState(Student student, ScheduleEvent scheduleEvent) {
		if (npcInstances.TryGetValue(student, out GameObject npcInstance)) {
			npcInstance.transform.position = scheduleEvent.coord;
			npcInstance.transform.forward = scheduleEvent.facing;
			Debug.Log($"Updated NPC {student.name} to position {scheduleEvent.coord} facing {scheduleEvent.facing}");
		}
		else {
			Debug.LogError($"NPC instance for {student.name} not found.");
		}
	}

	private void OnDestroy() {
		TimeManager.Instance.unregisterTracker(this);
	}
}
