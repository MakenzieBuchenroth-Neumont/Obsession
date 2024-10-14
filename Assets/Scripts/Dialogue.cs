using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq.Expressions;

public class Dialogue : MonoBehaviour {
	public Student studentData;
	public TextMeshProUGUI textComponent;
	public float textSpeed;

	private int index;
	private string[] currentLines;
	private bool isGoodbyeDialogue = false;

	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			if (textComponent.text == getCurrentLine()) {
				nextLine();
			}
			else {
				StopAllCoroutines();
				textComponent.text = getCurrentLine();
			}
		}
	}

	public void startDialogue(string[] lines, bool isGoodbye = false) {
		currentLines = lines;
		isGoodbyeDialogue = isGoodbye;
		index = 0;

		if (currentLines.Length > 0) {
			textComponent.text = string.Empty;
			StartCoroutine(typeLine());
		}
		else {
			Debug.Log("No dialogue lines assigned.");
		}
	}

	IEnumerator typeLine() {
		// type each character 1 by 1
		foreach (char c in currentLines[index].ToCharArray()) {
			textComponent.text += c;
			yield return new WaitForSeconds(textSpeed);
		}
	}

	void nextLine() {
		if (currentLines == null || index < 0 || index >= currentLines.Length) {
			Debug.LogError("CurrentLines is null or Index {index} is out of bounds for currentLines with length {currentLines.Length}");
		}
		Debug.Log($"Next line called. Current index: {index}, total lines: {currentLines.Length}");
		if (index < getCurrentLine().Length - 1) {
			index++;	
			textComponent.text = string.Empty;
			Debug.Log("Moving to next line. Current index: " + index);
			StartCoroutine(typeLine());
		}
		else {
			Debug.Log("End of lines reached. Goodbye dialogue? " + isGoodbyeDialogue);
			if (isGoodbyeDialogue) {
				Debug.Log("Closing dialogue box.");
				DialogueManager.Instance.closeDialogueBox();	
			}
			else {
				Debug.Log("Dialogue is finished, but not a goodbye. Not closing.");
			}
		}
	}

	private string getCurrentLine() {
		return currentLines[index];
	}

	public void OnTalkButton() {
		if (studentData == null) {
			Debug.Log("Student data is null in Dialogue class.");
			return;
		}
		if (studentData != null && studentData.TalkLines.Length > 0) {
			startDialogue(studentData.TalkLines, false);
		}
		else {
			Debug.Log("No talk lines or no studentData assigned.");
		}
	}

	public void OnAskButton() {
		if (studentData != null) {
			Debug.Log($"Ask lines: {studentData.AskLines.Length}");
		}
		if (studentData != null && studentData.AskLines.Length > 0) {
			startDialogue(studentData.AskLines, false);
		}
		else {
			Debug.Log("No ask lines.");
		}
	}

	public void OnQuestButton() {
		if (studentData != null) {
			Debug.Log($"Quest lines: {studentData.QuestLines.Length}");
		}
		if (studentData != null && studentData.QuestLines.Length > 0) {
			startDialogue(studentData.QuestLines, false);
		}
		else {
			Debug.Log("No quest lines.");
		}
	}

	public void OnByeButton() {
		if (studentData.GoodbyeLines.Length > 0) {
			startDialogue(studentData.GoodbyeLines, true);
		}
		else {
			Debug.Log("No goodbye lines.");
			gameObject.SetActive(false);
		}
	}
}
