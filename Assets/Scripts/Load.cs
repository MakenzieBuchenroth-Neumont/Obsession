using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour {
	[SerializeField] GameObject player;
	// Start is called before the first frame update
	void Start() {
		DontDestroyOnLoad(player.gameObject);
	}

	// Update is called once per frame
	void Update() {

	}
}
