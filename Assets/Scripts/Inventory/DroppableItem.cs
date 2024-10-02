using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableItem : MonoBehaviour {
	[SerializeField] public ItemData itemData;

	public void pickupItem() {
		if (!InventoryManager.Instance.isInventoryFull() && !InventoryManager.Instance.isHandFull()) {
			bool added = InventoryManager.Instance.tryAddToInventory(itemData);

			if (added) {
				Debug.Log($"{itemData.name} added to inventory.");
				Destroy(gameObject);
			}
			else {
				Debug.Log("Failed to add item.");
			}
		}
		else {
			if (InventoryManager.Instance.isInventoryFull()) {
				Debug.Log("Inventory is full.");
			}
			if (InventoryManager.Instance.isHandFull()) {
				Debug.Log("Hand is full.");
			}
		}
	}

	private void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			if (Input.GetKeyUp(KeyCode.F)) {
				pickupItem();
			}
		}
	}
}
