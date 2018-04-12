using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombChargeDrop : MonoBehaviour {

	public int _ChargeGain = 1;


	void OnTriggerEnter2D(Collider2D col){
		PlayerHP PHP = col.gameObject.GetComponent<PlayerHP> ();
		if (PHP != null && PHP.enabled) {

			/*
			if (PHP.gainBombCharge (_ChargeGain)) {
				getPickedUp ();
			}
			*/
		}
	}

	void getPickedUp(){
		Destroy(gameObject);
	}
}
