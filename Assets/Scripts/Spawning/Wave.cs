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


/*

// Custom editor for waves
#if UNITY_EDITOR
[CustomEditor(typeof(Wave))]
public class WaveEditor : Editor {
	private ReorderableList list;

	private void OnEnable() {
		list = new ReorderableList (serializedObject, serializedObject.FindProperty ("Spawners"), true, true, true, true);
	}

	public override void OnInspectorGUI() {

		list = new ReorderableList (serializedObject, serializedObject.FindProperty ("Spawners"), true, true, true, true);

		serializedObject.Update ();
		if (list != null) {
			var wave = target as Wave;
			wave.SpawnCondition = (Trigger) EditorGUILayout.EnumPopup("Spawn Condition", wave.SpawnCondition);
			wave.EnemyThreshold = list.count;
			wave.EnemyThreshold = EditorGUILayout.IntField ("Enemy Threshold", wave.EnemyThreshold);
		}
		list.DoLayoutList ();
		serializedObject.ApplyModifiedProperties ();

		//		// Custom editor setup
		//		//serializedObject.Update ();
		//		var wave = target as Wave;
		//		GUI.changed = false;
		//
		//		// Variable fields
		//		wave.SpawnCondition = (Trigger) EditorGUILayout.EnumPopup("Spawn Condition", wave.SpawnCondition);
		//
		//		if (wave.SpawnCondition == Trigger.RemainingEnemies) {
		//			wave.EnemyThreshold = EditorGUILayout.IntField ("Enemy Threshold", wave.EnemyThreshold);
		//		} else if (wave.SpawnCondition == Trigger.RemainingHealth) {
		//			wave.HealthThreshold = EditorGUILayout.IntField ("Health Threshold", wave.HealthThreshold);
		//		} else if (wave.SpawnCondition == Trigger.Time) {
		//			wave.TimeBeforeTrigger = EditorGUILayout.IntField ("Time Before Trigger", wave.TimeBeforeTrigger);
		//		}
		//
		//		// Managing reorderable list
		//
		//
		//
		//		// Set dirty if changed
		//		if (GUI.changed) {
		//			EditorUtility.SetDirty (wave);
		//		}
	}
}
#endif

*/