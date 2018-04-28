using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class nextlevel : MonoBehaviour {

	// Initialization
	void Start () {
		next();
	}

	void next(){
		SceneManager.LoadScene (GM.sceneAfterStart);

		/* if(SceneManager.sceneCountInBuildSettings <= SceneManager.GetActiveScene().buildIndex + 1) {
			//set ui end game active here
			return;
		}
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); */
	}
}
