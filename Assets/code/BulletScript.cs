using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
	public int _damage;

	public enum bulletType {basic}

	public bulletType _type;

	[SerializeField]
	
	void OnTriggerEnter2D(Collider2D col){

		bull2 b = col.gameObject.GetComponent<bull2>();
		if(b){
			b.hit(transform.position);
		}

		Invulnerable I = col.gameObject.GetComponent<Invulnerable> ();
		if (I != null && I.enabled) {
			I.gotHit ();
			die ();
			return;
		}

		BulletScript bs = col.gameObject.GetComponent<BulletScript> ();
		if (bs != null) {
			bs.die ();
			die ();
		}

		EnemyHP s = col.gameObject.GetComponent<EnemyHP>(); 
		if (s != null){
			s.gotHit(_damage);
			die();
		}

		if(col.gameObject.layer == 9){
			die();
		}

		PlayerHP PHP = col.gameObject.GetComponent<PlayerHP> ();
		if (PHP != null) {
			PHP.gotHit (_damage);
			die ();
		}
	}

	void die(){
		//Destroy(gameObject);
		Destroy(transform.root.gameObject);
		switch(_type){
			case bulletType.basic:
			break;
		}
	}



}
