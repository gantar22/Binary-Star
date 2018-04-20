using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

[CreateAssetMenu]
public class Wave : ScriptableObject {
	public List<TypeNumPair> TopSpawner = new List<TypeNumPair>();
	public List<TypeNumPair> BotSpawner = new List<TypeNumPair>();
	public List<TypeNumPair> LeftSpawner = new List<TypeNumPair>();
	public List<TypeNumPair> RightSpawner = new List<TypeNumPair>();

	public List<TypeNumPair> TopFarLeft = new List<TypeNumPair>();
	public List<TypeNumPair> TopMidLeft = new List<TypeNumPair>();
	public List<TypeNumPair> TopMidRight = new List<TypeNumPair>();
	public List<TypeNumPair> TopFarRight = new List<TypeNumPair>();
}


// Custom EnemyType - int pair class
[System.Serializable]
public class TypeNumPair {
	public EnemyType type;
	public int numEnemies;

	public TypeNumPair() {
	}

	public TypeNumPair(EnemyType first, int second) {
		this.type = first;
		this.numEnemies = second;
	}
}
