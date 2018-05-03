using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct sx {
	[SerializeField]
	public
	string name;
	[SerializeField]
	public float variation;
	[SerializeField]
	public float mid;
	[SerializeField]
	public AudioClip clip;
	[Range(0,1)]
	public float volume;
	[HideInInspector]
	public AudioSource source;

}

public class music_manager : MonoBehaviour {

	public AudioClip calm_music;
	public AudioClip danger_music;
	//[SerializeField]
	//public sx die_sound;
	//[SerializeField]
	//public sx shot_sound;
	[SerializeField]
	public sx[] sxs;
	[HideInInspector]
	public AudioSource a_s;
	private float pitch;
	public GameObject source;
	public float max_music_volume;
	public float max_effects_volume;
	public static float global_pitch = 1;


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
		a_s.volume = max_music_volume;
		a_s.Play();


		for(int i = 0; i < sxs.Length;i++) {
			GameObject g = Instantiate(source, transform);
			sxs[i].source = g.GetComponent<AudioSource>();
		}

	}

	void Update(){
		a_s.volume = max_music_volume;
		a_s.pitch  = global_pitch;
		if(!a_s.isPlaying)	a_s.Play();
	}


	public void shot(){
		play_sound(0);
	}

	public void die(){
		play_sound(1);
	}


	public static void play_by_name(string name){
		for(int i = 0; i < _instance.sxs.Length; i++){
			if (_instance.sxs[i].name == name)
				_instance.play_sound(i);
		}
	}

	public void play_sound(int id){
		sxs[id].source.pitch = ((Random.value - .5f) * sxs[id].variation + sxs[id].mid) * global_pitch;
		sxs[id].source.PlayOneShot(sxs[id].clip, sxs[id].volume * max_effects_volume);
	}

		
}
