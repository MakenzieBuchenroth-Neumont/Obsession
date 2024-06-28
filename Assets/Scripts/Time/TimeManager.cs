using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TimeManager : MonoBehaviour {
	public static TimeManager Instance {  get; private set; }
	private List<ITimeTracker> timeTrackers = new List<ITimeTracker>();

	[Header("Internal Clock")]
	[SerializeField] GameTimestamp timestamp;
	public float timeScale = 1.0f;

	[Header("Day and Night Cycle")]
	[SerializeField] Transform sunTransform;

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(this);
		}
		else {
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}

	private void Start() {
		timestamp = new GameTimestamp(1, 8, 0);
		StartCoroutine(TimeUpdate());
	}

	private IEnumerator TimeUpdate() {
		while (true) {
			Tick();
			yield return new WaitForSeconds(1 / timeScale);
		}
	}

	public void Tick() {
		timestamp.UpdateClock();

		foreach (ITimeTracker tracker in timeTrackers) {
			tracker.clockUpdate(timestamp);
		}

		updateSunMovement();
	}

	public void updateSunMovement() {
		// convert the current time to minutes
		int timeInMinutes = GameTimestamp.hoursToMinutes(timestamp.hour) + timestamp.minute;

		// sun moves 15 degrees an hour
		//.25 degrees in a minute
		// at midnight 00:00, the angle of the sun is -90

		float sunAngle = 0.25f * timeInMinutes - 90;

		// apply the angle to the directional light
		sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
	}

	// handling listeners

	// add the object to the list of listeners
	public void registerTracker(ITimeTracker tracker) {
		if (!timeTrackers.Contains(tracker)) { }
		timeTrackers.Add(tracker);
	}

	// remove the object from the list of listeners
	public void unregisterTracker(ITimeTracker tracker) {
		timeTrackers.Remove(tracker);
	}
}
