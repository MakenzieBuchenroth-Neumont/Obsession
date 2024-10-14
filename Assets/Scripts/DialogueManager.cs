using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
	public static DialogueManager Instance { get; private set; }
	public GameObject dialogueBox;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	public void closeDialogueBox() {
		if (dialogueBox != null) {
			dialogueBox.SetActive(false);
		}
		else {
			Debug.LogError("Dialogue box is null! Cannot close.");
		}
	}
}
