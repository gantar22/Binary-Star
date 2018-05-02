using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_range : MonoBehaviour {
	private Vector3 startPos;
	public float range = 5;

	private static float rangeBoostPerLvl = 3.5f;
	private static float extraRange;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(startPos,transform.position) > (range + extraRange)){
			transform.parent.gameObject.SetActive(false);
		}
		
	}

	public static void upgradeRange(int total) {
		extraRange = total * rangeBoostPerLvl;
	}
}
