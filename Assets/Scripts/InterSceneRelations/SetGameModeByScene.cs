using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetGameModeByScene : MonoBehaviour {
	// Start is called before the first frame update
	void Start() {
		string currentScene = SceneManager.GetActiveScene().name;
		if (currentScene == "Title") {
			GameManager.gameMode = GameManager.GameMode.Title;
		}
		else {
			GameManager.gameMode = GameManager.GameMode.Game;
		}
	}
}
