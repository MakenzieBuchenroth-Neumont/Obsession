using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : MonoBehaviour {
	public string itemName;
	private bool isPlayerNearby;

	public void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
			isPlayerNearby = true;
			UIManager.Instance.showInteractionPrompt($"Press 'F' to pick up {itemName}.");
        }
    }

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			isPlayerNearby= false;
			UIManager.Instance.hideInteractionPrompt();
		}
	}

	// Update is called once per frame
	void Update() {
		if (isPlayerNearby && Input.GetKeyDown(KeyCode.F)) {
			QuestManager.instance.pickUpItem(this);
			Destroy(gameObject);
		}
	}
}
