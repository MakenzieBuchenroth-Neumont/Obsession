using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : MonoBehaviour {
	[SerializeField] GameObject sleepPrompt;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			Vector3 promptLocation = Camera.main.WorldToScreenPoint(this.transform.position) * 1;
			sleepPrompt.transform.position = promptLocation;
			sleepPrompt.SetActive(true);
			if (Input.GetKeyDown(KeyCode.F)) {
				//SaveLoad.save();
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Player") {
			sleepPrompt.SetActive(false);
		}
	}
}
