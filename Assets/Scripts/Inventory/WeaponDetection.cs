using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponDetection : MonoBehaviour {
	public float detectionRadius = 5f;
	public float detectionAngle = 90f;

	[Header("Required Scripts")]
	private NPCManager npcManager;
	private Student student;
	private InventoryManager inventoryManager;
	private Animator npcAnimator;
	public AudioClip npcReactVoiceLine;
	private AudioSource audioSource;


	// Start is called before the first frame update
	void Start() {
		npcManager = FindObjectOfType<NPCManager>();
		inventoryManager = InventoryManager.Instance;
		npcAnimator = GetComponent<Animator>();
		//audioSource = GetComponent<AudioSource>

		if (npcManager == null || inventoryManager == null || npcAnimator == null) {
			Debug.LogError("Required component missing!");
		}
		/*if (audioSource == null) {
			Debug.LogError("No audioSource component found!");
		}*/

		student = npcManager.findStudentByNPC(gameObject);

		if (student == null) {
			Debug.LogError("No Student reference found for this NPC.");
		}
	}

	// Update is called once per frame
	void Update() {
		detectWeaponsInVisionCone();
	}

	private void detectWeaponsInVisionCone() {
		Collider[] collider = Physics.OverlapSphere(transform.position, detectionRadius);

		foreach (Collider col in collider) {
			if (isWithinVisionCone(col.transform)) {
				DroppableItem droppableItem = col.GetComponent<DroppableItem>();
				if (droppableItem != null && droppableItem.itemData.isWeapon) {
					if (student != null & npcManager != null) {
						npcManager?.handleWeaponThreat(this.gameObject);
					}
				}
			}
		}
	}

	private bool isWithinVisionCone(Transform target) {
		Vector3 directionToTarget = target.position - transform.position;
		float distanceToTarget = directionToTarget.magnitude;

		if (distanceToTarget > detectionAngle) return false;

		directionToTarget.Normalize();
		float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
		return angleToTarget <= detectionAngle /2f;
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, detectionRadius);

		Vector3 forwardLeft = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionRadius;
		Vector3 forwardRight = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionRadius;

		Gizmos.DrawLine(transform.position, transform.position + forwardLeft);
		Gizmos.DrawLine(transform.position, transform.position + forwardRight);
	}
}
