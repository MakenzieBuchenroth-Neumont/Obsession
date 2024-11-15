
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {
	public static AudioManager Instance;
	public Sound[] musicSounds,sfxSounds;
	public AudioSource musicSource,sfxSource;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	private void Start() {
		playMusic("MainTheme");	
	}

	public void playMusic(string name) {
		Sound s = Array.Find(musicSounds, s => s.name == name);
		if (s == null) {
			Debug.Log("Sound not found!");
		}
		else {
			musicSource.clip=s.clip;
			musicSource.Play();
		}
	}

	public void playSFX(string name) {
		Sound s = Array.Find(sfxSounds, s => s.name == name);
		if (s == null) {
			Debug.Log("SFX not found!");
		}
		else {
			sfxSource.PlayOneShot(s.clip);
		}
	}
}
