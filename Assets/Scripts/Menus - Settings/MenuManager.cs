using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	public GameObject defaultSelect;
	public GameObject defaultUpSelect;

	public Button backButton;

	public static float backTimer;

	private bool buttonSelected;
	private GameObject prevSelected;

	private bool delayed;
	private int lastDirection;
	private float delayTimer;
	private float holdTimer;

	private float slowDelay = 0.65f;
	private float fastDelay = 0.14f;
	private float currentDelay;

	private EventSystem eventSystem;


	void OnEnable() {
		resetDelay ();
	}

	void Update() {
		if (eventSystem == null) {
			eventSystem = EventSystem.current;
		}

		// Press 'b' on joystick or backspace on keyboard to go back
		backTimer += Time.deltaTime;
		if (Input.GetAxisRaw ("Cancel") > 0 && backButton != null && (backTimer >= 0.5f || PauseManager.paused)) {
			backButton.onClick.Invoke ();
			backTimer = 0;
		}

		// Default select the top or bottom of the menu
		if (eventSystem.currentSelectedGameObject == null) {
			buttonSelected = false;
		}

		float vertical = Input.GetAxisRaw ("Vertical");
		if ((vertical == -1 || vertical == 1) && !buttonSelected) {
			if (vertical == -1) {
				eventSystem.SetSelectedGameObject (defaultSelect);
			} else if (vertical == 1) {
				eventSystem.SetSelectedGameObject (defaultUpSelect);
			}

			buttonSelected = true;
		}

		// Wrapping top-to-bottom and bottom-to-top
		if (prevSelected != eventSystem.currentSelectedGameObject) {
			delayed = true;
			delayTimer = 0;
		}
		if (vertical != 0) {
			lastDirection = (int)vertical;
		}

		if ((vertical == -1 || vertical == 1) && buttonSelected) {
			if (prevSelected != eventSystem.currentSelectedGameObject) {
				delayed = true;
				delayTimer = 0;
			}

			if ((int)vertical == lastDirection) {
				delayTimer += Time.deltaTime;
				holdTimer += Time.deltaTime;
			} else {
				resetDelay ();
			}

			if (holdTimer >= slowDelay) {
				currentDelay = fastDelay;
			} else {
				currentDelay = slowDelay;
			}

			if (eventSystem.currentSelectedGameObject == defaultSelect && vertical == 1 && (!delayed || (delayTimer >= currentDelay)) ) {
				eventSystem.SetSelectedGameObject (defaultUpSelect);
				delayTimer = 0;
			} else if (eventSystem.currentSelectedGameObject == defaultUpSelect && vertical == -1 && (!delayed || (delayTimer >= currentDelay)) ) {
				eventSystem.SetSelectedGameObject (defaultSelect);
				delayTimer = 0;
			}

		} else {
			resetDelay ();
		}

		prevSelected = eventSystem.currentSelectedGameObject;
	}

	private void resetDelay() {
		delayTimer = 0;
		holdTimer = 0;
		lastDirection = 2;
		delayed = false;
	}

	private void OnDisable() {
		buttonSelected = false;
	}


	// Functions for buttons to call

	public void ContinueGame() {
		SceneManager.LoadScene (2); 
	}

	public void NewGame() {
		GM.ResetProgressThenPlay ();
	}

	public void LoadMainMenu() {
		SceneManager.LoadScene ("MainMenu");
	}

	public void Quit() {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}