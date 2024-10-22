using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
	float interactRange = 1.5f;
	public bool hasWeapon => InventoryManager.Instance.equippedTool != null;

	private void Update() {
		Collider[] hitCollider = Physics.OverlapSphere(transform.position, interactRange);
		bool promptShown = false;

		foreach (Collider c in hitCollider) {
			if (c.TryGetComponent(out DoorPrompt prompt)) {
				UIManager.Instance.showInteractionPrompt("Open Door");
				promptShown = true;
				if (Input.GetKeyDown(KeyCode.F)) {
					prompt.toggleDoor();
				}
			}
			else if (c.TryGetComponent(out NPCInteractable interactable)) {
				UIManager.Instance.showInteractionPrompt("Interact");
				promptShown = true;

				if (Input.GetKeyDown(KeyCode.F)) {
					interactable.interact(this);
				}
			}
        }
		if (!promptShown) {
			UIManager.Instance.hideInteractionPrompt();
		}
	}
}
