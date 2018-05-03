using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMo : MonoBehaviour {

	public float slowedTimeScale = 0.5f;
	public float slowedPitchScale = 0.7f;
	public float realtimeDuration = 5;
	public float fadeTime = 0.75f;

	// Global variables
	[HideInInspector]
	public bool timeIsSlowed;
	[HideInInspector]
	public float realSlowTimeLeft;

	// References
	private YButtonManager yButtManager;


	// Initialization
	void Start () {
		yButtManager = transform.root.GetComponentInChildren<YButtonManager> ();
	}

	// Called once per frame
	void Update () {
		if (!timeIsSlowed || PauseManager.paused) {
			return;
		}

		if (realSlowTimeLeft <= 0) {
			finishSlowMo ();
			return;
		}

		// Managing slow motion
		float realDeltaTime = Time.deltaTime / Time.timeScale;
		realSlowTimeLeft -= realDeltaTime;
		float realTimeElapsed = realtimeDuration - realSlowTimeLeft;

		float newScale;
		float pitchScale;

		if (realTimeElapsed <= fadeTime) {
			newScale = Mathf.Lerp (1, slowedTimeScale, realTimeElapsed / fadeTime);
			pitchScale = Mathf.Lerp (1, slowedPitchScale, realTimeElapsed / fadeTime);
		} else if (realSlowTimeLeft <= fadeTime) {
			newScale = Mathf.Lerp (1, slowedTimeScale, realSlowTimeLeft / fadeTime);
			pitchScale = Mathf.Lerp (1, slowedPitchScale, realSlowTimeLeft / fadeTime);
		} else {
			newScale = slowedTimeScale;
			pitchScale = slowedPitchScale;
		}

		updateTimeScale (newScale, pitchScale);
	}

	// Update the timeScale, and sound settings accordingly
	private void updateTimeScale (float newScale, float pitchScale) {
		Time.timeScale = newScale;
		music_manager.global_pitch = pitchScale;
	}

	// Start or finish the ability
	public void startSlowMo() {
		timeIsSlowed = true;
		realSlowTimeLeft = realtimeDuration;
	}

	private void finishSlowMo() {
		timeIsSlowed = false;
		resetTimeScale ();
		yButtManager.finishedAbility ();
	}

	// Reset all the time settings
	public static void resetTimeScale() {
		Time.timeScale = 1;
		music_manager.global_pitch = 1;
	}
}
