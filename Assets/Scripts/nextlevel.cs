using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class nextlevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		next();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void next(){
		if(SceneManager.sceneCountInBuildSettings <= SceneManager.GetActiveScene().buildIndex + 1) {
			//set ui end game active here
			return;
		}
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
