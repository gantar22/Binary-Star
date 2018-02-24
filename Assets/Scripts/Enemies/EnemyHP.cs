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
		HP -= dmg;

		if (HP <= 0) {
			// DO SOMETHING ELSE WHEN ENEMY IS DESTROYED? - TODO
			Destroy (gameObject);
		}
	}
}
