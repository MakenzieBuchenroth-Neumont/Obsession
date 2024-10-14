using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentSpawner : MonoBehaviour {
	[SerializeField] private List<Student> studentsToSpawn;
	[SerializeField] private NPCManager npcManager;

	// Start is called before the first frame update
	void Start() {
		spawnStudents();
	}

	private void spawnStudents() {
		foreach (var student in studentsToSpawn) {
			GameObject studentInstance = Instantiate(student.prefab, student.spawnPoint, Quaternion.identity);

			npcManager.RegisterNPC(student, studentInstance);
		}
	}
}
