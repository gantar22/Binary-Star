using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class no_collide_with_player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	/*void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.GetComponent<PlayerHP>()){
			Bounds bounds = GetComponent<Collider2D>().bounds;
			float bottom = bounds.min.y;
			float top    = bounds.max.y;
			float left   = bounds.min.x;
			float right  = bounds.max.x;

			Vector3 player_pos = other.transform.position;
			Transform pt = GM.Instance.player.transform;
			float dif_bottom = player_pos.y - bottom;
			float dif_top    = top - player_pos.y;
			float dif_left   = player_pos.x - left;
			float dif_right  = right - player_pos.x;

			float min_dis = Mathf.Min(new float[] {dif_bottom,dif_top,dif_left,dif_right});
			if(min_dis == dif_bottom){
				pt.position = new Vector3(player_pos.x,bottom,player_pos.z);
			}
			if(min_dis == dif_top){
				pt.position = new Vector3(player_pos.x,top,player_pos.z);
			}
			if(min_dis == dif_left){
				pt.position = new Vector3(left,player_pos.y,player_pos.z);
			}
			if(min_dis == dif_right){
				pt.position = new Vector3(right,player_pos.y,player_pos.z);
			}
		}
	}*/
}
