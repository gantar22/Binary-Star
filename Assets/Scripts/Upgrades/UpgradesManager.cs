using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gunnerEffect {	sword, missile, 
							A_splitShot,			A_ricochet,			A_range,			A_wideBullets,		A_fireRate,
							laser_duration,			laser_destroyProj,	laser_range,		laser_spin,
							missile_explosions,		missile_cooldown,	missile_trackinng,	missile_splitShot,
							Y_slowMo,				Y_starMode			};

public enum pilotEffect {	turtle,					dash, 
							health,					bombDR,				healthDR,			sprintEvasion,		sprintCooldown,
							turtle_move,			turtle_reflect,		turtle_decoy,		turtle_duration,
							dash_wider,				dash_longer,		dash_cooldown,		dash_damage,
							Y_rapidFire,			Y_turretMode		};

public enum X_Ability {		None, 					Turtle_Sword, 		Dash_Missile};


public class UpgradesManager : MonoBehaviour {

	// Upgrades
	public static Dictionary<gunnerEffect, int> gunnerUpgrades;
	public static Dictionary<pilotEffect, int> pilotUpgrades;
	public Text description_ui;


	// Singleton
	private static UpgradesManager _instance;
	public static UpgradesManager Instance { get { return _instance; } }

	// Initialization
	private void Awake() {
		// Static instance setup
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}

		if (transform.parent == null) {
			DontDestroyOnLoad (this);
		}

		resetUpgrades ();
	}

	// Initialize the upgrade dictionaries so that every upgrade is at 0
	public static void resetUpgrades() {
		gunnerUpgrades = new Dictionary<gunnerEffect, int> ();
		pilotUpgrades = new Dictionary<pilotEffect, int> ();

		foreach (gunnerEffect effect in System.Enum.GetValues (typeof(gunnerEffect))) {
			gunnerUpgrades.Add (effect, 0);
		}

		foreach (pilotEffect effect in System.Enum.GetValues (typeof(pilotEffect))) {
			pilotUpgrades.Add (effect, 0);
		}
	}


	// Checks if an upgrade has been purchased at all
	public static bool isPurchasedGunner(gunnerEffect ge) {
		int num = 0;
		gunnerUpgrades.TryGetValue (ge, out num);
		return (num > 0);
	}

	public static bool isPurchasedPilot(pilotEffect pe) {
		int num = 0;
		pilotUpgrades.TryGetValue (pe, out num);
		return (num > 0);
	}

	// Check if an X_Ability prereq is met
	public static bool XPrereqMet (X_Ability prereq) {
		if (prereq == X_Ability.Dash_Missile) {
			return (getDash ());
		} else if (prereq == X_Ability.Turtle_Sword) {
			return (getTurtle ());
		}

		return true;
	}

	// Access wrappers for the X/Y button abilities
	public static bool getMissile() {
		return isPurchasedGunner (gunnerEffect.missile);
	}

	public static bool getSword() {
		return isPurchasedGunner (gunnerEffect.sword);
	}

	public static bool getTurtle() {
		return isPurchasedPilot (pilotEffect.turtle);
	}

	public static bool getDash() {
		return isPurchasedPilot (pilotEffect.dash);
	}

	// ====== INDIVIDUAL UPGRADE IMPLEMENTATIONS HERE ======

	// Call this to purchase/enable an upgrade pair
	public static void purchaseUpgrades (gunnerEffect ge, pilotEffect pe) {
		purchaseGunnerUpgrade (ge);
		purchasePilotUpgrade (pe);
	}

	// Call this to purchase/enable a gunner upgrade
	public static void purchaseGunnerUpgrade (gunnerEffect toPurchase) {
		int total = gunnerUpgrades [toPurchase] + 1;
		gunnerUpgrades [toPurchase] = total;

		switch(toPurchase) {
		case gunnerEffect.missile:
			break;
		case gunnerEffect.sword:
			break;
		}

		// --- OR ---

		if (toPurchase == gunnerEffect.missile) {

		} else if (toPurchase == gunnerEffect.sword) {

		}
	}

	// Call this to purchase/enable a pilot upgrade
	public static void purchasePilotUpgrade (pilotEffect toPurchase) {
		int total = pilotUpgrades [toPurchase] + 1;
		pilotUpgrades [toPurchase] = total;

		switch(toPurchase) {
			case pilotEffect.dash:
				break;
			case pilotEffect.turtle:
				break;
		}
	}
}
