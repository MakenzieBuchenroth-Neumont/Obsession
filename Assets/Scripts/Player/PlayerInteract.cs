using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
	float interactRange = 2.0f;
	private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
			Collider[] hitCollider = Physics.OverlapSphere(transform.position, interactRange);
			foreach (Collider collider in hitCollider) {
				if (collider.TryGetComponent(out NPCInteractable interactable)) {
					interactable.interact();
					break;
				}
				DroppableItem droppableItem = collider.GetComponent<DroppableItem>();
				if (droppableItem != null) {
					Debug.Log($"Picked up {droppableItem.itemData.name}.");
					droppableItem.pickupItem();
					break;
				}
			}
		}
	}
}
