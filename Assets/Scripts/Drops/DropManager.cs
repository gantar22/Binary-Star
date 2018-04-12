using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropType {Health, BombCharge}

public class DropManager : MonoBehaviour {

	// Settings
	public int maxDropInterval = 40;
	public DropTypePrefab[] TypePrefabs;

	// Other variables
	private int pityCounter;
	private int totalDropWeight;

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

		// Sum up the total drop weight
		totalDropWeight = 0;
		for (int i = 0; i < TypePrefabs.Length; i++) {
			totalDropWeight += TypePrefabs [i].DropWeight;
		}

		// Initialize variables
		pityCounter = 0;
	}

	// Spawn a random drop at the given position
	public GameObject SpawnDrop (Vector3 pos) {
		GameObject newDrop = GameObject.Instantiate (RandomDropPrefab ());
		newDrop.transform.position = pos;
		return newDrop;
	}

	// Return the prefab for a random drop
	public GameObject RandomDropPrefab() {
		int remainingProb = totalDropWeight;
		foreach (DropTypePrefab DTP in TypePrefabs) {
			int roll = Random.Range (1, remainingProb + 1);
			if (roll <= DTP.DropWeight) {
				return DTP.prefab;
			}
			remainingProb -= DTP.DropWeight;
		}
		print ("That's not how odds should work...");
		return TypePrefabs [TypePrefabs.Length - 1].prefab;
	}

	// Pseudorandom chance, weighted by HP^2, to spawn a drop at the given position
	public bool MaybeDrop (int HP, Vector3 pos) {
		pityCounter += HP * HP;
		int roll = Random.Range (1, maxDropInterval + 1);
		if (roll <= pityCounter) {
			SpawnDrop (pos);
			pityCounter = 0;
			return true;
		}
		return false;
	}
}


// Struct for editing drop type prefabs and weights in the editor
[System.Serializable]
public struct DropTypePrefab {
	public DropType Type;
	public GameObject prefab;
	public int DropWeight;
}