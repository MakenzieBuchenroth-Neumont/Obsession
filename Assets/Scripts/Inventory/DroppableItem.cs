using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableItem : MonoBehaviour {
	[SerializeField] public ItemData itemData;
	public void pickupItem() {
		if (InventoryManager.Instance.isInventoryFull() && InventoryManager.Instance.isHandFull()) {
			Debug.Log($"Hand and inventory are full.");
			return;
		}

		if (InventoryManager.Instance.tryAddToInventory(itemData)) {
			Destroy(gameObject);
		}
	}
	


	private void OnTriggerStay(Collider other) {
		if (other.tag == "Player" ) {
			if (Input.GetKeyUp(KeyCode.F)) {
				pickupItem();
			}
		}
	}
}
