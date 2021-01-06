using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killIfOOB : MonoBehaviour {

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
			EnemyHP[] EHPs = transform.root.gameObject.GetComponentsInChildren<EnemyHP>();
			foreach (EnemyHP EHP in EHPs) {
				EHP.die();
			}
			Destroy(transform.root.gameObject); //Scary Bugs!
		}
	}
}
