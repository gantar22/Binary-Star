using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fill_pilot_x : MonoBehaviour {



	private Image im;
	// Use this for initialization
	void Start () {
		im = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		im.fillAmount = PlayerMove.x_cooldown();
	}
}
