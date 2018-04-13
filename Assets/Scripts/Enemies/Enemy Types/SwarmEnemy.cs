using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeightedEnemyPhysics))]
public class SwarmEnemy : MonoBehaviour {

	// Settings/properties
	public GameObject SwarmerPrefab;
	public float innerRadius, outerRadius;
	public int innerCount, outerCount;
	public float maxSpeed, accelMag;

	// Other variables
	private List<WeightedEnemyPhysics> SwarmersWEP;
	private int SwarmerCount;

	// References
	private WeightedEnemyPhysics WEP;


	// Initialization
	void Start () {
		WEP = GetComponent<WeightedEnemyPhysics> ();
		WEP.maxSpeed = maxSpeed;

		SwarmersWEP = new List<WeightedEnemyPhysics>(1 + innerCount + outerCount);
		SpawnSwarm ();
	}
	
	// Called once per frame
	void Update () {
		// Update movement of swarm

		Vector2 direction = new Vector2 ();
		Vector2 pos = new Vector2 (transform.position.x, transform.position.y);

		if (GM.Instance.player == null) return;
		Vector2 targetPos = GM.Instance.player.transform.position;

		// Decide what direction to move in
		direction = targetPos - pos;

		// Normalize the velocity and set to desired speed
		Vector2 newAccel = direction.normalized * accelMag;
		RedirectSwarm (newAccel);

		// Check if the swarm has died
		if (SwarmerCount == 0) {
			Destroy (gameObject);
		} else {
			// Center the hive mind
			center();
		}
	}

	// Create the swarm of enemies
	private void SpawnSwarm() {
		// Spawn center swarmer
		SpawnOneSwarmer (transform.position);

		// Spawn inner and outer swarmers
		SpawnRing (innerCount, innerRadius);
		SpawnRing (outerCount, outerRadius);
	}

	// Spawn a ring of swarmer enemies around the current position
	private void SpawnRing (int enemyCount, float radius) {
		float degrees = 0f;
		float incrBy = 2f * Mathf.PI / enemyCount;
		for (int i = 0; i < enemyCount; i++) {
			float y = Mathf.Cos (degrees);
			float x = Mathf.Sin (degrees);

			Vector2 displacement = (new Vector2(x, y)).normalized * radius;
			Vector3 newPos = transform.position + ((Vector3) displacement);
			SpawnOneSwarmer (newPos);

			degrees += incrBy;
		}
	}

	// Spawn a single swarmer enemy at the given location, attached to this swarm
	private void SpawnOneSwarmer (Vector3 position) {
		GameObject newSwarmer = Instantiate (SwarmerPrefab);
		newSwarmer.transform.position = position;

		SwarmerCount++;

		WeightedEnemyPhysics newWEP = newSwarmer.GetComponent<WeightedEnemyPhysics> ();
		newWEP.maxSpeed = maxSpeed;
		SwarmersWEP.Add (newWEP);
		GM.Instance.Spawn (newSwarmer);
	}

	// Change the acceleration of the entire swarm
	private void RedirectSwarm (Vector2 acceleration) {
		WEP.acceleration = acceleration;

		List<WeightedEnemyPhysics> toRemove = new List<WeightedEnemyPhysics> ();
		foreach (WeightedEnemyPhysics swarmer in SwarmersWEP) {
			if (swarmer == null) {
				toRemove.Add (swarmer);
			} else {
				swarmer.acceleration = acceleration;
			}
		}

		foreach (WeightedEnemyPhysics swarmer in toRemove) {
			SwarmersWEP.Remove (swarmer);
			SwarmerCount--;
		}
	}

	// Set the swarm hive mind position centered based on all the swarmers
	private void center() {
		Vector3 sumPosition = Vector2.zero;
		foreach (WeightedEnemyPhysics swarmer in SwarmersWEP) {
			sumPosition += swarmer.transform.position;
		}
		transform.position = sumPosition / SwarmerCount;
	}
}
