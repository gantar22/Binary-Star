using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {


    [SerializeField]
    public GameObject player;

    [HideInInspector]
	public List<GameObject> enemies;

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



}
