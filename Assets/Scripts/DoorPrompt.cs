using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class DoorPrompt : MonoBehaviour {
	[SerializeField] GameObject doorPrompt;
	[SerializeField] Transform doorTransform;
	[SerializeField] BoxCollider doorBoxCollider;
	[SerializeField] BoxCollider wallBoxCollider;
	bool doorOpened = false;

	private void OnCollisionStay(Collision collision) {
			if (collision.gameObject.tag.Equals("Player")) {
			Debug.Log("Player entered door trigger area.");

			Vector3 promptLocation = Camera.main.WorldToScreenPoint(doorTransform.transform.position) * 1;
			doorPrompt.transform.position = promptLocation;
			doorPrompt.SetActive(true);
			if (Input.GetKeyDown(KeyCode.F)) {
				toggleDoor();
			}
		}
		else if (collision.gameObject.tag.Equals("NPC")) {
			NavMeshAgent agent = collision.gameObject.GetComponent<NavMeshAgent>();
			if (agent != null && !doorOpened) {
				agent.isStopped = true;
				toggleDoor();
				StartCoroutine(ResumeAfterDoorOpened(agent));
			}
		}
	}

	private void OnCollisionExit(Collision collision) {
		if (collision.gameObject.tag.Equals("Player")) {
			doorPrompt.SetActive(false);
		}
	}

	private void toggleDoor() {
		doorOpened = !doorOpened;
		if (doorOpened) {
			doorTransform.transform.Rotate(Vector3.up, -90);
			doorBoxCollider.enabled = true;
			wallBoxCollider.enabled = true;
			Debug.Log("doorBoxCollider.enabled: " + doorBoxCollider.enabled);
			Debug.Log("wallBoxCollider.enabled: " + wallBoxCollider.enabled);
		}
		else {
			doorTransform.transform.Rotate(Vector3.up, 90);
			doorBoxCollider.enabled = false;
			wallBoxCollider.enabled = false;
			Debug.Log("doorBoxCollider.enabled: " + doorBoxCollider.enabled);
			Debug.Log("wallBoxCollider.enabled: " + wallBoxCollider.enabled);
		}
	}

	private IEnumerator ResumeAfterDoorOpened(NavMeshAgent agent) {
		yield return new WaitForSeconds(3f);
		agent.isStopped = false;
	}
}
