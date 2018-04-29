﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private float invulnTime = 3f, minFrequency = 2f, maxFrequency = 6f, minAlpha = 0.3f, maxAlpha = 0.9f;
	[SerializeField]
	private GameObject PlayerTurret;

	// Other variables
	public static bool invuln, checkColliders;
	private float invulnTimer;
	private float alphaCounter;

	// References
	private SpriteRenderer thisSR;
	private SpriteRenderer PlayerTurretSR;
	private Player_Fire player_Fire;

	// Sprint chance
	private static float stillHitOdds = 1f;

	// THE PLAYER's HP
	private static int startingMaxHP = 3;
	[HideInInspector]
	public static int HP, currentMaxHP;


	void Awake(){
		if(GM.Instance && SpawnManager.Instance){
			GM.Instance.player = transform.parent.gameObject;
			SpawnManager.Instance.sequenceIndex = 0;
			if(SpawnManager.Instance.idle){
				SpawnManager.Instance.idle = true;
				SpawnManager.Instance.nextSequence();
			}
		}

	}

	// Initialize
	void Start () {
		thisSR = GetComponent<SpriteRenderer> ();
		PlayerTurretSR = PlayerTurret.GetComponent<SpriteRenderer> ();
		player_Fire = PlayerTurret.GetComponent<Player_Fire> ();

		HP = startingMaxHP;
		invuln = false;
		checkColliders = false;
		setAllColorScale (1);
	}
	
	// Called once per frame. Manages invuln timer and alpha
	void Update() {
		if (invuln) {
			//float frequency = Mathf.Lerp (minFrequency, maxFrequency, (invulnTime - invulnTimer) / invulnTime);
			float frequency = Mathf.Lerp (minFrequency, maxFrequency, Mathf.Pow((invulnTime - invulnTimer) / invulnTime, 2));
			alphaCounter += frequency * Time.deltaTime * (2f * Mathf.PI);
			float alphaScale = (Mathf.Sin (alphaCounter) + 1f) / 2f;
			float newAlpha = Mathf.Lerp (minAlpha, maxAlpha, alphaScale);

			invulnTimer -= Time.deltaTime;
			if (invulnTimer <= 0f) {
				toInvuln (false);
			} else {
				setAllColorScale (newAlpha);
			}
		}
	}

	// Upgrade player maximum HP
	public static void UpgradePlayerHP (int total) {
		if (total == 0) {
			currentMaxHP = startingMaxHP;
		} else {
			currentMaxHP++;
			if (GM.Instance && GM.Instance.player && GM.Instance.player.GetComponent<PlayerHP>()) {
				GM.Instance.player.GetComponent<PlayerHP> ().gainHP (1);
			}
		}
	}

	// Upgrade sprint evasion chance
	public static void UpgradeEvasionOdds (int total) {
		if (total == 0) {
			stillHitOdds = 1f;
		} else {
			stillHitOdds *= 0.8f;
		}
	}


	// Switch to invulnerable, or no longer invulnerable
	private void toInvuln (bool nowInvuln) {
		if (invuln == nowInvuln) {
			return;
		}

		invuln = nowInvuln;
		player_Fire.cantFire = nowInvuln;

		if (nowInvuln) {
			invulnTimer = invulnTime;
			alphaCounter = Mathf.PI / -2f;
		} else {
			setAllColorScale (1);
			checkColliders = true;
			Invoke ("disableCC", 0.5f);
		}
	}

	// Check for collisions with enemies, and stay out of asteroids
	void OnTriggerEnter2D (Collider2D col){
		if (invuln) {
			return;
		}

		EnemyHP s = col.gameObject.GetComponent<EnemyHP>(); 
		if (s != null){
			gotHit();
			if (s.gameObject.GetComponent<Rock> () == null) {
				s.gotHit (col.transform.position - transform.position,4, true);
			}
		}

		// Bullet and HP drop collisions managed by BulletScript.cs and HPDrop.cs
	}

	// When invulnerability is over, check once for anything currently in the collider
	void OnTriggerStay2D (Collider2D col) {
		if (checkColliders) {
			OnTriggerEnter2D (col);
		}
	}

	// Invoke this with a delay to disable checkColliders
	private void disableCC() {
		checkColliders = false;
	}


	// Regain moreHP more HP, up to max, and return true. If already at max, return false
	public bool gainHP(int moreHP) {
		if (HP >= currentMaxHP) {
			return false;
		} else {
			HP = Mathf.Min (currentMaxHP, HP + moreHP);
			return true;
		}
	}

	public bool gainBombCharge(int charge){
		bomb_det bd = GetComponent<bomb_det>();
		if(bd){
			return bd.addCharge(charge);
		}
		return false;
	}

	// Called when damage is taken
	public void gotHit(int dmg = 1) {
		if (invuln) {
			return;
		}

		// Chance to dodge damage:
		float roll = Random.Range(0f, 1f);
		if (roll >= stillHitOdds) {
			// EVASION/DODGE HERE (if you want an animation/sound effect)
			return;
		}

		HP -= dmg;
		if (HP <= 0) {
			die ();
		} else {
			toInvuln (true);
		}
	}

	// Player runs out of health or dies by other means
	public void die() {
		CameraShakeScript CSS = Camera.main.GetComponent<CameraShakeScript> ();
		if(CSS != null){
			CSS.activate(.1f,.1f);
		}

		// TODO - Death screen as transition
		if (!SpawnManager.Instance.freeplayMode) {
			GM.Instance.restartThisSequence ();
		} else {
			GM.ResetProgressThenMainMenu ();
		}

		Destroy (gameObject.transform.parent.gameObject);
	}

	// Scale the colors of both body and pilot sprites
	private void setAllColorScale(float scale) {
		Color newColor = new Color (scale, scale, scale, scale);

		thisSR.color = newColor;
		PlayerTurretSR.color = newColor;
	}
}
