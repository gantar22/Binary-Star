using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPDrop : MonoBehaviour {

	public int _HPGain = 1;


	void OnTriggerEnter2D(Collider2D col){
		PlayerHP PHP = col.gameObject.GetComponent<PlayerHP> ();
		if (PHP != null && PHP.enabled) {
			if (PHP.gainHP (_HPGain)) {
				getPickedUp ();
			}
		}
	}

	void getPickedUp(){
		Destroy(gameObject);
	}
}
