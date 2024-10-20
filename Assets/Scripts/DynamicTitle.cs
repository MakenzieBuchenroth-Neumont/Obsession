using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicTitle : MonoBehaviour {
	enum State {
		Cheery,
		Evil
	}

	[SerializeField] Sprite cheeryLogo;
	[SerializeField] Sprite evilLogo;

	[SerializeField] Image logo;

	[Header("Happy Buttons")]
	[SerializeField] Sprite button;
	[SerializeField] Sprite buttonPressed;
	[SerializeField] Sprite buttonHover;

	[Header("Dark Buttons")]
	[SerializeField] Sprite buttonDark;
	[SerializeField] Sprite buttonDarkPressed;
	[SerializeField] Sprite buttonDarkHover;

	[Header("Button GameObjects")]
	[SerializeField] GameObject newGame;
	[SerializeField] GameObject loadGame;
	[SerializeField] GameObject settings;
	[SerializeField] GameObject quit;

	[Header("Fonts")]
	[SerializeField] TMP_FontAsset happyFont;
	[SerializeField] TMP_FontAsset evilFont;

	[Header("Overlays")]
	[SerializeField] GameObject cheerfulOverlay;
	[SerializeField] GameObject darkOverlay;

	[Header("Animator")]
	[SerializeField] Animator anim;

	float timer = 0f;

	private State state { get; set; }

	// Start is called before the first frame update
	void Start() {
		timer = 0f;
		state = State.Cheery;
		anim.SetBool("dark", false);
		cheerfulOverlay.gameObject.SetActive(true);
		updateSpriteStates();
	}

	// Update is called once per frame
	void Update() {
		timer += Time.deltaTime;
		if (timer >= 3f) {
			state = (state == State.Cheery) ? State.Evil : State.Cheery;

			updateSpriteStates();
		}
		timer = 0;
	}

	public void updateSpriteStates() {
		if (state == State.Cheery) {
			logo.sprite = evilLogo;

			updateButton(newGame, buttonDark, buttonDarkHover, buttonDarkPressed, evilFont);
			updateButton(loadGame, buttonDark, buttonDarkHover, buttonDarkPressed, evilFont);
			updateButton(settings, buttonDark, buttonDarkHover, buttonDarkPressed, evilFont);
			updateButton(quit, buttonDark, buttonDarkHover, buttonDarkPressed, evilFont);

			darkOverlay.SetActive(true);
			cheerfulOverlay.SetActive(false);

			anim.SetBool("dark", true);
		}
		else {
			logo.sprite = cheeryLogo;

			updateButton(newGame, button, buttonHover, buttonPressed, happyFont);
			updateButton(loadGame, button, buttonHover, buttonPressed, happyFont);
			updateButton(settings, button, buttonHover, buttonPressed, happyFont);
			updateButton(quit, button, buttonHover, buttonPressed, happyFont);

			cheerfulOverlay.SetActive(true);
			darkOverlay.SetActive(false);

			anim.SetBool("dark", false);
		}
	}

	private void updateButton(GameObject button, Sprite normalSprite, Sprite hoverSprite, Sprite pressedSprite, TMP_FontAsset font) {
		button.GetComponent<Image>().sprite = normalSprite;

		Button buttonComponent = button.GetComponent<Button>();
		SpriteState spriteState = new SpriteState();
		spriteState.highlightedSprite = hoverSprite;
		spriteState.pressedSprite = pressedSprite;

		buttonComponent.spriteState = spriteState;

		TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
		if (buttonText != null) {
			buttonText.font = font;
		}

		buttonComponent.enabled = false;
		buttonComponent.enabled = true;
	}
}
