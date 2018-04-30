using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ContinueButton : MonoBehaviour {

	private Button butt;
	private Image image;
	private Text text;

	// Initialization
	void Awake () {
		butt = GetComponent<Button> ();
		image = butt.GetComponent<Image> ();
		text = butt.GetComponentInChildren<Text> ();
	}

	// Check if there is any progress to continue from
	void OnEnable () {
		if (SpawnManager.Instance.sequenceIndex > 0) {
			if (!butt.interactable) {
				setInteractable (true);
			}
		} else if (butt.interactable) {
			setInteractable (false);
		}
	}

	private void setInteractable (bool setToTrue) {
		if (setToTrue) {
			butt.interactable = true;
			text.color = text.color + Color.black;
			image.color = image.color + Color.black;
		} else {
			butt.interactable = false;
			text.color = text.color - (Color.black * 0.65f);
			image.color = image.color - (Color.black * 0.65f);
		}
	}
}
