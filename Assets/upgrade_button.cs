using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class upgrade_button : MonoBehaviour {


	public Upgrade u;
	[SerializeField]
	private SpriteRenderer pilot_icon;
	[SerializeField]
	private SpriteRenderer gunner_icon;


	// Use this for initialization
	void Start () {
		if(!u) return;
		pilot_icon.sprite = u.pilot_icon;
		gunner_icon.sprite = u.gunner_icon;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void put_description(){
		UpgradesManager.Instance.description_ui.text = u.description;
	}
}
