using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {Basic, Snake, Vulture, Sleeper, Centipede, SyncCircle, Zipper}

public class EnemyIdentifier : MonoBehaviour {

	public static EnemyIdentifier instance;

	public static Dictionary<EnemyType, GameObject> EnemyTypeDict;

	public EnemyTypePrefab[] TypePrefabs;

	void Start() {
		if (instance) {
			Debug.LogError ("Multiple EnemyIdentifiers in scene!");
		} else {
			instance = this;
		}

		// Fill enemy type dictionary with array from editor
		EnemyTypeDict = new Dictionary<EnemyType, GameObject>();
		for (int i = 0; i < TypePrefabs.Length; i++) {
			EnemyTypeDict.Add (TypePrefabs [i].Type, TypePrefabs [i].prefab);
		}
	}

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