using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDetection : MonoBehaviour {
	public float detectionRadius = 5f;
	private NPCManager npcManager;

	// Start is called before the first frame update
	void Start() {
		npcManager = FindObjectOfType<NPCManager>();
	}

	// Update is called once per frame
	void Update() {
		detectWeapons();
	}

	private void detectWeapons() {
		Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

		foreach (Collider hitCollider in colliders) {
			DroppableItem droppableItem = hitCollider.GetComponent<DroppableItem>();
			if (droppableItem != null && droppableItem.itemData.isWeapon) {
				npcManager.handleWeaponPickup(droppableItem.gameObject);
			}
		}
	}
}
