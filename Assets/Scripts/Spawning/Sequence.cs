using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Trigger {RemainingEnemies, Time}

[CreateAssetMenu]
public class Sequence : ScriptableObject {
	public List<WaveTuple> waveTuples;
}


// Custom Trigger - Threshold - Wave 3-tuple class
[System.Serializable]
public class WaveTuple {
	public Trigger condition;
	public int threshold;

	public float cameraSizeChange;
	public direction scrollDirection;
	public float scrollSpeed;

	public Wave wave;
}