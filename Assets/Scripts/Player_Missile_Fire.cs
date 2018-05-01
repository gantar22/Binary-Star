using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Player_Missile_Fire : MonoBehaviour {

	// Settings
	[SerializeField]
	XboxController _ctlr;
	[SerializeField]
	XboxButton _button;
	[SerializeField]
	GameObject _missile;

	[SerializeField]
	float _offset;

	// Other variables
	private float cooldown;

	// Upgrade properties
	private static float lvl1Radius, lvl2Radius;
	private static float lvl1Cooldown = 8f, lvl2Cooldown = 3f;

	private static bool missilesEnabled;
	private static float explosionRadius;
	private static float maxCooldown;
	private static bool tracking;
	private static bool tripleShot;


	// Initialize the upgrade settings to lvl1 if not already set
	void Start () {
		if (explosionRadius == 0f) {
			explosionRadius = lvl1Radius;
			maxCooldown = lvl1Cooldown;
		}

		// Testing:
		UnlockMissiles(1);
		EnableTracking (1);
		UpgradeCooldown (1);
	}

	// Called once per frame
	void Update () {
		if (!missilesEnabled) {
			return;
		}

		if (cooldown > 0) {
			cooldown -= Time.deltaTime;
		} else if (XCI.GetButtonDown (_button, _ctlr)) {
			fire ();
		} else if (Input.GetKey (KeyCode.X)) { // KEYBOARD TESTING --- REMOVE
			fire ();
		}
	}

	// Fire missile(s)
	private void fire() {
		// Effects

		//music_manager.Instance.shot (); // PLAY A MISSILE SHOOTING SOUND HERE
		GetComponentInChildren<ParticleSystem>().Play(); // PLays same particles as normal bullets

		CameraShakeScript CSS = Camera.main.GetComponent<CameraShakeScript> ();
		if(CSS != null){
			CSS.activate(.01f,.05f); //this feels bad
		}

		// Shoot the missile(s)
		float a = transform.eulerAngles.z * 2 * Mathf.PI / 360 ;
		spawnMissile (transform.position + new Vector3 (Mathf.Cos (a), Mathf.Sin (a), 0) * _offset);

		if (tripleShot) {
			float angleDiff = Mathf.PI / 4;
			float aLeft = a - angleDiff;
			float aRight = a + angleDiff;

			spawnMissile (transform.position + new Vector3 (Mathf.Cos (aLeft), Mathf.Sin (aLeft), 0) * _offset);
			spawnMissile (transform.position + new Vector3 (Mathf.Cos (aRight), Mathf.Sin (aRight), 0) * _offset);
		}

		cooldown = maxCooldown;
	}

	private void spawnMissile (Vector3 pos) {
		GameObject missile = Instantiate(_missile, pos, transform.rotation);
		linear_travel linTrav = missile.GetComponentInChildren<linear_travel> ();
		seeking_missile seeking = missile.GetComponentInChildren<seeking_missile> ();

		if (tracking) {
			linTrav.enabled = false;
			seeking.enabled = true;
		} else {
			linTrav.enabled = true;
			seeking.enabled = false;
		}
	}


	// ====== UPGRADES =====

	// Enable missile shooting
	public static void UnlockMissiles (int total) {
		if (total == 0) {
			missilesEnabled = false;
		} else {
			missilesEnabled = true;
		}
	}

	// Upgrade explosions
	public static void UpgradeExplosionRadius (int total) {
		if (total == 0) {
			explosionRadius = lvl1Radius;
		} else {
			explosionRadius = lvl2Radius;
		}
	}

	// Upgrades cooldown time
	public static void UpgradeCooldown (int total) {
		if (total == 0) {
			maxCooldown = lvl1Cooldown;
		} else {
			maxCooldown = lvl2Cooldown;
		}
	}

	// Enables tracking on missiles
	public static void EnableTracking (int total) {
		if (total == 0) {
			tracking = false;
		} else {
			tracking = true;
		}
	}

	// Enables triple shot missiles
	public static void EnableTripleShot (int total) {
		if (total == 0) {
			tripleShot = false;
		} else {
			tripleShot = true;
		}
	}
}
