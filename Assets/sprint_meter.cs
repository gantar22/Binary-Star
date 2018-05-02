﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sprint_meter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!GM.Instance.player) {
			return;
		}

		GetComponent<Slider>().value = GM.Instance.player.GetComponentInChildren<PlayerMove>().GetHeat();
	}
}
