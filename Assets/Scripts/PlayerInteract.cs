using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
	private void Update() {
		if (Input.GetKeyDown(KeyCode.F)) {
			Collider[] hitCollider = Physics.OverlapSphere(transform.position, 1f);

			foreach (Collider col in hitCollider) {
				DroppableItem droppableItem = col.GetComponent<DroppableItem>();
				if (droppableItem != null) {
					Debug.Log($"Picked up {droppableItem.itemData.name}.");
					droppableItem.pickupItem();
					break;
				}
			}
		}
	}
}
