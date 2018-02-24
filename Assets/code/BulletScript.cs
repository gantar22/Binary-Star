using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
	public int _damage;

	public enum bulletType {basic}

	public bulletType _type;

	[SerializeField]
	
	void OnTriggerEnter2D(Collider2D col){
		BulletScript s = col.gameObject.GetComponent<BulletScript>(); //enemyscript
		if (s != null){
			//s.SetHP(_damage);
			die();
		}
	}

	void die(){

	}



}
