using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootsAtPlayer : MonoBehaviour {

	// Settings
	[SerializeField]
	private GameObject _bullet;
	[SerializeField]
	private float _offset;
	[SerializeField, Range(.01f, 10)]
	private float _fire_rate;

	// Other variables
	private bool cool_down;

	// Object references
	private WeightedEnemyPhysics WEP;


	// Initialization
	void Start () {
		WEP = GetComponent<WeightedEnemyPhysics> ();

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
		if (GM.Instance.player == null) {
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

		bul.GetComponentInChildren<linear_travel>().setSpeed(WEP.velocity.magnitude);
	}

	// Set the fire cooldown to false
	void reload() {
		cool_down = false;
	}
}
