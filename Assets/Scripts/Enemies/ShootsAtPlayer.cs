using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootsAtPlayer : MonoBehaviour {

	// Settings
	[SerializeField]
	private GameObject _bullet;
	[SerializeField]
	private float _offset, _kick = 1f;
	[SerializeField, Range(.01f, 5)]
	private float _fire_rate;

	// Other variables
	private bool cool_down;
	[HideInInspector]
	public bool shootingEnabled = true;

	// Object references
	private WeightedEnemyPhysics WEP;


	// Initialization
	void Start () {
		WEP = GetComponent<WeightedEnemyPhysics> ();

		shootingEnabled = true;
		cool_down = true;
		Invoke ("reload", 1 / _fire_rate);
	}
	
	// Called once per frame
	void Update () {
		if (killIfOOB.is_OOB (transform.position)) {
			return;
		}

		if (!cool_down) {
			fire ();
		}
	}

	// Shoot at the player
	void fire() {
		if (GM.Instance.player == null || !shootingEnabled) {
			return;
		}

		cool_down = true;
		Invoke("reload",1 / _fire_rate);

		// For the ship's angle, in degrees:
		//float a = transform.eulerAngles.z * 2 * Mathf.PI / 360 ;

		// Get the shoot direction and angle for the bullet
		Vector3 shootDirection = (GM.Instance.player.transform.position - transform.position).normalized;
		float targetAngle = Mathf.Atan2 (shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
		float currentAngle = transform.eulerAngles.z;
		float diff = targetAngle - currentAngle;

		// Instantiate the bullet and set to the right direction and speed
		GameObject bul = Instantiate(_bullet, transform.position + shootDirection * _offset, transform.rotation);
		bul.transform.Rotate (Vector3.forward * diff);

		if (WEP != null) {
			setBulletSpeed (bul, WEP.velocity.magnitude);

			// Enemy should be kicked back a bit after firing
			WEP.velocity -= (Vector2)shootDirection * _kick;
		}
	}

	// Set the fire cooldown to false
	public void reload() {
		CancelInvoke ();
		cool_down = false;
	}

	// Set the speed of the bullet that was just shot
	private void setBulletSpeed(GameObject bullet, float speed) {
		linear_travel LT = bullet.GetComponentInChildren<linear_travel> ();
		if (LT != null) {
			LT.setSpeed (speed);
			return;
		}

		// seeking_missile starts with 0 velocity
		// sin_travel starts with fixed velocity
	}
}
