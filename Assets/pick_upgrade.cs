using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pick_upgrade : MonoBehaviour {


	private Upgrade u;
	private bool got;
	// Use this for initialization
	void Start () {
		got = false;
		u = GetComponentInParent<upgrade_button>().u;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.GetComponent<BulletScript>() && other.gameObject.GetComponent<ObjT>() && other.gameObject.GetComponent<ObjT>().typ == ObjT.obj.player_bullet && !got){
			GetComponentInParent<Animator>().SetTrigger("shut_down");
			UpgradesManager.purchaseUpgrades(u.gunEffect,u.pilEffect);
			got = true;
			Invoke("end",.4f);
		}
	}


	void end(){
		UpgradesManager.End_Upgrade_Scene();
	}

}

