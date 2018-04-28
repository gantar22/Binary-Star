using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ContinueButton : MonoBehaviour {

	private Button butt;
	private Text text;

	// Initialization
	void Awake () {
		butt = GetComponent<Button> ();
		text = butt.GetComponentInChildren<Text> ();
	}

	// Check if there is any progress to continue from
	void OnEnable () {
		if (SpawnManager.Instance.sequenceIndex > 0) {
			if (!butt.interactable) {
				butt.interactable = true;
				text.color = text.color + Color.black;
			}
		} else if (butt.interactable) {
			butt.interactable = false;
			text.color = text.color - (Color.black * 0.65f);
		}
	}
}
