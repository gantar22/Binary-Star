using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropType {Health, BombCharge}

public class DropManager : MonoBehaviour {

	// Settings
	public DropTypePrefab[] TypePrefabs;

	// Other variables
	private Dictionary<DropType, DropTypePrefab> DropTypeDict;
	private Dictionary<DropType, int> pityCounters;

	private static DropManager _instance;
	public static DropManager Instance { get { return _instance; } }


	// Initialization
	private void Awake() {
		// Static instance setup
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}

		if (transform.parent == null) {
			DontDestroyOnLoad (this);
		}

		// Initialize dictionaries
		DropTypeDict = new Dictionary<DropType, DropTypePrefab>();
		foreach (DropTypePrefab drop in TypePrefabs) {
			DropTypeDict.Add (drop.Type, drop);
		}

		pityCounters = new Dictionary<DropType, int>();
		foreach (DropType type in System.Enum.GetValues(typeof(DropType))) {
			pityCounters.Add(type, 0);
		}
	}

	// Add incr to each pity counter
	private void incrPityCounters (int incr) {
		foreach (DropType type in System.Enum.GetValues(typeof(DropType))) {
			pityCounters [type] += incr;
		}
	}

	// Spawn a random drop at the given position
	public GameObject SpawnDrop (DropType type, Vector3 pos) {
		GameObject newDrop = GameObject.Instantiate (DropTypeDict[type].prefab);
		newDrop.transform.position = pos;
		return newDrop;
	}

	// Pseudorandom chance, weighted by HP^2, to spawn a drop at the given position
	public void MaybeDrop (int HP, Vector3 pos) {
		incrPityCounters (HP * HP);

		foreach (DropType type in System.Enum.GetValues(typeof(DropType))) {
			int roll = Random.Range (1, DropTypeDict[type].maxDropInterval + 1);
			if (roll <= pityCounters [type]) {
				SpawnDrop (type, pos);
				pityCounters [type] = 0;
				return;
			}
		}
	}

	// Spawn a random drop, weighted by maxDropInterval
	public void SpawnRandDrop (Vector3 pos) {
		// Calculate the total probability
		float remainingProb = 0;
		foreach (DropType type in System.Enum.GetValues(typeof(DropType))) {
			remainingProb += dropWeight(type);
		}

		// Pick a random one
		foreach (DropType type in System.Enum.GetValues(typeof(DropType))) {
			float roll = Random.Range (0f, remainingProb);
			if (roll <= dropWeight(type)) {
				SpawnDrop(type, pos);
				return;
			}
			remainingProb -= dropWeight(type);
		}

		print ("That's not how odds should work...");
		SpawnDrop(DropType.Health, pos);
	}

	// Compute the drop weight of a certain type
	private float dropWeight (DropType type) {
		return 1f / DropTypeDict [type].maxDropInterval;
	}
}


// Struct for editing drop type prefabs and weights in the editor
[System.Serializable]
public struct DropTypePrefab {
	public DropType Type;
	public GameObject prefab;
	public int maxDropInterval;
}