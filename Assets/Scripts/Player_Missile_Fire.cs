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
	[SerializeField]
	int _explosionDamage;

	// Other variables
	private float cooldown;

	// References
	private Player_Fire player_fire;

	// Upgrade properties
	private static float lvl1ExploRadiusMult = 1.8f, lvl2ExploRadiusMult =  3.6f;
	private static float lvl1Cooldown = 8f, lvl2Cooldown = 3f;
	private static float angleDiffDegrees = 35;

	public static bool missilesEnabled;
	private static float exploRadiusMult;
	private static float maxCooldown;
	private static bool tracking;
	private static bool tripleShot;


	// Initialize the upgrade settings to lvl1 if not already set
	void Start () {
		player_fire = GetComponent<Player_Fire> ();

		if (exploRadiusMult == 0f) {
			exploRadiusMult = lvl1ExploRadiusMult;
			maxCooldown = lvl1Cooldown;
		}

		// Testing:
		//UnlockMissiles(1);
		//EnableTracking (1);
		//EnableTripleShot(1);
		//UpgradeCooldown (1);
		//UpgradeExplosionRadius (1);
	}

	// Called once per frame
	void Update () {
		if (!missilesEnabled) {
			return;
		}

		if (cooldown > 0) {
			cooldown -= Time.deltaTime;
		} else if (!player_fire.cantFire) {
			if (XCI.GetButtonDown (_button, _ctlr)) {
				fire ();
			}
			#if UNITY_EDITOR
			else if (Input.GetKey (KeyCode.X)) { // KEYBOARD TESTING --- REMOVE
				fire ();
			}
			#endif
		}
	}

	public static float get_cooldown(){
		if(missilesEnabled) return 1 - (GM.Instance.player.GetComponentInChildren<Player_Missile_Fire>().cooldown / maxCooldown);
		return -1;
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
		spawnMissileAtDegreeOffset (0, tracking);

		if (tripleShot) {
			spawnMissileAtDegreeOffset (angleDiffDegrees, tracking);
			spawnMissileAtDegreeOffset (-1 * angleDiffDegrees, tracking);
		}

		cooldown = maxCooldown;
	}

	// Spawn a missile with degree offset of angleDiffDegr
	public void spawnMissileAtDegreeOffset (float angleDiffDegr, bool seekingEnabled) {
		float angleDiffRad = angleDiffDegr * Mathf.Deg2Rad;

		float a = transform.eulerAngles.z * Mathf.Deg2Rad;
		a += angleDiffRad;

		Vector3 offset = new Vector3 (Mathf.Cos (a), Mathf.Sin (a), 0) * _offset;

		// Spawn the missile
		Vector3 pos = transform.position + offset;

		GameObject missile = Instantiate(_missile, pos, transform.rotation);
		linear_travel linTrav = missile.GetComponentInChildren<linear_travel> ();
		seeking_missile seeking = missile.GetComponentInChildren<seeking_missile> ();

		//Vector2 playerVelo = transform.root.gameObject.GetComponentInChildren<PlayerMove> ().velo;

		if (seekingEnabled) {
			linTrav.enabled = false;
			seeking.enabled = true;

			//seeking.setVelo (playerVelo);
			Vector2 direction = pos - transform.position;
			float speed = linTrav.getBaseSpeed;
			seeking.setVelo (direction, speed);
		} else {
			linTrav.enabled = true;
			seeking.enabled = false;

			//linTrav.setSpeed(playerVelo.magnitude);
		}

		// Set the correct rotation
		missile.transform.Rotate(new Vector3(0, 0, angleDiffDegr));

		// Set its explosion settings
		BulletScript BS = missile.GetComponentInChildren<BulletScript>();
		BS.setExploSettings(exploRadiusMult, _explosionDamage);
	}

	public void refreshCooldown() {
		cooldown = 0;
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
			exploRadiusMult = lvl1ExploRadiusMult;
		} else {
			exploRadiusMult = lvl2ExploRadiusMult;
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
