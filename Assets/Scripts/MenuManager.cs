using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public void StartGame() {
		// TEMPORARY:
		SceneManager.LoadScene ("EnemyTesting"); 
	}

	public void LoadMainMenu() {
		SceneManager.LoadScene ("MainMenu");
	}

	public void Quit() {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit;
		#endif
	}
}