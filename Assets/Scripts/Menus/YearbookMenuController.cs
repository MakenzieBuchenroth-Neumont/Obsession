using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YearbookMenuController : MonoBehaviour {
	[SerializeField] GameObject yearbookMenu;
	[SerializeField] GameObject pauseMenu;

	private HashSet<string> allowedScenes = new HashSet<string>() {
		"FirstFloor", "SecondFloor", "ThirdFloor", "FourthFloor", "Outside", "Bedroom"
	};

	private void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (allowedScenes.Contains(scene.name)) {
			yearbookMenu.SetActive(false);
		}
		else {
			yearbookMenu.SetActive(true);
		}
	}

	public void yearbook() {
		if (GameManager.gameMode == GameManager.GameMode.Game) {
			Debug.Log("Clicked yearbook");
			yearbookMenu.SetActive(!yearbookMenu.activeSelf);
			if (pauseMenu != null && yearbookMenu.activeInHierarchy) {
				pauseMenu.SetActive(false);
			}
			else {
				pauseMenu.SetActive(true);
			}
		}
		else {
			Debug.Log("Cannot toggle yearbook menu, gameMode is not game.");
		}
	}
}
