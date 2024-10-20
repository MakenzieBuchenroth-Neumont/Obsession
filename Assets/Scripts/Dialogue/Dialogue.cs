using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq.Expressions;

public class Dialogue : MonoBehaviour {
	public Student studentData;
	public TextMeshProUGUI textComponent;
	public float textSpeed;

	private NPCManager npcManager;

	private int index;
	private string[] currentLines;
	private bool isGoodbyeDialogue = false;
	private bool isTyping = false;

	private void Start() {
		npcManager = FindObjectOfType<NPCManager>();
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			if (isTyping) {
				StopAllCoroutines();
				textComponent.text = getCurrentLine();
				isTyping = false;
			}
			else {
				if (index < currentLines.Length - 1) {
					nextLine();
				}
				else if (isGoodbyeDialogue) {
					DialogueManager.Instance.closeDialogueBox();
				}
			}
		}
	}

	public void startDialogue(string[] lines, bool isGoodbye = false) {
		currentLines = lines;
		isGoodbyeDialogue = isGoodbye;
		index = 0;

		if (currentLines.Length > 0) {
			gameObject.SetActive(true);
			textComponent.text = string.Empty;
			StartCoroutine(typeLine());
		}
		else {
			Debug.Log("No dialogue lines assigned.");
		}
	}

	IEnumerator typeLine() {
		isTyping = true;
		textComponent.text = string.Empty;
		// type each character 1 by 1
		foreach (char c in currentLines[index].ToCharArray()) {
			textComponent.text += c;
			yield return new WaitForSeconds(textSpeed);
		}
		isTyping = false;

		if (isGoodbyeDialogue) {
			DialogueManager.Instance.closeDialogueBox();
		}
	}

	void nextLine() {
		if (currentLines == null || index < 0 || index >= currentLines.Length) {
			Debug.LogError("CurrentLines is null or Index {index} is out of bounds for currentLines with length {currentLines.Length}");
			return;
		}
		if (index < currentLines.Length - 1) {
			index++;
			textComponent.text = string.Empty;
			StartCoroutine(typeLine());
		}
	}

	private string getCurrentLine() {
		return currentLines[index];
	}

	public void OnTalkButton() {
		if (studentData == null || studentData.TalkLines.Length == 0) {
			Debug.Log("No talkLines or no studentData.");
			return;
		}
		startDialogue(studentData.TalkLines, false);
	}

	public void OnAskButton() {
		if (studentData != null || studentData.AskLines.Length > 0) {
			startDialogue(studentData.AskLines, false);
		}
		else {
			Debug.Log("No askLines.");
		}
	}

	public void OnQuestButton() {
		if (studentData != null || studentData.QuestLines.Length > 0) {
			startDialogue(studentData.QuestLines, false);
			npcManager.giveQuest(studentData);
		}
			Debug.Log("No questLines.");
	}

	public void OnByeButton() {
		if (studentData != null || studentData.GoodbyeLines.Length > 0) {
			startDialogue(studentData.GoodbyeLines, true);
		}
		else {
			Debug.Log("No goodbyeLines.");
			DialogueManager.Instance.closeDialogueBox();
		}
	}
}
