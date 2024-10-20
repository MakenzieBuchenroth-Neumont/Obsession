using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public enum GameMode {
		Title,
		Start,
		Game,
		GameOver
	} 

	[SerializeField] private GameObject titleUI;

	//private TitleUIManager Instance;

	public static GameMode gameMode { get; set; }

	private void Awake() {
		if (FindObjectsOfType<GameManager>().Length > 1) {
			Destroy(gameObject);
		}
		else {
			DontDestroyOnLoad(gameObject);
		}
	}

	void Start() {
		gameMode = GameMode.Title;
		//TitleUIManager.Instance.showTitleUI();
	}

	// Update is called once per frame
	void Update() {
			switch (gameMode) {
			case GameMode.Title:
				//TitleUIManager.Instance.showTitleUI();
				break;
			case GameMode.Start:
				//TitleUIManager.Instance.hideTitleUI();
				gameMode = GameMode.Game;
				break;
			case GameMode.Game:
					break;
			case GameMode.GameOver:
				gameMode = GameMode.Title;
				//TitleUIManager.Instance.showTitleUI();
				break;
			default:
				break;
			}
		}



}
