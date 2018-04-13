using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	// Settings/properties
	private float randomizedSpawnRadius = 0.8f;
	private float extraScreenSize = 7f;

	public Sequence[] sequences;

	// Other variables
	private Vector2 TopSpawner, BotSpawner, LeftSpawner, RightSpawner, TopFarLeft, TopMidLeft, TopMidRight, TopFarRight;
	private Vector2[] Spawners;

	[HideInInspector]
	public bool idle; // True iff the last sequence has finished and all enemies are dead
	private int sequenceIndex;

	private static SpawnManager _instance;
	public static SpawnManager Instance { get { return _instance; } }


	// Initialization
	private void Awake()
	{
		// Static instance setup
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}

		if (transform.parent == null) {
			DontDestroyOnLoad (this);
		}
			
		// Initialize variables
		idle = true;
		sequenceIndex = 0;
		initSpawnersArray ();
	}

	// If idle, start the next sequence in the list
	public void nextSequence() {
		if (!idle) {
			return;
		}

		if (sequenceIndex >= sequences.Length) {
			// All sequences have finished
			return;
		}
		idle = false;
		StartCoroutine ("playSequence");
	}

	// Play a sequence of waves, pausing at each trigger condition
	IEnumerator playSequence() {
		Sequence sequence = sequences[sequenceIndex];
		if (sequence == null) {
			yield break;
		}

		for (int x = 0; x < sequence.waveTuples.Count; x++) {
			WaveTuple tuple = sequence.waveTuples [x];

			// Wait until condition is met
			if (tuple.condition == Trigger.RemainingEnemies) {
				yield return new WaitForEndOfFrame ();
				yield return new WaitWhile(() => GM.Instance.enemyCount > tuple.threshold);
			} else if (tuple.condition == Trigger.RemainingHealth) {
				yield return new WaitWhile(() => GM.Instance.playerHP > tuple.threshold);
			} else if (tuple.condition == Trigger.Time) {
				yield return new WaitForSeconds (tuple.threshold);
			}
			spawnWave(tuple.wave);
		}

		print("finish them");
		yield return new WaitWhile (() => GM.Instance.enemyCount > 0);
		sequenceIndex++;
		idle = true;
		GM.Instance.handleWaveOver();
		print(sequence);
	}

	// Spawn all the enemies in a given wave
	private void spawnWave(Wave w) {
		initSpawnersArray ();
		List<TypeNumPair>[] waveSpawns = {
			w.TopSpawner,
			w.BotSpawner,
			w.LeftSpawner,
			w.RightSpawner,
			w.TopFarLeft,
			w.TopMidLeft,
			w.TopMidRight,
			w.TopFarRight
		};

		for (int i = 0; i < 8; i++) {
			foreach (TypeNumPair pair in waveSpawns[i]) {
				GameObject prefab = EnemyIdentifier.GetEnemyPrefab (pair.type);
				for (int j = 0; j < pair.numEnemies; j++) {
					GameObject newEnemy = Instantiate (prefab, Spawners[i], Quaternion.identity);
					randomizePosition (newEnemy);
					//GM.Instance.Spawn (newEnemy);
				}
			}
		}
	}

	// Initialize the array of spawner locations
	private void initSpawnersArray () {
		findSpawnPoints ();
		Spawners = new Vector2[8];

		Spawners [0] = TopSpawner;		Spawners [1] = BotSpawner;		Spawners [2] = LeftSpawner; 	Spawners [3] = RightSpawner;
		Spawners [4] = TopFarLeft;		Spawners [5] = TopMidLeft;		Spawners [6] = TopMidRight;		Spawners [7] = TopFarRight;
	}

	// Find the spawn points based on the camera aspects
	private void findSpawnPoints() {
		float aspect = Camera.main.aspect;
		float height = Camera.main.orthographicSize + extraScreenSize;
		float width = Camera.main.orthographicSize * aspect + extraScreenSize;
		float thirdOfCamWidth = Camera.main.orthographicSize * aspect / 3f;

		TopSpawner = new Vector2 (0, height);
		BotSpawner = new Vector2 (0, -height);
		LeftSpawner = new Vector2 (-width, 0);
		RightSpawner = new Vector2 (width, 0);

		TopFarLeft = new Vector2 (-thirdOfCamWidth * 2f, height);
		TopMidLeft = new Vector2 (-thirdOfCamWidth, height);
		TopMidRight = new Vector2 (thirdOfCamWidth, height);
		TopFarRight = new Vector2 (thirdOfCamWidth * 2f, height);
	}

	// Set the new enemy to a random location within the randomizedSpawnRadius of the spawn location
	private void randomizePosition (GameObject newEnemy) {
		Vector3 offset = new Vector3 (Random.Range (-randomizedSpawnRadius, randomizedSpawnRadius), Random.Range (-randomizedSpawnRadius, randomizedSpawnRadius), 0);
		newEnemy.transform.position += offset;
	}
}


// Use in coroutines as follows: yield return new WaitWhile(() => /*bool expression here*/);
public class WaitWhile : CustomYieldInstruction {
	System.Func<bool> m_Predicate;

	public override bool keepWaiting { get { return m_Predicate (); } }

	public WaitWhile(System.Func<bool> predicate) { m_Predicate = predicate; }
}