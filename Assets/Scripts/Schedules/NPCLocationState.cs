using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NPCLocationState {
	public Student student;
	public Vector3 coord;
	public Vector3 facing;

	public NPCLocationState(Student student) : this() {
		this.student = student;
	}

	public NPCLocationState(Student student,  Vector3 coord, Vector3 facing) : this() {
		this.student = student;
		this.coord = coord;
		this.facing = facing;
	}
}
