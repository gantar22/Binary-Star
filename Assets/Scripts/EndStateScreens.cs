using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EndStateScreen {You_Win, You_Died, Game_Over};

public class EndStateScreens : MonoBehaviour {

	// Settings/properties
	[SerializeField]
	private float delay = 1;
	[SerializeField]
	private AnimationClip You_Win;
	[SerializeField]
	private AnimationClip You_Died;
	[SerializeField]
	private AnimationClip Game_Over;
	[SerializeField]
	private GameObject HUD;

	// References
	private Animator animator;

	// Other variables
	private bool readyToContinue = false;
	private EndStateScreen chosenScreen;

	[HideInInspector]
	public static bool stateScreenIsUp = false;


	// Static instance setup
	private static EndStateScreens _instance;
	public static EndStateScreens Instance { get { return _instance; } }


	// Initialization
	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}

		animator = GetComponent<Animator> ();
	}
		
	// Update is called once per frame
	void Update () {
		if (!readyToContinue) {
			return;
		}

		if (Input.GetAxis ("Submit") > 0) {
			animator.Play ("Deactivated"); // Make this smooth?
			HUD.SetActive (true);

			stateScreenIsUp = false;
			readyToContinue = false;

			if (chosenScreen == EndStateScreen.You_Win) {
				SpawnManager.Instance.nextSequence();
				// TODO - Might want to go straight to an upgrade screen or
				//		  do something else as freeplay begins
			} else if (chosenScreen == EndStateScreen.You_Died) {
				GM.Instance.resetToSameSequence();
				GM.Instance.RespawnPlayer ();
				SpawnManager.Instance.nextSequence();
			} else if (chosenScreen == EndStateScreen.Game_Over) {
				GM.ResetProgressThenMainMenu ();
			}
		}
	}

	// Invoke PlayEndScreen after a delay
	public void InvokeEndScreen (EndStateScreen screen) {
		stateScreenIsUp = true;
		readyToContinue = false;
		chosenScreen = screen;

		HUD.SetActive (false);
		Invoke ("PlayEndScreen", delay);
	}

	// Play one of the End State Screens
	private void PlayEndScreen () {
		if (chosenScreen == EndStateScreen.You_Win) {
			animator.Play ("You_Win");
			Invoke ("ReadyUp", You_Win.length);
		} else if (chosenScreen == EndStateScreen.You_Died) {
			animator.Play ("You_Died");
			Invoke ("ReadyUp", You_Died.length);
		} else if (chosenScreen == EndStateScreen.Game_Over) {
			animator.Play ("Game_Over");
			Invoke ("ReadyUp", Game_Over.length);
		}
	}

	// Enable readyToContinue
	private void ReadyUp() {
		readyToContinue = true;
	}
}
