using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour {

	// Settings
	public bool KillIfVeryOOB = true;
	private float extraScreenRadius = 4f;


	// Called once per frame
	void Update () {
		Vector2 velo = ScrollManager.scrollVelo;
		transform.position += new Vector3(velo.x, velo.y, 0) * Time.deltaTime;

		if (KillIfVeryOOB) {
			checkForVeryOOB ();
		}
	}

	// Destroy this gameObject if it is OOB in current scroll direction
	private void checkForVeryOOB() {
		Vector2 pos = transform.position;



	}

	private void destroyThisObj() {
		EnemyHP EHP = gameObject.GetComponent<EnemyHP> ();
		if (EHP) {
			EHP.die ();
		} else {
			Destroy (gameObject);
		}
	}
}
