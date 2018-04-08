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
	private GameObject rock;
	private List<Turret> Turrets;


	// Initialization
	void Awake () {
		Turrets = new List<Turret> ();
		rb = GetComponent<Rigidbody2D> ();

		// Pick random initial angular velocity
		//minInitAngularVelo *= Mathf.Deg2Rad;
		//maxInitAngularVelo *= Mathf.Deg2Rad;
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
		Turrets.Add (newTurret);
	}

	// Remove a turret from the Turrets list
	public void removeTurret(Turret toRemove) {
		Turrets.Remove (toRemove);
	}

	// When the rock is destroyed, all the turrets die
	public void killTurrets() {
		// Kill all turrets that are still alive
		for (int i = 0; i < Turrets.Count; i++) {
			Turret t = Turrets [i];
			if (t != null) {
				t.gameObject.GetComponent<EnemyHP> ().die ();
			}
		}
	}
}
