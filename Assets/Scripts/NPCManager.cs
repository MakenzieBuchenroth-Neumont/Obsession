using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class NPCManager : MonoBehaviour, ITimeTracker {
	public static NPCManager Instance { get; private set; }

	[SerializeField]
	private GameObject npcPrefab;

	private Dictionary<Student, GameObject> npcGameObjects;

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(this);
		}
		else {
			Instance = this;
		}
	}

	List<Student> students = null;
	List<NPCScheduleData> npcSchedules;

	[SerializeField]
	List<NPCLocationState> npcLocations;

	// load all student data
	public List<Student> Students() {
		if (students != null) return students;
		Student[] studentDatabase = Resources.LoadAll<Student>("Students");
		students = studentDatabase.ToList();
		return students;
	}

	private void OnEnable() {
		NPCScheduleData[] schedules = Resources.LoadAll<NPCScheduleData>("Schedules");
		npcSchedules = schedules.ToList();
		InitNPCLocations();
		InstantiateNpcs();
	}

	private void Start() {
		TimeManager.Instance.registerTracker(this);
	}

	private void InitNPCLocations() {
		npcLocations = new List<NPCLocationState>();
		foreach (Student student in Students()) {
			npcLocations.Add(new NPCLocationState(student));
		}
	}

	private void InstantiateNpcs() {
		npcGameObjects = new Dictionary<Student, GameObject>();
		foreach(Student student in Students()) {
			GameObject npcInstance = Instantiate(npcPrefab);
			npcGameObjects[student] = npcInstance;
		}
	}

	public void clockUpdate(GameTimestamp timestamp) {
		updateNPCLocations(timestamp);
	}

	private void updateNPCLocations(GameTimestamp timestamp) {
		for (int i = 0; i < npcLocations.Count; i++) {
			NPCLocationState npcLocator = npcLocations[i];
			// find the schedule belonging to the npc
			NPCScheduleData schedule = npcSchedules.Find(x => x.student == npcLocator.student);
			if (schedule == null) {
				Debug.LogError("No schedule found for " + npcLocator.student.name);
				continue;
			}

			// current time
			GameTimestamp.DayOfTheWeek dayOfTheWeek = timestamp.GetDayOfTheWeek();

			// find the events that correspond to the current time
			List<ScheduleEvent> eventsToConsider = schedule.npcScheduleList.FindAll(x => x.time.hour <= timestamp.hour && (x.dayOfTheWeek == dayOfTheWeek || x.ignoreDayOfTheWeek));
			// check if the events are empty
			if (eventsToConsider.Count < 1) {
				Debug.LogError("None found for " + npcLocator.student.name);
				Debug.LogError(timestamp.hour);
				continue;
			}

			// remove all the events with the hour that is lower than the max time (the time has already elapsed)
			int maxHour = eventsToConsider.Max(x => x.time.hour);
			eventsToConsider.RemoveAll(x => x.time.hour < maxHour);

			// get the events with the highest priority
			ScheduleEvent eventToExecute = eventsToConsider.OrderByDescending(x => x.priority).FirstOrDefault();
			Debug.Log(eventToExecute.name);	
			// set the npc locator value accordingly
			npcLocations[i] = new NPCLocationState(schedule.student, eventToExecute.coord, eventToExecute.facing);

			// update npc position and rotation
			if (npcGameObjects.TryGetValue(npcLocator.student, out GameObject npcGameObject)) {
				npcGameObject.transform.position = eventToExecute.coord;
				npcGameObject.transform.forward = eventToExecute.facing;
			}
		}
	}
}
