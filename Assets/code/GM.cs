using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {


    [SerializeField]
    public GameObject player;

    [HideInInspector]
	public List<GameObject> enemies;
	[HideInInspector]
    public int enemyCount = 0;

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
    }


	// Called once per frame
	void Update () {
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().buildIndex > 1) {
			inGameScene = true;
		} else {
			inGameScene = false;
		}
	}


    public void handleWaveOver(){
		if (inGameScene) {
			UpgradesManager.Start_Upgrade_Scene();
		}
	}

	// What to do if you just loaded the game scene
	public void onLoadGame() {
		inGameScene = true; // redundant
		// More TODO here?
	}

	// Enemy list and enemyCount management
	public void Spawn(GameObject enemy){
        enemyCount++;
		enemies.Add(enemy);
	}

	public void Died(GameObject enemy) {
		enemyCount--;
		enemies.Remove (enemy);
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
		if (!SpawnManager.Instance.freeplayMode) {
			EndStateScreens.Instance.InvokeEndScreen (EndStateScreen.You_Died);
		} else {
			EndStateScreens.Instance.InvokeEndScreen (EndStateScreen.Game_Over);
		}
	}


	// ======= MANAGING ENEMIES, SEQUENCES, AND SCENES =======

	// Destroy all enemies and make sure the enemy count is set back to 0
	public void resetEnemies() {
		SpawnManager.Instance.reset ();

		while (enemies.Count != 0) {
			enemies [0].GetComponent<EnemyHP> ().die ();
		}
		enemyCount = 0;
		enemies.Clear ();
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
		Destroy (GM.Instance.gameObject);
		resetGame ();
		sceneAfterStart = 2;
		SceneManager.LoadScene ("start");
	}

	// Reset all progress then return to main menu
	public static void ResetProgressThenMainMenu() {
		Destroy (GM.Instance.gameObject);
		resetGame ();
		sceneAfterStart = 1;
		SceneManager.LoadScene ("start");
	}
}
