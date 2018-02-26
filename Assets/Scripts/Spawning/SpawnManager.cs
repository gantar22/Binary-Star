using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour {

	public Sequence[] sequences;
	public Vector2 TopSpawner, BotSpawner, LeftSpawner, RightSpawner;

	[HideInInspector]
	public bool idle; // True iff the last sequence has finished and all enemies are dead

	private int sequenceIndex = 0;


	void Start () {
		nextSequence ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void nextSequence() {
		if (sequenceIndex >= sequences.Length) {
			// End 
			return;
		}

		sequenceIndex++;
	}

	IEnumerator playSequence(Sequence sequence) {
		for (int x = 0; x < sequence.waveTuples.Count; x++) {
			WaveTuple tuple = sequence.waveTuples [x];

			// Wait until condition is met
			if (tuple.condition == Trigger.RemainingEnemies) {
				yield return new WaitWhile(() => 0 > tuple.threshold); // TODO Replace 0 with length of list of enemies, from gameManager
			} else if (tuple.condition == Trigger.RemainingHealth) {
				yield return new WaitWhile(() => 1 > tuple.threshold); // TODO Replace 1 with player health variable
			} else if (tuple.condition == Trigger.Time) {
				yield return new WaitForSeconds (tuple.threshold);
			}

			spawnWave(tuple.wave);
		}

		yield return new WaitWhile (() => 0 > 0); // TODO Replace first 0 with length of list of enemies, from gameManager

		idle = true;
	}

	// Spawn all the enemies in a given wave
	private void spawnWave(Wave wave) {
		foreach (TypeNumPair pair in wave.TopSpawner) {
			GameObject prefab = EnemyIdentifier.GetEnemyPrefab (pair.type);
			for (int i = 0; i < pair.numEnemies; i++) {
				Instantiate (prefab, TopSpawner, Quaternion.identity);
			}
		}
		foreach (TypeNumPair pair in wave.BotSpawner) {
			GameObject prefab = EnemyIdentifier.GetEnemyPrefab (pair.type);
			for (int i = 0; i < pair.numEnemies; i++) {
				Instantiate (prefab, BotSpawner, Quaternion.identity);
			}
		}
		foreach (TypeNumPair pair in wave.LeftSpawner) {
			GameObject prefab = EnemyIdentifier.GetEnemyPrefab (pair.type);
			for (int i = 0; i < pair.numEnemies; i++) {
				Instantiate (prefab, LeftSpawner, Quaternion.identity);
			}
		}
		foreach (TypeNumPair pair in wave.RightSpawner) {
			GameObject prefab = EnemyIdentifier.GetEnemyPrefab (pair.type);
			for (int i = 0; i < pair.numEnemies; i++) {
				Instantiate (prefab, RightSpawner, Quaternion.identity);
			}
		}
	}
}

public class WaitWhile : CustomYieldInstruction {
	Func<bool> m_Predicate;

	public override bool keepWaiting { get { return m_Predicate (); } }

	public WaitWhile(Func<bool> predicate) { m_Predicate = predicate; }
}