using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum gunnerEffect {	A_multishot,	A_ricochet,				A_range,			A_wideBullets,		A_fireRate,
							missile,		missile_explosions,		missile_cooldown,	missile_tracking,	missile_tripleShot,

							sword,			sword_duration,			sword_destroyProj,	sword_range,		sword_spin,
							Y_slowMo,		Y_starMode				};

public enum pilotEffect {	health,			bombDR,					healthDR,			sprint_cooldown,	sprint_evasion,
							dash,			dash_wider,				dash_longer,		dash_cooldown,		dash_damage,
							turtle,			turtle_move,			turtle_reflect,		turtle_decoy,		turtle_duration,
							Y_rapidFire,	Y_turretMode			};

public enum X_Ability {		None, 			Dash_Missile,			Turtle_Sword, 		};




[System.Serializable]
public struct Upgrade_Option{
	public Upgrade[] choices;
}

public class UpgradesManager : MonoBehaviour {

	// Upgrades
	public static Dictionary<gunnerEffect, int> gunnerUpgrades;
	public static Dictionary<pilotEffect, int> pilotUpgrades;

	private GameObject upgrade_holder_X;
	private GameObject upgrade_holder_A;
	public GameObject upgrade_holder_A_prefab;
	public GameObject upgrade_holder_X_prefab;
	public Upgrade_Option[] upgrade_sequence;
	private int upgrade_index = 0;


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
			gunnerUpgrades.Add (effect, -1);
			purchaseGunnerUpgrade (effect);
		}

		foreach (pilotEffect effect in System.Enum.GetValues (typeof(pilotEffect))) {
			pilotUpgrades.Add (effect, -1);
			purchasePilotUpgrade (effect);
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

	// Starts the upgrade scene
	public static void Start_Upgrade_Scene(){
		Text description_ui	 = hudManager.Instance.description;
		hudManager.Instance.health.SetActive(false);
		hudManager.Instance.bomb.SetActive(false);
		if(UpgradesManager.Instance.upgrade_holder_A) Destroy(UpgradesManager.Instance.upgrade_holder_A);
		if(UpgradesManager.Instance.upgrade_holder_X) Destroy(UpgradesManager.Instance.upgrade_holder_X);
		UpgradesManager.Instance.upgrade_holder_X = Instantiate(UpgradesManager.Instance.upgrade_holder_X_prefab);
		UpgradesManager.Instance.upgrade_holder_A = Instantiate(UpgradesManager.Instance.upgrade_holder_A_prefab);
		//assign the upgrades from the sequence
		if(UpgradesManager.Instance.upgrade_sequence[UpgradesManager.Instance.upgrade_index].choices.Length == 2){
			upgrade_button[] ubs = UpgradesManager.Instance.upgrade_holder_X.GetComponentsInChildren<upgrade_button>();
			for(int i = 0; i < ubs.Length;i++){
				ubs[i].u =  UpgradesManager.Instance.upgrade_sequence[UpgradesManager.Instance.upgrade_index].choices[i];
			}
			UpgradesManager.Instance.upgrade_holder_X.SetActive(true);
		} else {
			upgrade_button[] ubs = UpgradesManager.Instance.upgrade_holder_A.GetComponentsInChildren<upgrade_button>();
			for(int i = 0; i < ubs.Length;i++){
				ubs[i].u =  UpgradesManager.Instance.upgrade_sequence[UpgradesManager.Instance.upgrade_index].choices[i];
			}
			UpgradesManager.Instance.upgrade_holder_A.SetActive(true);
		}



		description_ui.gameObject.transform.parent.gameObject.SetActive(true);

		UpgradesManager.Instance.upgrade_index++;

		PlayerHP.HP = PlayerHP.currentMaxHP;
	}

	public static void End_Upgrade_Scene(){

		Text description_ui = hudManager.Instance.description;
		hudManager.Instance.health.SetActive(true);
		hudManager.Instance.bomb.SetActive(true);
		//the buttons are children of the holder and "shut_down" deactivates them
		Transform t = UpgradesManager.Instance.upgrade_holder_X.transform;
		for(int i = 0;i < t.childCount;i++){
			t.GetChild(i).gameObject.GetComponent<Animator>().SetTrigger("shut_down");
		}
		t = UpgradesManager.Instance.upgrade_holder_A.transform;
		for(int i = 0;i < t.childCount;i++){
			t.GetChild(i).gameObject.GetComponent<Animator>().SetTrigger("shut_down");
		}
		description_ui.transform.parent.gameObject.SetActive(false);
		SpawnManager.Instance.nextSequence();
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
			Player_Fire.UpgradeFireRate (total);
		} else if (toPurchase == gunnerEffect.A_range) {
			bullet_range.upgradeRange (total);
		} else if (toPurchase == gunnerEffect.A_ricochet) {
			Player_Fire.UpgradeRicochet (total);
		} else if (toPurchase == gunnerEffect.A_multishot) {
			Player_Fire.UpgradeMultishot (total);
		} else if (toPurchase == gunnerEffect.A_wideBullets) {
			Player_Fire.UpgradeBulletWidth (total);
		} else if (toPurchase == gunnerEffect.missile) {
			Player_Missile_Fire.UnlockMissiles (total);
		} else if (toPurchase == gunnerEffect.missile_cooldown) {
			Player_Missile_Fire.UpgradeCooldown (total);
		} else if (toPurchase == gunnerEffect.missile_explosions) {
			Player_Missile_Fire.UpgradeExplosionRadius (total);
		} else if (toPurchase == gunnerEffect.missile_tripleShot) {
			Player_Missile_Fire.EnableTripleShot (total);
		} else if (toPurchase == gunnerEffect.missile_tracking) {
			Player_Missile_Fire.EnableTracking (total);
		} else if (toPurchase == gunnerEffect.sword) {
			sword_script.upgrade_enabled(total);
		} else if (toPurchase == gunnerEffect.sword_destroyProj) {
			sword_script.upgrade_projectiles(total);
		} else if (toPurchase == gunnerEffect.sword_duration) {
			sword_script.upgrade_dur(total);
		} else if (toPurchase == gunnerEffect.sword_range) {
			sword_script.upgrade_range(total);
		} else if (toPurchase == gunnerEffect.sword_spin) {
			sword_script.upgrade_spins(total);
		} else if (toPurchase == gunnerEffect.Y_slowMo) {
			YButtonManager.UnlockSlowMo (total);
		} else if (toPurchase == gunnerEffect.Y_starMode) {
			YButtonManager.UnlockStarMode (total);
		}
	}

	// Call this to purchase/enable a pilot upgrade
	public static void purchasePilotUpgrade (pilotEffect toPurchase) {
		int total = pilotUpgrades [toPurchase] + 1;
		pilotUpgrades [toPurchase] = total;

		if (toPurchase == pilotEffect.health) {
			PlayerHP.UpgradePlayerHP (total);
		} else if (toPurchase == pilotEffect.bombDR) {
			DropManager.upgradeDR (DropType.BombCharge, total);
		} else if (toPurchase == pilotEffect.healthDR) {
			DropManager.upgradeDR (DropType.Health, total);
		} else if (toPurchase == pilotEffect.sprint_cooldown) {
			PlayerMove.UpgradeSprintCooldown (total);
		} else if (toPurchase == pilotEffect.sprint_evasion) {
			PlayerHP.UpgradeEvasionOdds (total);
		} else if (toPurchase == pilotEffect.dash) {
			PlayerMove.UpgradeDashEnabled(total);
		} else if (toPurchase == pilotEffect.dash_cooldown) {
			PlayerMove.UpgradeDashCoolDown(total);
		} else if (toPurchase == pilotEffect.dash_damage) {
			PlayerMove.UpgradeDashKill(total);
		} else if (toPurchase == pilotEffect.dash_longer) {
			PlayerMove.UpgradeDashLength(total);
		} else if (toPurchase == pilotEffect.dash_wider) {
			PlayerMove.UpgradeDashLength(total);
		} else if (toPurchase == pilotEffect.turtle) {

		} else if (toPurchase == pilotEffect.turtle_decoy) {

		} else if (toPurchase == pilotEffect.turtle_duration) {

		} else if (toPurchase == pilotEffect.turtle_move) {

		} else if (toPurchase == pilotEffect.turtle_reflect) {

		} else if (toPurchase == pilotEffect.Y_rapidFire) {
			YButtonManager.UnlockRapidFire (total);
		} else if (toPurchase == pilotEffect.Y_turretMode) {
			YButtonManager.UnlockTurretMode (total);
		}
	}
}
