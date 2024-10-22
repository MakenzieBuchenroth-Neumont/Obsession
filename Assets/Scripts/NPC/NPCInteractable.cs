using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable {
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

	public void interact(PlayerInteract player) {
		Vector3 directionToNPC = (player.transform.position - transform.position).normalized;
		float dotProduct = Vector3.Dot(transform.forward, directionToNPC);

		if (dotProduct > 0.5f) {
			UIManager.Instance.showInteractionPrompt("Talk");
			talkToNPC();
		}
		else if (dotProduct < -0.5f && player.hasWeapon) {
			UIManager.Instance.showInteractionPrompt("Stab");
			stab();
		}
		else if (dotProduct < -0.5f && !player.hasWeapon) {
			talkToNPC();
		}
	}

	private void talkToNPC() {
		UIManager.Instance.hideInteractionPrompt();
		GameObject dialogueBox = DialogueManager.Instance.dialogueBox;
		if (dialogueBox != null) {
			dialogueBox.SetActive(true);
			if (dialogueBox.TryGetComponent(out Dialogue dialogue)) {
				dialogue.studentData = studentData;
				if (studentData.DialogueLines.Length > 0) {
					dialogue.startDialogue(studentData.DialogueLines);
				}
				else {
					Debug.LogWarning("No dialogue lines assigned.");
				}
			}
		}
		else {
			Debug.LogError("DialogueBox is null in talkToNPC().");
		}
	}

	private void stab() {
		UIManager.Instance.hideInteractionPrompt();
		startStabQTE();
	}

	private void startStabQTE() {
		KeyCode stabKey = KeyCode.U;
		QTEManager qTEManager = FindObjectOfType<QTEManager>();
		if (qTEManager != null) {
			qTEManager.StartQTE("U",3f);
			Debug.Log("QTE started for stabbing.");
		}
	}

	public void handleQTESuccess() {
		Debug.Log("NPC has been stabbed.");
	}

	public void handleQTEFailure() {
		Debug.Log("You got caught.");
	}
}
