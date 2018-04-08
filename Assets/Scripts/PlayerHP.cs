using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private int maxHP = 3;


	// Initialize
	void Start () {
		GM.Instance.playerHP = maxHP;
	}

	/* //Testing:
	void Update() {
		print (GM.Instance.playerHP);
	} */



	// Check for collisions with enemies
	void OnTriggerEnter2D(Collider2D col){
		EnemyHP s = col.gameObject.GetComponent<EnemyHP>(); 
		if (s != null){
			gotHit();
			if (s.gameObject.GetComponent<Asteroid> () == null) {
				s.die ();
			}
		}

		// Bullet and HP drop collisions managed by BulletScript.cs and HPDrop.cs
	}

	// Regain moreHP more HP, up to max, and return true. If already at max, return false
	public bool gainHP(int moreHP) {
		int currentHP = GM.Instance.playerHP;
		if (currentHP >= maxHP) {
			return false;
		} else {
			GM.Instance.playerHP = Mathf.Min (maxHP, currentHP + moreHP);
			return true;
		}
	}

	// Called when damage is taken
	public void gotHit(int dmg = 1) {
		GM.Instance.playerHP -= dmg;
		if (GM.Instance.playerHP <= 0) {
			die ();
		} else {
			// Got hit but didn't die. TODO - Play blinking animation/temporary invlunerability?
		}
	}

	// Player runs out of health or dies by other means
	public void die() {
		// TODO - What happens when the player dies/loses?

		CameraShakeScript CSS = Camera.main.GetComponent<CameraShakeScript> ();
		if(CSS != null){
			CSS.activate(.1f,.1f);
		}

		Destroy (gameObject.transform.parent.gameObject);
	}
}
