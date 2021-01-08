using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour {

	// Settings
	public bool KillIfVeryOOB = true;
	public float extraScreenRadius = 3f;


	// Called once per frame
	void Update () {
		Vector2 velo = ScrollManager.scrollVelo;
		transform.position += new Vector3(velo.x, velo.y, 0) * Time.deltaTime;

		if (KillIfVeryOOB) {
			checkScrollBounds ();
		}
	}

	// Destroy this gameObject if it is OOB in current scroll direction
	private void checkScrollBounds() {
		Vector3 pos = transform.position;
		Vector2 SVelo = ScrollManager.scrollVelo;

		if (( SVelo.x >= 0 && (Camera.main.WorldToViewportPoint(pos - Vector3.right * extraScreenRadius).x > 1f)	) ||
			( SVelo.x <= 0 && (Camera.main.WorldToViewportPoint(pos - Vector3.left * extraScreenRadius).x < 0f)		) ||
			( SVelo.y >= 0 && (Camera.main.WorldToViewportPoint(pos - Vector3.up * extraScreenRadius).y > 1f)		) ||
			( SVelo.y <= 0 && (Camera.main.WorldToViewportPoint(pos - Vector3.down * extraScreenRadius).y < 0f)		) ) {
			killThisObj ();
		}

	}

	// Destroy this gameObject
	private void killThisObj() {
		EnemyHP[] EHPs = gameObject.GetComponentsInChildren<EnemyHP> ();
		foreach (EnemyHP EHP in EHPs) {
			EHP.die ();
		}
		Destroy(gameObject);
	}
}
