using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour {

	// Upgrades
	public Dictionary<gunnerEffect, int> gunnerUpgrades;
	public Dictionary<pilotEffect, int> pilotUpgrades;


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

		initializeDicts ();
	}

	// Initialize the upgrade dictionaries
	private void initializeDicts() {
		gunnerUpgrades = new Dictionary<gunnerEffect, int> ();
		pilotUpgrades = new Dictionary<pilotEffect, int> ();

		foreach (gunnerEffect effect in System.Enum.GetValues (typeof(gunnerEffect))) {
			gunnerUpgrades.Add (effect, 0);
		}

		foreach (pilotEffect effect in System.Enum.GetValues (typeof(pilotEffect))) {
			pilotUpgrades.Add (effect, 0);
		}
	}


	// Call this to purchase/enable an upgrade pair
	public static void purchaseUpgrades (gunnerEffect ge, pilotEffect pe) {
		purchaseGunnerUpgrade (ge);
		purchasePilotUpgrade (pe);
	}

	// Call this to purchase/enable a gunner upgrade
	public static void purchaseGunnerUpgrade (gunnerEffect toPurchase) {
		Instance.gunnerUpgrades [toPurchase] = Instance.gunnerUpgrades [toPurchase] + 1;

		switch(toPurchase) {
		case gunnerEffect.missile:
			break;
		case gunnerEffect.sword:
			break;
		}
	}

	// Call this to purchase/enable a pilot upgrade
	public static void purchasePilotUpgrade (pilotEffect toPurchase) {
		Instance.pilotUpgrades [toPurchase] = Instance.pilotUpgrades [toPurchase] + 1;

		switch(toPurchase) {
			case pilotEffect.dash:
				break;
			case pilotEffect.turtle:
				break;
		}
	}

	// Checks if an upgrade has been purchased at all
	public static bool isPurchasedGunner(gunnerEffect ge) {
		int num = 0;
		Instance.gunnerUpgrades.TryGetValue (ge, out num);
		return (num > 0);
	}

	public static bool isPurchasedPilot(pilotEffect pe) {
		int num = 0;
		Instance.pilotUpgrades.TryGetValue (pe, out num);
		return (num > 0);
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
}
