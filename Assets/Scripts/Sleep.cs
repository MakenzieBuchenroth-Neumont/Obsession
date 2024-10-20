using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : MonoBehaviour {
	[SerializeField] GameObject sleepPrompt;

	private bool isPlayerNearby = false;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			isPlayerNearby = true;
			UIManager.Instance.showInteractionPrompt($"Press 'F' to sleep.");
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F)) {
			//SaveLoad.save();
			UIManager.Instance.hideInteractionPrompt();
		}
	}
}
