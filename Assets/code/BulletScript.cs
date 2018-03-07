using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
	public int _damage;

	public enum bulletType {basic}

	public bulletType _type;

	[SerializeField]
	
	void OnTriggerEnter2D(Collider2D col){
		EnemyHP s = col.gameObject.GetComponent<EnemyHP>(); 
		if (s != null){
			s.gotHit(_damage);
			die();
		}
	}

	void die(){
		Destroy(gameObject);
		switch(_type){
			case bulletType.basic:
			break;
		}
	}



}
