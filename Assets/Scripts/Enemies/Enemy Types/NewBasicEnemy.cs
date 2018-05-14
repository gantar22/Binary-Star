using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class NewBasicEnemy : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private float maxSpeed = 3.2f, accelMag = 0.1f;

	// Object references
	private WeightedEnemyPhysics WEP;


	// Initialize
	void Start () {
		WEP = GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = maxSpeed;
	}

	// Called every frame
	void Update () {
		Vector2 pos = transform.position;

		if (GM.Instance.player == null) return;
		Vector2 targetPos = GM.Instance.player_pos;

		// Decide what direction to move in
		Vector2 direction = targetPos - pos;

		// Normalize the velocity and set to desired speed
		WEP.acceleration = direction.normalized * accelMag;
	}
}
