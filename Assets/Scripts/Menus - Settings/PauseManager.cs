using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

	public GameObject PausePanel;
	public GameObject OptionsPanel;

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

		if (transform.parent == null) {
			DontDestroyOnLoad (this);
		}

		previousTimeScale = 1.0f;
	}

	void Update () {
		// Pause keys
		if (Input.GetAxisRaw("Cancel") > 0) {
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

	public void pause() {
		paused = true;
		previousTimeScale = Time.timeScale;
		Time.timeScale = 0f;
		PausePanel.SetActive (true);
	}

	public void resume() {
		paused = false;
		Time.timeScale = previousTimeScale;
		PausePanel.SetActive (false);
		OptionsPanel.SetActive (false);
	}
}