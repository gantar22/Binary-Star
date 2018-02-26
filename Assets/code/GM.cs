﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {



	public List<GameObject> enemies;

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



	// Update is called once per frame
	void Update () {
		
	}

	public void Spawn(GameObject enemy){
		enemies.Add(enemy);
	}



}