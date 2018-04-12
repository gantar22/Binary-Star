using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private int maxHP = 2;

	// Other variables
	private int HP;

	// Object references


	// Initialize
	void Start () {
		HP = maxHP;
	}

	// Called when damage is taken
	public void gotHit(int dmg) {
		UnParentOnDestroy retScript;
		if(special()) HP -= dmg;
		if (HP <= 0) {
			GM.Instance.enemyCount--;
			if((retScript = GetComponentInChildren<UnParentOnDestroy>()) != null){
				retScript.gameObject.transform.parent = null;
			}

			if(Camera.main.GetComponent<CameraShakeScript>() != null){
				Camera.main.GetComponent<CameraShakeScript>().activate(.03f,.03f);
			}

			Destroy (gameObject);
		}
	}

	private bool special(){ //special script checks for taking damage
		bull2 b;
		if((b = GetComponent<bull2>())){
			if(b._state != bull2.state.stunned) return false;
			if(transform.childCount > 0 && transform.GetChild(0).GetComponent<Animator>()) transform.GetChild(0).GetComponent<Animator>().SetTrigger("hit");
			return (--b.hp == 0);
		}
		return true;
	}
}
