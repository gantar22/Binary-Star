using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

	public GameObject HUD;
	public GameObject PauseMenu;
	public GameObject OptionsMenu;

	private bool pressed;
	private float previousTimeScale;

	[HideInInspector]
	public static bool paused = false;

	// Singleton instance setup
	private static PauseManager _instance;
	public static PauseManager Instance { get { return _instance; } }


	void Awake () {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			_instance = this;
		}

		/* if (transform.parent == null) {
			DontDestroyOnLoad (this);
		} */

		previousTimeScale = 1.0f;
		resume ();
	}

	void Update () {
		// Pause keys
		if (Input.GetAxisRaw("Pause") > 0) {
			if (paused && !pressed) {
				pressed = true;
				resume ();
			} else if (!paused && !pressed) {
				pressed = true;
				pause ();
			} 
		} else {
			pressed = false;
		}
	}

	// Pause the game
	public void pause() {
		if (EndStateScreens.stateScreenIsUp) {
			return;
		}

		paused = true;
		previousTimeScale = Time.timeScale;
		Time.timeScale = 0f;

		HUD.SetActive (false);
		PauseMenu.SetActive (true);
		OptionsMenu.SetActive (false);
	}

	// Resume the game. If resetTimeScale, Time.timeScale is set back to 1
	public void resume(bool resetTimeScale = true) {
		paused = false;
		if (resetTimeScale) {
			SlowMo.resetTimeScale ();
			previousTimeScale = 1;
		} else {
			Time.timeScale = previousTimeScale;
		}

		HUD.SetActive (true);
		PauseMenu.SetActive (false);
		OptionsMenu.SetActive (false);
	}

	// Resume the game, re
}