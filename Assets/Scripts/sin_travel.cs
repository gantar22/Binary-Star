using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sin_travel : MonoBehaviour {

	// Settings/properties
	[SerializeField]
	private float speed, frequency, amplitudeMult;

	// Other variables
	private Vector3 direction, perp;


	// Initialization
	void Start () {
		direction = transform.right;
		print ("Direction: " + direction);
		perp = new Vector3 (-direction.y, direction.x, 0);
	}

	// Called once per frame
	void Update () {
		// Decide what direction to move in
		float period = 1f / frequency;
		float scale = Mathf.Abs((Time.time % (2*period)) - period) / period;
		float radians = Mathf.Lerp(-Mathf.PI/2f, Mathf.PI/2f, scale);

		//Vector3 displacement = perp * Mathf.Sin (radians) * amplitudeMult + direction * Mathf.Cos (radians);
		Vector3 displacement = perp * Mathf.Sin (radians) * amplitudeMult + direction;

		print ((Vector3) displacement.normalized * speed);
		transform.position += displacement.normalized * speed * Time.deltaTime;
		//transform.Translate(speed * Vector3.right * Time.deltaTime,Space.Self);
	}
}
