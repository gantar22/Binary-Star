using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keep_in_game : MonoBehaviour {


	
	// Update is called once per frame
	void Update () {
		if(GM.Instance.player_screen_loc().x > 2 || GM.Instance.player_screen_loc().x < -1
			|| GM.Instance.player_screen_loc().y > 2 || GM.Instance.player_screen_loc().y < -1){
			Invoke("correct",.5f);
		}
	}


	void correct(){
		if(GM.Instance.player_screen_loc().x > 2 || GM.Instance.player_screen_loc().x < -1
			|| GM.Instance.player_screen_loc().y > 2 || GM.Instance.player_screen_loc().y < -1){
			transform.position = Vector3.zero;
		}
	}
}
