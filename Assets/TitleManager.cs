using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
	public void newGameButton() {
		SceneManager.LoadScene("FirstFloor");
		gameMode = GameMode.Start;
	}

	// need save/load system first
	public void loadGameButton() {

	}

	public void settingsButton() {

	}
}
