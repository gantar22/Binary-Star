using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music_manager : MonoBehaviour {

	public AudioClip calm_music;
	public AudioClip danger_music;
	public AudioClip die_sound;
	public AudioClip shot_sound;
	[HideInInspector]
	public AudioSource a_s;


	// Singleton instance setup
	private static music_manager _instance;
	public static music_manager Instance { get { return _instance; } }

	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}

		if (transform.parent == null) {
			DontDestroyOnLoad (this);
		}
		a_s = GetComponent<AudioSource>();
		a_s.clip = danger_music;
		a_s.Play();
	}


	public void shot(){
		a_s.PlayOneShot(shot_sound,.5f);
	}

	public void die(){
		a_s.PlayOneShot(die_sound,1);
	}

		
}
