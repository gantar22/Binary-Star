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


    void Start(){
        handleWaveOver();
    }


	// Update is called once per frame
	void Update () {
		handleWaveOver();
		//testing purposes
	}


    public void handleWaveOver(){
        SpawnManager.Instance.nextSequence();
    }

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
}
