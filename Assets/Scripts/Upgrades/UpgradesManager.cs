using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gunnerEffect {	A_splitShot,	A_ricochet,				A_range,			A_wideBullets,		A_fireRate,
							missile,		missile_explosions,		missile_cooldown,	missile_tracking,	missile_splitShot,
							sword,			sword_duration,			sword_destroyProj,	sword_range,		sword_spin,
							Y_slowMo,		Y_starMode				};

public enum pilotEffect {	health,			bombDR,					healthDR,			sprint_cooldown,	sprint_evasion,
							dash,			dash_wider,				dash_longer,		dash_cooldown,		dash_damage,
							turtle,			turtle_move,			turtle_reflect,		turtle_decoy,		turtle_duration,
							Y_rapidFire,	Y_turretMode			};

public enum X_Ability {		None, 			Dash_Missile,			Turtle_Sword, 		};


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

		if (toPurchase == gunnerEffect.A_fireRate) {

		} else if (toPurchase == gunnerEffect.A_range) {

		} else if (toPurchase == gunnerEffect.A_ricochet) {

		} else if (toPurchase == gunnerEffect.A_splitShot) {

		} else if (toPurchase == gunnerEffect.A_wideBullets) {

		} else if (toPurchase == gunnerEffect.missile) {

		} else if (toPurchase == gunnerEffect.missile_cooldown) {

		} else if (toPurchase == gunnerEffect.missile_explosions) {

		} else if (toPurchase == gunnerEffect.missile_splitShot) {

		} else if (toPurchase == gunnerEffect.missile_tracking) {

		} else if (toPurchase == gunnerEffect.sword) {

		} else if (toPurchase == gunnerEffect.sword_destroyProj) {

		} else if (toPurchase == gunnerEffect.sword_duration) {

		} else if (toPurchase == gunnerEffect.sword_range) {

		} else if (toPurchase == gunnerEffect.sword_spin) {

		} else if (toPurchase == gunnerEffect.Y_slowMo) {

		} else if (toPurchase == gunnerEffect.Y_starMode) {

		}
	}

	// Call this to purchase/enable a pilot upgrade
	public static void purchasePilotUpgrade (pilotEffect toPurchase) {
		int total = pilotUpgrades [toPurchase] + 1;
		pilotUpgrades [toPurchase] = total;

		if (toPurchase == pilotEffect.health) {

		} else if (toPurchase == pilotEffect.bombDR) {

		} else if (toPurchase == pilotEffect.healthDR) {

		} else if (toPurchase == pilotEffect.sprint_cooldown) {

		} else if (toPurchase == pilotEffect.sprint_evasion) {

		} else if (toPurchase == pilotEffect.dash) {

		} else if (toPurchase == pilotEffect.dash_cooldown) {

		} else if (toPurchase == pilotEffect.dash_damage) {

		} else if (toPurchase == pilotEffect.dash_longer) {

		} else if (toPurchase == pilotEffect.dash_wider) {

		} else if (toPurchase == pilotEffect.turtle) {

		} else if (toPurchase == pilotEffect.turtle_decoy) {

		} else if (toPurchase == pilotEffect.turtle_duration) {

		} else if (toPurchase == pilotEffect.turtle_move) {

		} else if (toPurchase == pilotEffect.turtle_reflect) {

		} else if (toPurchase == pilotEffect.Y_rapidFire) {

		} else if (toPurchase == pilotEffect.Y_turretMode) {

		}
	}
}
