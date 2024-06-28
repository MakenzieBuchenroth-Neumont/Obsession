using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPrompt : MonoBehaviour {
	[SerializeField] GameObject doorPrompt;
	[SerializeField] Transform doorTransform;
	[SerializeField] BoxCollider doorBoxCollider;
	[SerializeField] BoxCollider wallBoxCollider;

	bool doorOpened = false;

	private void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			Vector3 promptLocation = Camera.main.WorldToScreenPoint(doorTransform.transform.position);
			doorPrompt.transform.position = promptLocation;
			doorPrompt.SetActive(true);
			if (Input.GetKeyDown(KeyCode.F)) {
				if (doorOpened == false) {
					doorTransform.transform.Rotate(Vector3.up, -90);
					doorBoxCollider.enabled = false;
					wallBoxCollider.enabled = false;
					doorOpened = true;
				}
				else if (doorOpened == true) {
					doorTransform.transform.Rotate(Vector3.up, 90);
					doorBoxCollider.enabled = true;
					wallBoxCollider.enabled = true;
					doorOpened = false;
				}
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			doorPrompt.SetActive(false);
		}
	}
}
