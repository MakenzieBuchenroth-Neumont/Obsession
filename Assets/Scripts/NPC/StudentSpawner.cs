using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StudentSpawner : MonoBehaviour {
	[SerializeField] private List<Student> studentsToSpawn;
	[SerializeField] private List<GameObject> LockerLocations;
	[SerializeField] private List<GameObject> BeforeClassLocations;
	[SerializeField] private List<GameObject> SeatLocations;
	[SerializeField] private List<GameObject> LunchLocations;
	[SerializeField] private List<GameObject> AfterClassLocations;
	[SerializeField] private List<GameObject> LeaveLocations;
	[SerializeField] private NPCManager npcManager;

	// Start is called before the first frame update
	void Start() {
		spawnStudents();
	}

	private void spawnStudents() {
		for (int i = 0; i < studentsToSpawn.Count; i++) {
			GameObject studentInstance = Instantiate(studentsToSpawn[i].prefab, studentsToSpawn[i].spawnPoint, Quaternion.identity);
			studentInstance.GetComponent<NPCInteractable>().studentData.Locker = LockerLocations[i];
			studentInstance.GetComponent<NPCInteractable>().studentData.Seat = SeatLocations[i];
			studentInstance.GetComponent<NPCInteractable>().studentData.BeforeClass = BeforeClassLocations[i];
			studentInstance.GetComponent<NPCInteractable>().studentData.Lunch = LunchLocations[i];
			studentInstance.GetComponent<NPCInteractable>().studentData.AfterClass = AfterClassLocations[i];
			studentInstance.GetComponent<NPCInteractable>().studentData.Leave = LeaveLocations[i];

            studentInstance.transform.position = studentInstance.GetComponent<NPCInteractable>().studentData.spawnPoint;

            npcManager.RegisterNPC(studentsToSpawn[i], studentInstance);
		}
	}
}
