using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squeeze : MonoBehaviour {

	public float max_stretch;
	public float max_speed;
	private Vector3 prev_position;
	private Vector3 scale;
	private float prev_delta;
	private float speed;
	public float extra = .1f;
	public float delta_delta;

	void Awake(){
		scale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if(PauseManager.paused || Time.deltaTime == 0) return;
		float delta = Mathf.Pow((prev_position - transform.position).magnitude,1.5f);
		speed = delta / Time.deltaTime;
		//if(Mathf.Abs(prev_delta - delta) > .01f) overshoot = extra;
		//else overshoot -= Mathf.Abs(prev_delta - delta);
		transform.localScale = scale + new Vector3(1,-1,0) * (speed / max_speed) * max_stretch;
		prev_position = transform.position;
		prev_delta = delta;

		}
}
