using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour {

	// Settings
	[SerializeField]
	private float minInitAngularVelo = 10f, maxInitAngularVelo = 50f;

	// Other variables
	private float angularVelocity = 0f;

	// References
	private Rigidbody2D rb;
	[HideInInspector]
	public Rock rock;
	private List<Turret> Turrets = null;

	public static List<Asteroid> asteroids;


	// Initialization
	void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		rock = GetComponentInChildren<Rock> ();
		if (Turrets == null) {
			Turrets = new List<Turret>();
		}

		if (asteroids == null) {
			asteroids = new List<Asteroid> ();
		}
		asteroids.Add (this);

		// Pick random initial angular velocity
		angularVelocity = RandSign() * Random.Range(minInitAngularVelo, maxInitAngularVelo);
	}

	// Called once per frame
	void Update () {
		rb.rotation += angularVelocity * Time.deltaTime;
	}

	// Change the angular velocity of the RigidBody2D
	public void AddToAngVelo (float angVeloInDegrees) {
		rb.angularVelocity += (angVeloInDegrees * Mathf.Deg2Rad);
	}

	// Returns a random sign (-1 or 1)
	private float RandSign() {
		return Random.value < 0.5 ? -1f : 1f;
	}

	// Add a turret to the Turrets list
	public void addTurret(Turret newTurret) {
		if (Turrets == null) {
			Turrets = new List<Turret>();
		}
		Turrets.Add (newTurret);
	}

	/* // Remove a turret from the Turrets list
	public void removeTurret(Turret toRemove) {
		Turrets.Remove (toRemove);
	} */

	// When the rock is destroyed, all the turrets die
	public void killTurrets() {
		// Kill all turrets that are still alive
		for (int i = 0; i < Turrets.Count; i++) {
			Turret t = Turrets [i];
			if (t != null) {
				t.gameObject.GetComponent<EnemyHP> ().die ();
			}
		}

		asteroids.Remove (this);
		Destroy (gameObject);
	}
}
