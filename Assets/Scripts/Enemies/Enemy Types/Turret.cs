using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class Turret : MonoBehaviour {

	// Settings
	[SerializeField]
	private float degreesOfFreedom;

	// Properties
	private float aimAngleLeeway = 4f;

	// References
	private WeightedEnemyPhysics WEP;
	private Asteroid asteroid;


	// Initialization
	void Awake () {
		WEP = GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = 0f;

		asteroid = GetComponentInParent<Asteroid> ();
	}
	
	// Called once per frame
	void Update () {
		Vector2 pos = transform.position;

		if (GM.Instance.player == null) return;
		Vector2 lookAtPos = GM.Instance.player.transform.position;

		// Decide what direction to look towards
		Vector2 direction = lookAtPos - pos;
		WEP.acceleration = direction.normalized;
	}
}
