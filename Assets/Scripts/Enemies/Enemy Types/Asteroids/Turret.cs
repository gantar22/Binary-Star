using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class Turret : MonoBehaviour {

	// Settings
	[SerializeField]
	private float degreesOfFreedom = 180f;

	// Properties
	private float aimAngleLeeway = 4f;

	// References
	private WeightedEnemyPhysics WEP;
	private ShootsAtPlayer SAP;
	private Asteroid asteroid;


	// Initialization
	void Awake () {
		WEP = GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = 0f;

		SAP = GetComponent<ShootsAtPlayer> ();
		asteroid = GetComponentInParent<Asteroid> ();
		asteroid.addTurret (this);

		initDegreeClamp ();
	}
	
	// Called once per frame
	void Update () {
		Vector2 pos = transform.position;

		if (GM.Instance.player == null) return;
		Vector2 lookAtPos = GM.Instance.player.transform.position;

		// Decide what direction to look towards
		Vector2 direction = lookAtPos - pos;
		WEP.acceleration = direction.normalized;

		// Only enable shooting if the turret has a shot lined up with the player
		float currentAngle = transform.eulerAngles.z;
		currentAngle = WeightedEnemyPhysics.normalDegrees (currentAngle);

		float targetAngle = Mathf.Atan2 (WEP.acceleration.y, WEP.acceleration.x) * Mathf.Rad2Deg;
		targetAngle = WeightedEnemyPhysics.normalDegrees (targetAngle);

		float diff = targetAngle - currentAngle;
		diff = WeightedEnemyPhysics.normalDegrees (diff);

		SAP.shootingEnabled = (Mathf.Abs (diff) <= Mathf.Abs (aimAngleLeeway));
	}

	// Initialize the degree clamp in the WEP
	private void initDegreeClamp() {
		WEP.clampPerp = true;
		Vector2 normal = transform.position - asteroid.transform.position;
		float theta = Mathf.Rad2Deg * Mathf.Atan2 (normal.x, normal.y);

		float minDegree = (90f - degreesOfFreedom / 2) + theta;
		float maxDegree = (90f + degreesOfFreedom / 2) + theta;

		WEP.minDegree = WeightedEnemyPhysics.normalDegrees (minDegree);
		WEP.maxDegree = WeightedEnemyPhysics.normalDegrees (maxDegree);
	}
}
