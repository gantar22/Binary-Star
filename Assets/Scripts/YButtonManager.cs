using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class YButtonManager : MonoBehaviour {

	// Settings
	public float shortCooldown = 30;
	public float longCooldown = 50;

	[SerializeField]
	XboxController _pilot_ctlr;
	[SerializeField]
	XboxController _gunner_ctlr;
	XboxButton _button = XboxButton.Y;

	// Global variables
	private float cooldown;

	private bool rapidFire_active, turretMode_active;
	private bool slowMo_active, starMode_active;

	// Values relevant to upgrades
	private static bool shorterCooldown;

	private static bool pilot_rapidFire_unlocked, pilot_turretMode_unlocked;
	private static bool gunner_slowMo_unlocked, gunner_starMode_unlocked;

	// Ability script references here
	private SlowMo slowMo;
	private RapidFire rapidFire;
	private TurretMode turretMode;


	// Initialization
	void Start() {
		slowMo = transform.root.GetComponentInChildren<SlowMo> ();
		rapidFire = transform.root.GetComponentInChildren<RapidFire> ();
		turretMode = transform.root.GetComponentInChildren<TurretMode> ();
		// Assign other ability references here

		// Testing:
		//UnlockSlowMo(1);
		//UnlockRapidFire(1);
		//UnlockTurretMode(1);
	}


	// Returns the cooldown left, or the remaining duration of the active ability
	public float getYBarValue() {
		float returnVal = 0;
		if (!abilityIsActive ()) {
			returnVal = cooldown / (shorterCooldown ? shortCooldown : longCooldown);
		} else if (slowMo_active) {
			returnVal = 1 - slowMo.realSlowTimeLeft / slowMo.realtimeDuration;
		} else if (rapidFire_active) {
			returnVal = 1 - rapidFire.timeLeft / rapidFire.duration;
		} else if (turretMode_active) {
			returnVal = 1 - turretMode.timeLeft / turretMode.duration;
		}
		// ADD OTHER ABILITY DURATIONS HERE

		return Mathf.Clamp(returnVal, 0, 1);
	}


	// Update is called once per frame
	void Update () {
		if (!abilityIsUnlocked()) {
			return;
		}

		if (cooldown >= 0) {
			if (!abilityIsActive()) {
				cooldown -= Time.deltaTime;
			}
			return;
		} else {
			if (PlayerHP.invuln) {
				return;
			}

			if (XCI.GetButtonDown (_button, _pilot_ctlr) || Input.GetKey (KeyCode.Y)) {
				if (pilot_rapidFire_unlocked) {
					activateRapidFire ();
				} else if (pilot_turretMode_unlocked) {
					activateTurretMode ();
				}
			} else if (XCI.GetButtonDown (_button, _gunner_ctlr) || Input.GetKey (KeyCode.U)) {
				if (gunner_slowMo_unlocked) {
					activateSlowMo ();
				} else if (gunner_starMode_unlocked) {
					activateStarMode ();
				}
			}
		}
	}

	// Functions to activate abilities
	private void activateRapidFire() {
		rapidFire_active = true;
		rapidFire.startRapidFire ();
	}

	private void activateTurretMode() {
		turretMode_active = true;
		turretMode.startTurretMode ();
	}

	private void activateSlowMo() {
		slowMo_active = true;
		slowMo.startSlowMo ();
	}

	private void activateStarMode() {
		starMode_active = true;
		// TODO - Start starMode here
	}


	// Function to call once finished using an ability
	public void finishedAbility() {
		rapidFire_active = false;
		turretMode_active = false;
		slowMo_active = false;
		starMode_active = false;

		cooldown = shorterCooldown ? shortCooldown : longCooldown;
	}

	// Refresh the YButton cooldown
	public void refreshCooldown() {
		cooldown = 0;
	}


	// Returns true if any ability is currently active
	public bool abilityIsActive() {
		return (rapidFire_active || turretMode_active || slowMo_active || starMode_active);
	}

	// Returns true if any ability has been unlocked
	public static bool abilityIsUnlocked() {
		return (pilot_rapidFire_unlocked || pilot_turretMode_unlocked || gunner_slowMo_unlocked || gunner_starMode_unlocked);
	}


	// Unlocking abilities
	public static void UnlockRapidFire (int total) {
		if (total == 0) {
			pilot_rapidFire_unlocked = false;
		} else {
			pilot_rapidFire_unlocked = true;
			shorterCooldown = true;
		}
	}

	public static void UnlockTurretMode (int total) {
		if (total == 0) {
			pilot_turretMode_unlocked = false;
		} else {
			pilot_turretMode_unlocked = true;
			shorterCooldown = false;
		}
	}

	public static void UnlockSlowMo (int total) {
		if (total == 0) {
			gunner_slowMo_unlocked = false;
		} else {
			gunner_slowMo_unlocked = true;
			shorterCooldown = true;
		}
	}

	public static void UnlockStarMode (int total) {
		if (total == 0) {
			gunner_starMode_unlocked = false;
		} else {
			gunner_starMode_unlocked = true;
			shorterCooldown = false;
		}
	}
}
