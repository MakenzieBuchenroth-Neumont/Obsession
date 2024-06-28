using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour {
	[SerializeField] float interactRange = 2f;

	private void Update() {
		if (Input.GetKeyDown(KeyCode.F)) {
			Physics.OverlapSphere(transform.position, interactRange);
		}
	}
}
