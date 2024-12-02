using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemHandling : MonoBehaviour {
	private bool isHolding = false;

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Q)) {
			isHolding = true;
			Debug.Log("Started holding the button to drop item.");
			StartCoroutine(HoldToDrop());
		}

		if (Input.GetKeyUp(KeyCode.Q)) {
			isHolding = false;
			Debug.Log("Stopped holding the button.");
			StopCoroutine(HoldToDrop());
		}
	}

	private IEnumerator HoldToDrop() {
		float holdTime = 0f;

		while (isHolding) {
			holdTime += Time.deltaTime;
			Debug.Log($"Holding for {holdTime} seconds...");

			if (holdTime >= 3f) {
				Debug.Log("Hold time reached! Dropping item...");
				InventoryManager.Instance.dropItem();
				yield break;
			}

			yield return null;
		}
	}
}
