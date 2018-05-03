using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFire : MonoBehaviour {

	// Settings
	public float fireRateMultiplier;
	public float duration;

	// Global variables
	[HideInInspector]
	public float timeLeft;
	private bool rapidFireOn = false;
	private float last_fire = 0;

	// References
	private Player_Fire player_fire;
	private YButtonManager yButtManager;


	// Use this for initialization
	void Start () {
		player_fire = GetComponent<Player_Fire> ();
		yButtManager = GetComponent<YButtonManager> ();

		last_fire = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (!rapidFireOn || PauseManager.paused) {
			return;
		}

		if (Time.time - last_fire > player_fire.fastestFireInterval () / fireRateMultiplier) {
			player_fire.fire ();
			last_fire = Time.time;
		}

		timeLeft -= Time.deltaTime;
		if (timeLeft <= 0) {
			finishRapidFire ();
		}
	}

	// Start or finish the ability
	public void startRapidFire() {
		rapidFireOn = true;
		player_fire.cantFire = true;
		timeLeft = duration;
	}

	private void finishRapidFire() {
		rapidFireOn = false;
		player_fire.cantFire = false;
		yButtManager.finishedAbility ();
	}
}
