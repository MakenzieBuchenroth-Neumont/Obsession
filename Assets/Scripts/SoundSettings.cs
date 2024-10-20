using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour {
	[SerializeField] Slider soundSlider;
	[SerializeField] AudioMixer audioMixer;

	void Start() {
		setVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
	}

	public void setVolume(float value) {
		if (value < 1) {
			value = .001f;
		}
		refreshSlider(value);
		PlayerPrefs.SetFloat("SavedMasterVolume", value);
		audioMixer.SetFloat("MasterVolume", Mathf.Log10(value / 100) * 20f);
	}

	public void setVolumeFromSlider() {
		setVolume(soundSlider.value);
	}

	public void refreshSlider(float value) {
		soundSlider.value = value;
	}
}
