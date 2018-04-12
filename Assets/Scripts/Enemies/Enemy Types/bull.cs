
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class bull : MonoBehaviour {

	// Settings/properties:
	[SerializeField]
	private int fakeDives = 2;

	[SerializeField]
	private float maxSpeed = 3.5f, accelMag = 5f;

	[SerializeField]
	private float diveSpeedMult = 1.7f;

	[SerializeField]
	private float frequency = 2f;

	[SerializeField]
	private float minCircleTime = 3f, maxCircleTime = 5f;
	[SerializeField]
	private float minDiveTime = 0.8f, maxDiveTime = 1.2f;

	[SerializeField]
	private float speed;

	// Other variables
	private bool circling;
	private float delay;
	private int divesSoFar;
	private float handedness = 1;

	// Object references
	private WeightedEnemyPhysics WEP;

	private Vector2 direction;
	private bool stunned;



	// Initialize
	void Start () {
		WEP = GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = maxSpeed;

		circling = true;
		delay = minCircleTime;
		divesSoFar = 0;
		direction = new Vector2 ();
	}

	// Called every frame
	void Update () {
		
		controlDelay ();
		if (circling) {

			reposition();	

			Vector2 perp = new Vector2 (-direction.y, direction.x);
			direction = perp;

			if(Physics2D.Raycast(transform.position,direction, speed * Time.deltaTime * handedness,1 << 9)) {
				handedness *= -1;
				print(":::");
			}
		}

		// Normalize the velocity and set to desired speed
		//WEP.acceleration = direction.normalized * accelMag;
		Debug.DrawRay(transform.position, direction * speed * Time.deltaTime * handedness, Color.red, 5,false);
		transform.position += (Vector3)direction.normalized * speed * Time.deltaTime * handedness;
		//transform.eulerAngles = new Vector3(0,0,Mathf.LerpAngle(transform.eulerAngles.z, Vector2.Angle(Vector2.zero,direction), 10 * Time.deltaTime));
	}

	private void controlDelay() {
		if (divesSoFar > fakeDives) {
			return;
		}

		if (delay > 0) {
			delay -= Time.deltaTime;
		} else if (circling) {
			// Start to dive

			// Decide what direction to move in
			reposition();


			divesSoFar++;
			circling = false;
			frequency = frequency * diveSpeedMult;
			//WEP.maxSpeed = maxSpeed * diveSpeedMult;
			speed *= diveSpeedMult;
			handedness = 1;
			delay = Random.Range (minDiveTime, maxDiveTime);
		} else {
			// Start circling again	
			circling = true;
			frequency = frequency / diveSpeedMult;
			//WEP.maxSpeed = maxSpeed;
			speed /= diveSpeedMult;
			delay = Random.Range (minCircleTime, maxCircleTime);
		}
	}


	void reposition(){

		if(GM.Instance.player == null) return;
		Vector2 playerPos = GM.Instance.player.transform.position;
		transform.eulerAngles = new Vector3(0,0,(Mathf.Atan2(GM.Instance.player.transform.position.x,GM.Instance.player.transform.position.y) * Mathf.Rad2Deg));

		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);

		direction = playerPos - pos;
	}


	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.layer == 9){
			stunned = true;
		}
	}

}
