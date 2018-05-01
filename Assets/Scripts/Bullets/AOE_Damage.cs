using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjT), typeof(SpriteRenderer))]
public class AOE_Damage : MonoBehaviour {

	// Settings
	public int _damage;
	public float exploRadiusMult = 1f;
	public float duration = 1f;

	// Other variables
	private List<GameObject> alreadyHit;

	// References
	private ObjT objT;
	private SpriteRenderer sr;

	// Initialization
	void Awake () {
		objT = GetComponent<ObjT> ();
		sr = GetComponent<SpriteRenderer> ();
		alreadyHit = new List<GameObject> ();

		scaleExplosion(exploRadiusMult);
	}
	
	void OnTriggerEnter2D (Collider2D col) {
		GameObject obj = col.gameObject;

		if (!alreadyHit.Contains (obj)) {
			alreadyHit.Add (obj);
			BulletScript.dealDamage (obj, _damage, transform.position, objT.typ,Vector3.zero);
		}
	}

	// Called once per frame
	void Update () {
		sr.color -= Color.black * Time.deltaTime / duration;
		if (sr.color.a <= 0f) {
			Destroy (transform.root.gameObject);
		}
	}

	// Clears the alreadyHit list so it can hit things again
	public void reset() {
		alreadyHit.Clear ();
	}

	// Scale the whole explosion up by a certain multiplier
	public void scaleExplosion (float scaleUp) {
		transform.localScale *= scaleUp;
	}
}
