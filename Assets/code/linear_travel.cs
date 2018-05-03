using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linear_travel : MonoBehaviour {


	[SerializeField]
	private float _speed = 1;
	public float getBaseSpeed { get { return _speed; } }

	[SerializeField]
	private float _exponent = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(_speed * Vector3.right * Time.deltaTime,Space.Self);
		_speed += Time.deltaTime * _exponent * _speed;
	}

	public void setSpeed(float x){
		_speed += x;
	}
}
