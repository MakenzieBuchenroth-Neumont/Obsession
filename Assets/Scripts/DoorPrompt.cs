using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class DoorPrompt : MonoBehaviour {
	[SerializeField] GameObject doorPrompt;
	[SerializeField] Transform doorTransform;
	[SerializeField] BoxCollider doorBoxCollider;
	[SerializeField] BoxCollider wallBoxCollider;

	bool doorOpened = false;

	private void OnTriggerStay(Collider other) {
		if (other.CompareTag("Player")) {
			Vector3 promptLocation = Camera.main.WorldToScreenPoint(doorTransform.transform.position);
			doorPrompt.transform.position = promptLocation;
			doorPrompt.SetActive(true);
			if (Input.GetKeyDown(KeyCode.F)) {
				toggleDoor();
			}
		}
		else if (other.CompareTag("NPC")) {
			NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
			if (agent != null && !doorOpened) {
				agent.isStopped = true;
				toggleDoor();
				StartCoroutine(ResumeAfterDoorOpened(agent));
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			doorPrompt.SetActive(false);
		}
	}

	private void toggleDoor() {
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

	private IEnumerator ResumeAfterDoorOpened(NavMeshAgent agent) {
		yield return new WaitForSeconds(1f);
		agent.isStopped = false;
	}
}
