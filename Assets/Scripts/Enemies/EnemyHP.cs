using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private int maxHP = 2;
	[SerializeField]
	private bool guaranteeDrop = false;

	// Other variables
	private int HP;


	// Initialize
	void Start () {
		HP = maxHP;
	}

	// Called when damage is taken
	public void gotHit(int dmg) {
		HP -= dmg;
		if (HP <= 0) {
			die ();
		}
	}

	// Called when this enemy should die
	public void die() {
		GM.Instance.enemyCount--;

		// Make sure reticle is unparented
		UnParentOnDestroy retScript;
		if((retScript = GetComponentInChildren<UnParentOnDestroy>()) != null){
			retScript.gameObject.transform.parent = null;
		}

		// Camera shake
		CameraShakeScript CSS = Camera.main.GetComponent<CameraShakeScript> ();
		if(CSS != null){
			CSS.activate(.03f,.03f);
		}

		// Spawn drops
		if (guaranteeDrop) {
			DropManager.Instance.SpawnDrop (transform.position);
		} else {
			DropManager.Instance.MaybeDrop (maxHP, transform.position);
		}

		Destroy (gameObject);
	}
}
