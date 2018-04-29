using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb_timer_script : MonoBehaviour {


	bomb_det bd;

	// Use this for initialization
	void Start () {
		bd = GM.Instance.player.GetComponentInChildren<bomb_det>();
	}
	
	// Update is called once per frame
	void Update () {
		//raise children and lower grandchildren
	}
}
