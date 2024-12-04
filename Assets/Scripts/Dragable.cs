using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : MonoBehaviour
{
	bool beingDragged = false;
	// Start is called before the first frame update
	void Start()
    {
        
    }

	// Update is called once per frame
	private void Update() {
		//Gizmos.DrawWireSphere(gameObject.transform.position, 2);
		Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, 2);
		foreach (Collider collider in colliders) {
			if (collider.gameObject.tag == "Player") {
				if (Input.GetKeyUp(KeyCode.F)) {
					beingDragged = true;	
				}
			}
		}

		if (beingDragged) {
			if (Input.GetKeyUp(KeyCode.F)) {
				beingDragged = false;
			}
			gameObject.transform.position = InventoryManager.Instance.playerHand.position;
		}
		

	
	}

}
/*
 **/