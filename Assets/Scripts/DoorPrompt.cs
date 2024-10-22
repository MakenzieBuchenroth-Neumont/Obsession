using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class DoorPrompt : MonoBehaviour {
	[SerializeField] Transform doorTransform;
	[SerializeField] BoxCollider doorBoxCollider;
	[SerializeField] BoxCollider wallBoxCollider;
	private bool doorOpened = false;

	public void toggleDoor() {
		doorOpened = !doorOpened;
		if (doorOpened) {
			doorTransform.transform.Rotate(Vector3.up, -90);
			doorBoxCollider.enabled = false;
			wallBoxCollider.enabled = true;
			Debug.Log("Door opened.");
		}
		else {
			doorTransform.transform.Rotate(Vector3.up, 90);
			doorBoxCollider.enabled = true;
			wallBoxCollider.enabled = false;
			Debug.Log("Door closed.");
		}
	}

	public bool isDoorOpened() {
		return doorOpened;
	}
}
