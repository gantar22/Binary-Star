using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjT))]
public class BulletScript : MonoBehaviour {
	public int _damage;

	public enum bulletType {basic}

	public bulletType _type;

	private ObjT objT;

	// Variables to make sure bullet only hits one thing
	//private GameObject objToHit;
	private List<GameObject> objsToHit;
	private bool hitInvulnerable;


	void Awake () {
		objT = GetComponent<ObjT> ();
		objsToHit = new List<GameObject> ();
		hitInvulnerable = false;
	}

	// Called once per frame, after all physics & collision checks
	void Update () {
		// If the bullet has collided with anything, then damage one.
		// If it only hits something invulnerable, then hit it. Otherwise damage something else
		bool damagedSomething = false;
		Invulnerable invulnToHit = null;

		while (objsToHit.Count > 0) {
			GameObject nextObj = objsToHit [0];

			Invulnerable I = nextObj.GetComponent<Invulnerable> ();
			if (I != null && I.enabled) {
				invulnToHit = I;
				objsToHit.Remove (nextObj);
			} else if (dealDamage (nextObj, _damage, transform.position, objT.typ)) {
				die ();
				objsToHit.Clear ();
				damagedSomething = true;
			} else {
				objsToHit.Remove (nextObj);
			}
		}

		if (!damagedSomething && invulnToHit != null) {
			invulnToHit.gotHit ();
			die ();
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		objsToHit.Add (col.gameObject);

		/* if (hitInvulnerable) {
			return;
		}

		Invulnerable I = col.gameObject.GetComponent<Invulnerable> ();
		if (I != null && I.enabled) {
			hitInvulnerable = true;
			I.gotHit ();
			die ();
		} else {
			objsToHit.Add (col.gameObject);
		} */
	}

	// Anything that does damage should call this to damage whatever it hit.
	// Returns true if it actually deals damage (so that the bullet should die, etc.)
	public static bool dealDamage (GameObject obj, int damage, Vector3 pos, ObjT.obj type) {
		if (obj == null) {
			return false;
		}

		bull2 b = obj.GetComponent<bull2>();
		if(b){
			b.hit(pos);
		}

		EnemyHP s = obj.GetComponent<EnemyHP>(); 
		if (s != null){
			s.gotHit(damage);
			return true;
		}

		if(obj.layer == 9){
			return true;
		}

		PlayerHP PHP = obj.GetComponent<PlayerHP> ();
		if (PHP != null && (type != ObjT.obj.player_bullet) && (type != ObjT.obj.player_explosion)) {
			PHP.gotHit (damage);
			return true;
		}

		upgrade_button ub;
		if(ub = obj.GetComponentInParent<upgrade_button>()){
			ub.put_description();
			return true; // I beleive this will destroy the bullet
		}

		return false;
	}

	void die(){
		switch(_type){
			case bulletType.basic:
			ParticleSystem ps = transform.GetComponentInChildren<ParticleSystem>();
			ps.transform.parent = null;
			ps.Play();
			break;
		}
		Destroy(transform.root.gameObject);
	}



}
