using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachment : MonoBehaviour {
	// Start is called before the first frame update
	private enum Panels {
		Pause,
		Dialogue,
		Prompt,
		Inventory,
		Yearbook,
		Quest,
		QTE,
		GameOver
	}

	[SerializeField] private Panels panels;
	bool active = false;
	bool lateStart = true;

	void Start() {
		switch (panels) {
			case Panels.Pause:
				GameManager.Instance.pauseUI = this.gameObject;
				break;
			case Panels.Dialogue:
				GameManager.Instance.dialogueUI = this.gameObject;
				break;
			case Panels.Prompt:
				GameManager.Instance.promptUI = this.gameObject;
				break;
			case Panels.Inventory:
				active = true;
				GameManager.Instance.inventoryUI = this.gameObject;
				break;
			case Panels.Yearbook:
				GameManager.Instance.yearbookUI = this.gameObject;
				break;
			case Panels.Quest:
				GameManager.Instance.questUI = this.gameObject;
				break;
			case Panels.QTE:
				GameManager.Instance.qteUI = this.gameObject;
				break;
			case Panels.GameOver:
				GameManager.Instance.gameoverUI = this.gameObject;
				break;
		}
	}


	// Update is called once per frame
	void Update() {
		if (lateStart) {
			lateStart = false;
			this.gameObject.SetActive(active);
		}
	}
}
