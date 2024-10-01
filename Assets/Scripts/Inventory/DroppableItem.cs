using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableItem : MonoBehaviour {
	[SerializeField] public ItemData itemData;
	[SerializeField] private GameObject itemPromptPrefab;
	private GameObject currentPromptUI;

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log($"Press 'F' to up {itemData.name}");
			showPrompt();
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log("Player left pickup area.");
			hidePrompt();
		}
	}

	private void showPrompt() {
		if (currentPromptUI == null) // Only instantiate if it doesn't already exist
		{
			currentPromptUI = Instantiate(itemPromptPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
			currentPromptUI.transform.SetParent(Camera.main.transform); // Parent to the camera for consistent positioning
			Debug.Log($"Prompt shown for {itemData.name}.");
		}
	}

	private void hidePrompt() {
		if (currentPromptUI != null) {
			Destroy(currentPromptUI);
			currentPromptUI = null; // Reset reference
			Debug.Log("Prompt hidden.");
		}	
	}

	public void pickupItem() {
		InventoryManager.Instance.addItemToInventory(itemData);
		Destroy(gameObject);
		hidePrompt();
	}
}
