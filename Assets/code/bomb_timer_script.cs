using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb_timer_script : MonoBehaviour {


	bomb_det bd;
	public float range;
	public GameObject fill;
	Transform fill_child;
	Vector3 start_pos;
	Canvas canvas;

	// Use this for initialization
	void Start () {
		bd = GM.Instance.player.GetComponentInChildren<bomb_det>();
		start_pos = fill.transform.position;
		fill_child = fill.transform.GetChild(0);
		canvas = GetComponentInParent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
		float pos = Mathf.Clamp((1 - (bd.timer / bd.cooldown)) * range,0,range);
		//raise children and lower grandchildren

		fill_child.parent = canvas.transform;
		fill.transform.position = pos * Vector3.up + start_pos;
		fill_child.parent = fill.transform;
	}
}
