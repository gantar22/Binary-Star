using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjT))]
public class AOE_Damage : MonoBehaviour {

	// Settings
	public int _damage;

	// Other variables
	private List<GameObject> alreadyHit;

	// References
	private ObjT objT;

	// Initialization
	void Awake () {
		objT = GetComponent<ObjT> ();
		alreadyHit = new List<GameObject> ();
	}
	
	void OnTriggerEnter2D (Collider2D col) {
		GameObject obj = col.gameObject;

		if (!alreadyHit.Contains (obj)) {
			alreadyHit.Add (obj);
			BulletScript.dealDamage (obj, _damage, transform.position, objT.typ);
		}
	}

	// Clears the alreadyHit list so it can hit things again
	public void reset() {
		alreadyHit.Clear ();
	}
}
