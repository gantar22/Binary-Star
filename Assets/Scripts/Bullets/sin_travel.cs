using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sin_travel : MonoBehaviour {

	// Settings/properties
	[SerializeField]
	private float speed = 1f, frequency = 1f, amplitudeMult = 1f;

	// Other variables
	private Vector3 direction, perp;
	private float period, timeOffset;


	// Initialization
	void Start () {
		direction = transform.right;
		perp = new Vector3 (-direction.y, direction.x, 0);

		period = 1f / frequency;
		timeOffset = period / 2f;
	}

	public void redirect(){
		direction = transform.right;
	}

	// Called once per frame
	void Update () {
		// Decide what direction to move in
		float scale = Mathf.Abs(((Time.time + timeOffset) % (2*period)) - period) / period;
		float radians = Mathf.Lerp(-Mathf.PI/2f, Mathf.PI/2f, scale);

		//Vector3 displacement = perp * Mathf.Sin (radians) * amplitudeMult + direction * Mathf.Cos (radians);
		Vector3 displacement = perp * Mathf.Sin (radians) * amplitudeMult + direction;

		transform.position += displacement.normalized * speed * Time.deltaTime;
		//transform.Translate(speed * Vector3.right * Time.deltaTime,Space.Self);
	}
}
