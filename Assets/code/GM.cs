using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		//testing purposes - should manage this better:
		if (inGameScene) {
			handleWaveOver ();
		}
	}


    public void handleWaveOver(){
		if (inGameScene) {
			UpgradesManager.Start_Upgrade_Scene();
		}
	}

	// What to do if you just loaded the game scene
	public void onLoadGame() {
		inGameScene = true;
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
        return Camera.main.WorldToViewportPoint(player.transform.position);
    }

	// Destroy all enemies and make sure the enemy count is set back to 0
	public void resetEnemies() {
		while (enemies.Count != 0) {
			enemies [0].GetComponent<EnemyHP> ().die ();
		}
		enemyCount = 0;
		enemies.Clear ();
	}

	// Restart at the current sequence
	public void restartThisSequence() {
		restartFromSequence (SpawnManager.Instance.sequenceIndex);
	}

	// Restart a specific sequence
	public void restartFromSequence (int seqIndex) {
		SpawnManager.Instance.resetToSequence (seqIndex);
		resetEnemies ();
	}

	// Restart from the first sequence and reset all upgrades
	public void restartGame() {
		restartFromSequence (0);
		UpgradesManager.resetUpgrades ();
	}

	// Reset all progress and restart game from the main menu
	public static void ResetProgressThenPlay() {
		Destroy (GM.Instance.gameObject);
		sceneAfterStart = 2;
		UnityEngine.SceneManagement.SceneManager.LoadScene ("start");
	}
}
