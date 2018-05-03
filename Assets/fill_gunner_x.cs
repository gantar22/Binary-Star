using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fill_gunner_x : MonoBehaviour {

	private Image im;
	// Use this for initialization
	void Start () {
		im = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Player_Missile_Fire.get_cooldown() != -1)
			im.fillAmount = Player_Missile_Fire.get_cooldown();
		else im.fillAmount = 0;
	}
}
