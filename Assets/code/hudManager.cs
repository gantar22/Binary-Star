using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hudManager : MonoBehaviour {

	private static hudManager	_instance;
	public static hudManager Instance {get {return _instance;}}

	public Text description;
	public GameObject health;
	public GameObject bomb;

	void Awake(){
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
