using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Trigger {RemainingEnemies, RemainingHealth, Time}

[CreateAssetMenu]
public class Sequence : ScriptableObject {
	public List<WaveTuple> waveTuples;
}


// Custom Trigger - Threshold - Wave 3-tuple class
[System.Serializable]
public class WaveTuple {
	public Trigger condition;
	public int threshold;
	public Wave wave;

	public WaveTuple() {
	}

	public WaveTuple(Trigger frst, int scnd, Wave thrd) {
		this.condition = frst;
		this.threshold = scnd;
		this.wave = thrd;
	}
}