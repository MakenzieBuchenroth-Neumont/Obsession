using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour {
	public static TimeManager Instance {  get; private set; }
	private List<ITimeTracker> timeTrackers = new List<ITimeTracker>();

	[Header("Internal Clock")]
	[SerializeField] public GameTimestamp timestamp;
	public float timeScale = 1.0f;

	[Header("Day and Night Cycle")]
	[SerializeField] Transform sunTransform;
	private Coroutine timeUpdateCoroutine;
	private bool isUpdating = false;
	private bool isTimeUpdating = false;

	private void Awake() {
		if (Instance != null && Instance != this) {
			Debug.Log("Duplicate TimeManager detected. Destroying the new instance.");
			Destroy(this);
		}
		else {
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
			if (sunTransform != null) {
				DontDestroyOnLoad(sunTransform.gameObject);
			}
			Debug.Log("TimeManger instance created and set to persist between scenes.");
			SceneManager.sceneLoaded += OnSceneLoaded;
			startTimeUpdate();
		}
	}
	private void Start() {
		timestamp = new GameTimestamp(1, 7, 0);
	}

	private void startTimeUpdate() {
		if (timeUpdateCoroutine != null) {
			StopCoroutine(timeUpdateCoroutine);
		}
		timeUpdateCoroutine = StartCoroutine(TimeUpdate());
	}

	private IEnumerator TimeUpdate() {
		Debug.Log("Starting TimeUpdate coroutine...");
		while (true) {
			Tick();
			yield return new WaitForSeconds(1 / timeScale);
		}
	}

	public void Tick() {
		if (timestamp == null) {
			Debug.LogWarning("Timestamp is missing!");
			return;
		}
		timestamp.UpdateClock();
		//Debug.Log($"Time updated: Day {timestamp.day}, Hour {timestamp.hour}, Minute {timestamp.minute}");

		foreach (ITimeTracker tracker in timeTrackers) {
			if (tracker != null) {
				tracker.clockUpdate(timestamp);
			}
			else {
				//Debug.LogWarning("Found a null time tracker. Skipping update for this tracker.");
			}
		}

		updateSunMovement();
		//Debug.Log("Tick.");
	}

	public void updateSunMovement() {
		if (sunTransform == null) {
			Debug.LogWarning("Sun Transform is null. Cannot update transform.");
			return;
		}
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

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		Debug.Log($"Scene '{scene.name}' loaded. Ensuring TimeUpdate coroutine is active.");
		startTimeUpdate();
	}

	private void OnDestroy() {
		Debug.Log("TimeManager is being destroyed. Unsubscribing from scene-loaded event.");
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
