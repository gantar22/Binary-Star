using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bomb_fill : MonoBehaviour {

	bomb_det bd;

	// Use this for initialization
	void Start () {
		bd = GM.Instance.player.GetComponentInChildren<bomb_det>();
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Image>().fillAmount = 1 - (bd.timer / bd.cooldown);
	}
}
