using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
	public int _damage;

	public enum bulletType {basic}

	public bulletType _type;

	[SerializeField]
	
	OnTriggerEnter2D(Collider2D col){
		s = col.gameObject.GetComponent<EnemyScript>()
		if(s != null){
			s.SetHP(int);
			die();
		}
	}

	void die(){
		
	}



}
