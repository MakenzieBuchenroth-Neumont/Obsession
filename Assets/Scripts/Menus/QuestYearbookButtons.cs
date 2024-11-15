using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestYearbookButtons : MonoBehaviour {
	public Button questButton, yearbookButton, closeQuestButton, closeYearbookButton;
		
	// Start is called before the first frame update
	void Start() {
		questButton.onClick.AddListener(openQuestMenu);
		yearbookButton.onClick.AddListener(openYearbookMenu);
		closeQuestButton.onClick.AddListener(closeQuestMenu);
		closeYearbookButton.onClick.AddListener(closeYearbookMenu);
	}

	public void openQuestMenu() {
		GameManager.Instance.quest();
	}

	public void openYearbookMenu() {
		GameManager.Instance.yearbook();
	}

	public void closeQuestMenu() {
		GameManager.Instance.closeQuest();
	}

	public void closeYearbookMenu() {
		GameManager.Instance.closeYearbook();
	}
}
