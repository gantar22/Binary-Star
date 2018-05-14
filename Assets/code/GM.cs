using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {


    [SerializeField]
    public GameObject player;

    [HideInInspector]
    public Vector2 player_pos {get {return decoy != null ? decoy.transform.position : player.transform.position;}}
    [HideInInspector]
    public GameObject decoy;

    [HideInInspector]
	public List<GameObject> enemies;

	public static int sceneAfterStart = 1; // 1 for MainMenu, 2 for game

	public static bool inGameScene = false;

    private static GM _instance;

    public static GM Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        DontDestroyOnLoad(this);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


	// Called once per frame
	void Update () {
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex > 1) {
			inGameScene = true;
		} else {
			inGameScene = false;
		}
	}


    public void handleSequenceOver(){
		if (inGameScene) {
			UpgradesManager.Start_Upgrade_Scene();
		}
	}

	// What to do if you just loaded the game scene
	public static void onLoadGame() {
		inGameScene = true; // redundant
		// More TODO here?
		GM.Instance.resetEnemies ();
		SpawnManager.Instance.idle = true;
		SpawnManager.Instance.nextSequence();
	}

    public Vector2 player_screen_loc(){
		if (player) {
			return Camera.main.WorldToViewportPoint (player.transform.position);
		} else {
			return new Vector2 (0.5f, 0.5f);
		}
    }
		

	// Called when the player dies
	public void PlayerDied() {
		GM.Instance.resetEnemies ();

		if (!SpawnManager.Instance.freeplayMode) {
			EndStateScreens.Instance.InvokeEndScreen (EndStateScreen.You_Died);
		} else {
			EndStateScreens.Instance.InvokeEndScreen (EndStateScreen.Game_Over);
		}
	}

	// Respawn the player
	public void RespawnPlayer() {
		player.SetActive (true);
		PlayerHP.resetHPAndCooldowns();
		player.transform.position = new Vector3 (0, 0, 0);
	}


	// ======= MANAGING ENEMIES, SEQUENCES, AND SCENES =======

	// Enemy list management
	public void Spawn(GameObject enemy){
		enemies.Add(enemy);
	}

	public void Died(GameObject enemy) {
		enemies.Remove (enemy);
	}

	// Stop all SpawnManager coroutines, and then destroy all enemies
	public void resetEnemies() {
		SpawnManager.Instance.reset ();

		while (enemies.Count != 0) {
			EnemyHP EHP;
			if (enemies [0] != null && (EHP = enemies [0].GetComponent<EnemyHP> ())) {
				enemies.RemoveAt (0);
				EHP.die ();
			} else {
				enemies.RemoveAt (0);
			}
		}
		enemies.Clear ();
	}

	// Start the YouWin endstatescreen that leads to freeplay
	public void YouWin() {
		resetEnemies ();
		resetToSequence (SpawnManager.Instance.sequences.Length);
		EndStateScreens.Instance.InvokeEndScreen (EndStateScreen.You_Win);
	}

	// Restart a specific sequence
	public void resetToSequence (int seqIndex) {
		SpawnManager.Instance.resetToSequence (seqIndex);
		GM.Instance.resetEnemies ();
	}

	// Restart at the current sequence
	public void resetToSameSequence() {
		resetToSequence (SpawnManager.Instance.sequenceIndex);
	}

	// Set the sequence index to 0 and reset all upgrades
	public static void resetGame() {
		if (GM.Instance) {
			GM.Instance.resetToSequence (0);
		}
		UpgradesManager.resetUpgrades ();
	}

	// Reset all progress and go straight into game
	public static void ResetProgressThenPlay() {
		resetGame ();
		Destroy (GM.Instance.gameObject);
		sceneAfterStart = 2;
		SceneManager.LoadScene ("start");
	}

	// Reset all progress then return to main menu
	public static void ResetProgressThenMainMenu() {
		resetGame ();
		Destroy (GM.Instance.gameObject);
		sceneAfterStart = 1;
		SceneManager.LoadScene ("start");
	}
}
