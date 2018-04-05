using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour {

	// Settings
	[SerializeField]
	private float minInitAngularVelo = 10f, maxInitAngularVelo = 90f;

	// References
	private Rigidbody2D rb;


	// Initialization
	void Awake () {
		rb = GetComponent<Rigidbody2D> ();

		// Pick random initial angular velocity
		minInitAngularVelo *= Mathf.Deg2Rad;
		maxInitAngularVelo *= Mathf.Deg2Rad;
		rb.angularVelocity = RandSign() * Random.Range(minInitAngularVelo, maxInitAngularVelo);
	}
	
	// Called once per frame
	void Update () {
		
	}

	// Change the angular velocity of the RigidBody2D
	public void AddToAngVelo (float angVeloInDegrees) {
		rb.angularVelocity += (angVeloInDegrees * Mathf.Deg2Rad);
	}

	// Returns a random sign (-1 or 1)
	private float RandSign() {
		return Random.value < 0.5 ? -1f : 1f;
	}
}
