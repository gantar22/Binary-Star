using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	// Settings/properties
	private float randomizedSpawnRadius = 0.8f;
	private float extraScreenSize = 7f;
	private float delayBeforeSeq = 3f;
	private int baseFreeplayWaveNum = 10;
	private int tierInterval = 5;

	public Sequence[] sequences;
	public Wave[] freeplayWavesTier1;
	public Wave[] freeplayWavesTier2;
	public Wave[] freeplayWavesTier3;
	public Wave[] freeplayWavesTier4;
	public Wave[] freeplayWavesTier5;

	private Wave[] freeplayWaves;

	// Other variables
	private Vector2 TopSpawner, BotSpawner, LeftSpawner, RightSpawner, TopFarLeft, TopMidLeft, TopMidRight, TopFarRight;
	private Vector2[] Spawners;

	[HideInInspector]
	public bool idle, freeplayMode = false;
	[HideInInspector]
	public int sequenceIndex, freeplayMult = 1, freeplayCounter = 0;

	private Sequence freeplaySeq;


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
		resetToSequence(0);
	}

	// Stop all coroutines and set to idle
	public void reset() {
		StopAllCoroutines ();
		idle = true;
	}

	// Reset the spawnManager to a certain index
	public void resetToSequence (int newSeqIndex) {
		reset ();
		sequenceIndex = newSeqIndex;
	}


	// If idle, start the next sequence in the list
	public void nextSequence() {
		if (!idle) {
			return;
		}

		if (freeplayMode) {
			freeplaySeq = newFreeplaySeq ();
		} else if (sequenceIndex >= sequences.Length) {
			freeplayMode = true;
			nextSequence ();
			return;
		}

		idle = false;
		StartCoroutine ("playSequence");
	}

	// Play a sequence of waves, pausing at each trigger condition
	IEnumerator playSequence() {
		Sequence sequence;
		if (!freeplayMode) {
			sequence = sequences [sequenceIndex];
		} else {
			sequence = freeplaySeq;
		}

		if (sequence == null) {
			yield break;
		}

		// Fixed delay before every sequence
		yield return new WaitForSeconds (delayBeforeSeq);

		for (int x = 0; x < sequence.waveTuples.Count; x++) {
			WaveTuple tuple = sequence.waveTuples [x];

			// Wait until condition is met
			if (tuple.condition == Trigger.RemainingEnemies) {
				yield return new WaitForEndOfFrame ();
				yield return new WaitUntil (() => GM.Instance.enemies.Count <= tuple.threshold);
			} else if (tuple.condition == Trigger.Time) {
				yield return new WaitForSeconds (tuple.threshold);
			}
			spawnWave(tuple);
			yield return new WaitForEndOfFrame ();
		}

		print("finish them");
		yield return new WaitUntil (() => GM.Instance.enemies.Count <= 0);

		if (!freeplayMode) {
			sequenceIndex++;
		} else {
			freeplayCounter++;
			if (freeplayCounter % 3 == 0) {
				freeplayMult++;
			}
		}

		idle = true;
		GM.Instance.handleSequenceOver();
	}

	// Construct a new sequence out of the freeplay waves, based on the multiplier
	private Sequence newFreeplaySeq () {
		setFreeplayWaves ();

		Sequence newSeq = new Sequence ();
		int waveNum = baseFreeplayWaveNum + freeplayMult;

		for (int i = 0; i < waveNum; i++) {
			int randIndex = Random.Range (0, freeplayWaves.Length);
			int randThresh = Random.Range (1, 5) + freeplayMult;
			WaveTuple newWaveTuple = new WaveTuple (Trigger.RemainingEnemies, randThresh, freeplayWaves [randIndex]);
			newSeq.waveTuples.Add (newWaveTuple);

			for (int j = 1; j < freeplayMult; j++) {
				randIndex = Random.Range (0, freeplayWaves.Length);
				newWaveTuple = new WaveTuple (Trigger.Time, 1, freeplayWaves [randIndex]);
				newSeq.waveTuples.Add (newWaveTuple);
			}
		}

		return newSeq;
	}

	// Set the freeplayWaves array depending on the freeplayCounter
	private void setFreeplayWaves() {
		if (freeplayWaves == null) {
			freeplayWaves = freeplayWavesTier1;
		} else if (freeplayCounter == 1 * tierInterval) {
			addFPWaves (freeplayWavesTier2);
		} else if (freeplayCounter == 2 * tierInterval) {
			addFPWaves (freeplayWavesTier3);
		} else if (freeplayCounter == 3 * tierInterval) {
			addFPWaves (freeplayWavesTier4);
		} else if (freeplayCounter == 4 * tierInterval) {
			addFPWaves (freeplayWavesTier5);
		}
	}

	// Adds newWaves to freeplayWave
	private void addFPWaves (Wave[] toAdd) {
		Wave[] newWaves = new Wave[freeplayWaves.Length + toAdd.Length];
		freeplayWaves.CopyTo(newWaves, 0);
		toAdd.CopyTo (newWaves, freeplayWaves.Length);
		freeplayWaves = newWaves;
	}


	// Spawn all the enemies in a given wave
	private void spawnWave(WaveTuple tuple) {
		Wave w = tuple.wave;

		// Adjust the camera size and scroll speed
		if (!freeplayMode) {
			Camera.main.GetComponent<CameraSmoothZoom> ().addToCameraSize (tuple.cameraSizeChange);
			ScrollManager.setScrollVelo (tuple.scrollDirection, tuple.scrollSpeed);
		} else {
			ScrollManager.setScrollVelo (direction.down, 5);
		}

		// Re-initialize the spawners array and then spawn the wave
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
				for (int j = 0; j < pair.numEnemies; j++) { //move to coroutine
					GameObject newEnemy = Instantiate (prefab, Spawners[i], Quaternion.identity);
					randomizePosition (newEnemy);
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
