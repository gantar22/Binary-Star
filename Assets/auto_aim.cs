using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auto_aim : MonoBehaviour {

	public float window_angle = 15;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		
		int hand = 1;
		int h = 1;
		RaycastHit2D r = new RaycastHit2D();
		float z = transform.eulerAngles.z;
		float dis = 25;
		Vector2 dir;

		float degToRad = (2 * Mathf.PI) / 360;

	
		for(float i = 0; Mathf.Abs(i) < window_angle && !r; i += (h * hand))
		{
			dir = new Vector2(Mathf.Cos((z + i) * degToRad),Mathf.Sin((z + i) * degToRad));

			r = Physics2D.Raycast(transform.position,dir,dis,1 << 8);

			if (r){
				transform.GetChild(0).eulerAngles = new Vector3(0,0,z + i);
				return;
			} else{
				r = new RaycastHit2D();
			}

			i = -i;
			dir = new Vector2(Mathf.Cos((z + i) * degToRad),Mathf.Sin((z + i) * degToRad));

			r = Physics2D.Raycast(transform.position,dir,dis,1 << 8);	
			if (r) {
				transform.GetChild(0).eulerAngles = new Vector3(0,0,z + i);
				return;
			} else{
				r = new RaycastHit2D();
			}
			i = -i;
		}
		transform.GetChild(0).localEulerAngles = Vector3.zero;
	}
}
