using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour {
	public Student studentData;

	private GameObject dialogueBox;

	private void Start() {
		dialogueBox = FindDialogueBoxInDontDestroyOnLoad();

		if (dialogueBox != null) {
			dialogueBox.SetActive(false);
		}
	}

	private GameObject FindDialogueBoxInDontDestroyOnLoad() {
		return GameObject.FindGameObjectWithTag("DialogueBox");
	}

	public void interact() {
		GameObject dialogueBox = DialogueManager.Instance.dialogueBox;
		if (dialogueBox != null) {
			dialogueBox.SetActive(true);
			if (dialogueBox.TryGetComponent(out Dialogue dialogue)) {
				dialogue.studentData = studentData;
				if (studentData.DialogueLines.Length > 0) {
					dialogue.startDialogue(studentData.DialogueLines);
				}
				else {
					Debug.LogWarning("No dialogue lines assigned to this student.");
				}
			}
		}
		else {
			Debug.LogError("DialogueBox is null in interact(). Ensure it is correctly assigned in Start.");
		}
	}
}
