using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		
	}
	
	public static Vector3 ChangeZ(Vector3 v, float z)
	{
	 return new Vector3(v.x, v.y, z);
	}
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = vectorUtil.ChangeZ(transform.position,-1);
		#endif
	}
}
