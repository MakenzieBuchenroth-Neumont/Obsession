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
	bool doorOpened = false;

	private bool isPlayerNearby = false;

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			isPlayerNearby = true;
			UIManager.Instance.showInteractionPrompt("Open Door");
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			isPlayerNearby = false;
			UIManager.Instance.hideInteractionPrompt();
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

	private void Update() {
		if (isPlayerNearby && Input.GetKeyDown(KeyCode.F)) {
			toggleDoor();
		}
	}

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag("NPC")) {
			NavMeshAgent agent = collision.gameObject.GetComponent<NavMeshAgent>();
			if (agent != null && !doorOpened) {
				agent.isStopped = true;
				toggleDoor();
				StartCoroutine(ResumeAfterDoorOpened(agent));
			}
		}
	}

	private IEnumerator ResumeAfterDoorOpened(NavMeshAgent agent) {
		yield return new WaitForSeconds(3f);
		agent.isStopped = false;
	}
}
