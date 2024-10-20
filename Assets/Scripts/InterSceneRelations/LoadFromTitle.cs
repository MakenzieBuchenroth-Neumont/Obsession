using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFromTitle : MonoBehaviour {
	[SerializeField] GameManager gameManager;

	void Start() {
		DontDestroyOnLoad(gameManager.gameObject);
	}
}
