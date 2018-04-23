using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {Basic, Vulture, Sleeper, Centipede, Shielded, Swarm, Asteroid, TurretAsteroid, Fireworks, MissileShooter, SinShooter}

public class EnemyIdentifier : MonoBehaviour {

	public static Dictionary<EnemyType, GameObject> EnemyTypeDict;
	public EnemyTypePrefab[] TypePrefabs;

	private static EnemyIdentifier _instance;
	public static EnemyIdentifier Instance { get { return _instance; } }


	// Initialization
	private void Awake() {
		// Static instance setup
		if (_instance != null && _instance != this) {
			Debug.LogError ("Multiple EnemyIdentifiers in scene!");
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}

		if (transform.parent == null) {
			DontDestroyOnLoad (this);
		}

		// Fill enemy type dictionary with array from editor
		EnemyTypeDict = new Dictionary<EnemyType, GameObject>();
		for (int i = 0; i < TypePrefabs.Length; i++) {
			EnemyTypeDict.Add (TypePrefabs [i].Type, TypePrefabs [i].prefab);
		}
	}

	// Returns the enemy prefab for a given EnemyType
	public static GameObject GetEnemyPrefab(EnemyType type) {
		GameObject prefab;
		if (EnemyTypeDict.TryGetValue (type, out prefab)) {
			return prefab;
		} else {
			Debug.LogError ("No prefab found for EnemyType: " + type);
			return null;
		}
	}
}


// Hack for editing the dictionary in the editor
[System.Serializable]
public struct EnemyTypePrefab {
	public EnemyType Type;
	public GameObject prefab;
}