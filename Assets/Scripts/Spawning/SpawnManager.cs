using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour {

	public Sequence[] sequences;
	public Vector2 TopSpawner, BotSpawner, LeftSpawner, RightSpawner, TopFarLeft, TopMidLeft, TopMidRight, TopFarRight;
	private Vector2[] Spawners;

	[HideInInspector]
	public bool idle; // True iff the last sequence has finished and all enemies are dead
	private int sequenceIndex;

	private static SpawnManager _instance;
	public static SpawnManager Instance { get { return _instance; } }


	// Static instance setup
	private void Awake()
	{
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
		DontDestroyOnLoad(this);

		// Initialize
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
		for (int x = 0; x < sequence.waveTuples.Count; x++) {
			WaveTuple tuple = sequence.waveTuples [x];
			// Wait until condition is met
			if (tuple.condition == Trigger.RemainingEnemies) {
				yield return new WaitWhile(() => GM.Instance.enemyCount > tuple.threshold);
			} else if (tuple.condition == Trigger.RemainingHealth) {
				yield return new WaitWhile(() => 1 > tuple.threshold); // TODO Replace 1 with player health variable
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
					GM.Instance.Spawn (newEnemy);
				}
			}
		}

//		foreach (TypeNumPair pair in wave.TopSpawner) {
//			GameObject prefab = EnemyIdentifier.GetEnemyPrefab (pair.type);
//			for (int i = 0; i < pair.numEnemies; i++) {
//				GameObject newEnemy = Instantiate (prefab, TopSpawner, Quaternion.identity);
//				GM.Instance.Spawn (newEnemy);
//			}
//		}
//		foreach (TypeNumPair pair in wave.BotSpawner) {
//			GameObject prefab = EnemyIdentifier.GetEnemyPrefab (pair.type);
//			for (int i = 0; i < pair.numEnemies; i++) {
//				GameObject newEnemy = Instantiate (prefab, BotSpawner, Quaternion.identity);
//				GM.Instance.Spawn (newEnemy);
//			}
//		}
	}

	private void initSpawnersArray () {
		Spawners = new Vector2[8];

		Spawners [0] = TopSpawner;		Spawners [1] = BotSpawner;		Spawners [2] = LeftSpawner; 	Spawners [3] = RightSpawner;
		Spawners [4] = TopFarLeft;		Spawners [5] = TopMidLeft;		Spawners [6] = TopMidRight;		Spawners [7] = TopFarRight;
	}
}


// Use in coroutines as follows: yield return new WaitWhile(() => /*bool expression here*/);
public class WaitWhile : CustomYieldInstruction {
	Func<bool> m_Predicate;

	public override bool keepWaiting { get { return m_Predicate (); } }

	public WaitWhile(Func<bool> predicate) { m_Predicate = predicate; }
}