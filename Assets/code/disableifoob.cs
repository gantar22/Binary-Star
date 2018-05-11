using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableifoob : MonoBehaviour {

	private Vector3 _location;

	static bool not_percent(float x){
		return x < -.0 || x > 1.0;
	}

	public static bool is_OOB (Vector3 pos) {
		Vector3 VPPos = Camera.main.WorldToViewportPoint(pos);
		return (not_percent (VPPos.x) || not_percent (VPPos.y));
	}



	void Update () {
		_location = Camera.main.WorldToViewportPoint(transform.position);
		if(not_percent(_location.x) || not_percent(_location.y)) {
			GetComponent<BoxCollider2D>().enabled = false;
			Destroy(transform.root.gameObject,3); //Scary Bugs!
		}
	}
}
