using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Trigger {RemainingEnemies, Time}

[CreateAssetMenu]
public class Sequence : ScriptableObject {
	public List<WaveTuple> waveTuples;

	public Sequence () {
		waveTuples = new List<WaveTuple> ();
	}
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


	// Constructor with default camera size change and scroll settings
	public WaveTuple (Trigger cond, int thresh, Wave w) {
		condition = cond;
		threshold = thresh;
		wave = w;

		cameraSizeChange = 0f;
		scrollDirection = direction.down;
		scrollSpeed = 5f;
	}

	// Full constructor
	public WaveTuple (Trigger cond, int thresh, float camSizeChange, direction scrollDir, float speed, Wave w) {
		condition = cond;
		threshold = thresh;

		cameraSizeChange = camSizeChange;
		scrollDirection = scrollDir;
		scrollSpeed = speed;

		wave = w;
	}
}