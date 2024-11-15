using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager Instance { get; private set; }

	public enum GameMode {
		Title,
		Start,
		Game,
		GameOver
	}

	[SerializeField] private GameObject titleUI;

	[SerializeField] public GameObject pauseUI;
	[SerializeField] public GameObject dialogueUI;
	[SerializeField] public GameObject promptUI;
	[SerializeField] public GameObject inventoryUI;
	[SerializeField] public GameObject yearbookUI;
	[SerializeField] public GameObject questUI;
	[SerializeField] public GameObject qteUI;
	[SerializeField] public GameObject gameoverUI;

	public static GameMode gameMode { get; set; }

	public bool hasStoppedMusic = false;

	private void Awake() {
		if (FindObjectsOfType<GameManager>().Length > 1) {
			Destroy(gameObject);
		}
		else {
			DontDestroyOnLoad(gameObject);
			Instance = this;
		}
	}

	void Start() {
		gameMode = GameMode.Title;
	}

	// Update is called once per frame
	void Update() {
		switch (gameMode) {
			case GameMode.Title:
				if (hasStoppedMusic == false) {
					stopMusic();
					hasStoppedMusic = true;
					playTitleTheme();
					break;
				}
				break;
			case GameMode.Start:
				gameMode = GameMode.Game;
				if (hasStoppedMusic == false) {
					stopMusic();
					hasStoppedMusic = true;
					playMainTheme();
					break;
				}
				break;
			case GameMode.Game:
				if (Input.GetKeyDown(KeyCode.Escape)) {
					pauseUI.SetActive(!pauseUI.activeSelf);
				}
				if (TimeManager.Instance.timestamp.hour == 18 && TimeManager.Instance.timestamp.minute == 0 && hasStoppedMusic == false) {
					stopMusic();
					hasStoppedMusic = true;
					playBedroomTheme();
					break;
				}
				break;
			case GameMode.GameOver:
				if (hasStoppedMusic == false) {
					stopMusic();
					hasStoppedMusic = true;
					playGameOverTheme();
					break;
				}
				break;
			default:
				break;
		}
	}

	public void gameOver() {
		if (gameMode == GameMode.Game) {
			gameoverUI.SetActive(true);
			gameMode = GameMode.GameOver;
			hasStoppedMusic = false;
		}
	}

	public void quest() {
		if (gameMode == GameMode.Game) {
			questUI.SetActive(true);
			pauseUI.SetActive(false);
		}
	}

	public void yearbook() {
		if (gameMode == GameMode.Game) {
			yearbookUI.SetActive(true);
			pauseUI.SetActive(false);
		}
	}

	public void closeQuest() {
		questUI.SetActive(false);
		pauseUI.SetActive(true);
		yearbookUI.SetActive(false);
	}

	public void closeYearbook() {
		yearbookUI.SetActive(false);
		pauseUI.SetActive(true);
		questUI.SetActive(false);
	}

	public void stopMusic() {
		AudioManager.Instance.musicSource.Stop();
	}

	public void playTitleTheme() {
		AudioManager.Instance.playMusic("TitleTheme");
	}

	public void playMainTheme() {
		AudioManager.Instance.playMusic("MainTheme");
	}

	public void playBedroomTheme() {
		AudioManager.Instance.playMusic("BedroomTheme");
	}

	public void playGameOverTheme() {
		AudioManager.Instance.playMusic("GameOverTheme");
	}
}
