using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class QuestManager : MonoBehaviour {
	public static QuestManager instance { get; private set; }
	public List<Quest> activeQuests = new List<Quest>();

	private void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	public void addQuest(Quest quest) {
		activeQuests.Add(quest);
	}

	public void pickUpItem(QuestItem item) {
		foreach (Quest quest in activeQuests) {
			if (quest.itemToFind == item.itemName) {
				quest.completeQuest();
				Debug.Log($"Quest updated: {quest.objective}.");
				break;
			}
		}
	}
}
