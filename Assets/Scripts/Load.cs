using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {
	[SerializeField] GameObject player;
	[SerializeField] GameObject canvas;
	[SerializeField] GameObject directionalLight;
	// Start is called before the first frame update
	void Start() {
		DontDestroyOnLoad(player.gameObject);
		DontDestroyOnLoad(canvas.gameObject);
		DontDestroyOnLoad(directionalLight.gameObject);
	}

	// Update is called once per frame
	void Update() {

	}
}
