using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour {
	public float rayDistance = 3f;
	public LayerMask itemLayer;
	public GameObject promptText;
	public Transform playerTransform;

	private DroppableItem currentItem = null;

	// Update is called once per frame
	void Update() {
		RaycastHit hit;
		
		if (Physics.Raycast(playerTransform.position, playerTransform.forward, out hit, rayDistance)) {
			DroppableItem item = hit.collider.GetComponent<DroppableItem>();
			if (item != null) {
				if (!InventoryManager.Instance.isInventoryFull() && InventoryManager.Instance.equippedItem == null) {
					showPrompt(item);
					Debug.Log($"Prompt shown for {item.name}.");
					if (Input.GetKeyDown(KeyCode.F)) {
						item.pickupItem();
						hidePrompt();
						Debug.Log($"Prompt hidden for {item.name}.");
					}
				}
				else {
					hidePrompt();
				}
			}
			else {
				hidePrompt();
			}
		}
		else {
			hidePrompt();
		}
	}

	private void showPrompt(DroppableItem item) {
		if (item != currentItem) {
			currentItem = item;
			promptText.gameObject.SetActive(true);
		}
	}

	private void hidePrompt() {
		if (currentItem != null) {
			currentItem = null;
			promptText.gameObject.SetActive(false);
		}
	}
}
