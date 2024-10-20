using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class QuestBook : MonoBehaviour {
	public TMP_Text questListText;
	private QuestManager questManager;

	// Start is called before the first frame update
	void Start() {
		questManager = FindObjectOfType<QuestManager>();
		updateQuestList();
	}

	public void updateQuestList() {
		StringBuilder sb = new StringBuilder();

		foreach (Quest quest in questManager.activeQuests) {
			sb.AppendLine($"Guest Giver: {quest.questGiver}");
			sb.AppendLine($"Objective: {quest.objective}");
			sb.AppendLine();
		}

		questListText.text = sb.ToString();
	}
}
