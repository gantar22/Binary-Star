using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMode : MonoBehaviour {

	// Settings
	[Range(0.01f, 1)]
	public float fireInterval = 0.25f;
	public float duration = 3;
	public float randomAngleRange = 80;
	public bool trackingMissiles = true;

	// Global variables
	[HideInInspector]
	public float timeLeft;
	private bool turretModeOn = false;
	private float last_fire = 0;

	// References
	private Player_Fire player_fire;
	private Player_Missile_Fire player_missile_fire;
	private YButtonManager yButtManager;


	// Initialization
	void Start () {
		player_fire = GetComponent<Player_Fire> ();
		player_missile_fire = GetComponent<Player_Missile_Fire> ();
		yButtManager = GetComponent<YButtonManager> ();

		last_fire = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (!turretModeOn || PauseManager.paused) {
			return;
		}

		if (Time.time - last_fire > fireInterval) {
			float randDegreeOffset = Random.Range (randomAngleRange/(-2), randomAngleRange/2);
			player_missile_fire.spawnMissileAtDegreeOffset (randDegreeOffset, trackingMissiles);
			last_fire = Time.time;
		}

		timeLeft -= Time.deltaTime;
		if (timeLeft <= 0) {
			finishTurretMode ();
		}
	}

	// Start or finish the ability
	public void startTurretMode() {
		turretModeOn = true;
		player_fire.cantFire = true;
		timeLeft = duration;
	}

	private void finishTurretMode() {
		turretModeOn = false;
		player_fire.cantFire = false;
		yButtManager.finishedAbility ();
	}
}
