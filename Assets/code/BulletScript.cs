using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
	public int _damage;

	public enum bulletType {basic}

	public bulletType _type;

	private ObjT objT;

	// Variables to make sure bullet only hits one thing
	private GameObject objToHit;
	private bool hitInvulnerable;


	void Awake () {
		objT = GetComponent<ObjT> ();
		hitInvulnerable = false;
	}

	// Called once per frame, after all physics & collision checks
	void Update () {
		// If the bullet has collided with anything, none of which is invulnerable, then destroy one
		if (objToHit != null) {
			hitObject (objToHit);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (hitInvulnerable) {
			return;
		}

		Invulnerable I = col.gameObject.GetComponent<Invulnerable> ();
		if (I != null && I.enabled) {
			hitInvulnerable = true;
			I.gotHit ();
			die ();
		} else {
			objToHit = col.gameObject;
		}
	}

	private void hitObject (GameObject obj) {
		bull2 b = obj.GetComponent<bull2>();
		if(b){
			b.hit(transform.position);
		}

		EnemyHP s = obj.GetComponent<EnemyHP>(); 
		if (s != null){
			s.gotHit(_damage);
			die();
		}

		if(obj.layer == 9){
			die();
		}

		PlayerHP PHP = obj.GetComponent<PlayerHP> ();
		if (PHP != null && (objT.typ != ObjT.obj.player_bullet)) {
			PHP.gotHit (_damage);
			die ();
		}
	}

	void die(){
		switch(_type){
			case bulletType.basic:
			break;
		}
		Destroy(transform.root.gameObject);
	}



}
