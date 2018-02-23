using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killIfOOB : MonoBehaviour {

	private Vector3 _location;


	static bool not_percent(float x){
		return x < -.2 || x > 1.2;
	}



	void Update () {
		_location = Camera.main.WorldToViewportPoint(transform.position);
		if(not_percent(_location.x) || not_percent(_location.y)) {
			Destroy(transform.root.gameObject,.1f); //Scary Bugs!
		}
	}
}
