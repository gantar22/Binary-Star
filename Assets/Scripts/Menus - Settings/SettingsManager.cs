using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SettingsManager : MonoBehaviour {

	public static GameSettings gameSettings;

	// Singleton instance setup
	private static SettingsManager _instance;
	public static SettingsManager Instance { get { return _instance; } }


	void Awake () {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			_instance = this;
		}

		if (transform.parent == null) {
			DontDestroyOnLoad (this);
		}

		// Load the settings file. If there is none, initialize the settings and save them
		if (!LoadSettings ()) {
			gameSettings = new GameSettings ();

			gameSettings.fullscreen = Screen.fullScreen;
			gameSettings.cameraShake = true;

			gameSettings.masterVolume = 1.0f;
			gameSettings.localMusicVolume = 1.0f;
			gameSettings.localSFXVolume = 1.0f;
		}

		UpdateFinalVolumes ();
	}

	void UpdateFinalVolumes() {
		gameSettings.musicVolume = gameSettings.masterVolume * gameSettings.localMusicVolume;
		gameSettings.SFXVolume = gameSettings.masterVolume * gameSettings.localSFXVolume;

		SaveSettings ();

		if (music_manager.Instance) {
			music_manager.Instance.max_music_volume = gameSettings.musicVolume;
			music_manager.Instance.max_effects_volume = gameSettings.SFXVolume;
			if (music_manager.Instance.a_s) {
				music_manager.Instance.a_s.volume = gameSettings.musicVolume;
			}
		}
	}

	public static void changeMasterVolume (float newVolume) {
		gameSettings.masterVolume = newVolume;
		Instance.UpdateFinalVolumes ();
	}

	public static void changeMusicVolume (float newVolume) {
		gameSettings.localMusicVolume = newVolume;
		Instance.UpdateFinalVolumes ();
	}

	public static void changeSFXVolume (float newVolume) {
		gameSettings.localSFXVolume = newVolume;
		Instance.UpdateFinalVolumes ();
	}

	public static void changeFullscreen (bool newBool) {
		Screen.fullScreen = newBool;
		gameSettings.fullscreen = newBool;
		SaveSettings ();
	}

	public static void changeCameraShake (bool newBool) {
		gameSettings.cameraShake = newBool;
		SaveSettings ();
	}

	// Persistence (Saving and loading)
	private static void SaveSettings() {
		string path = Application.persistentDataPath + "/Settings.dat";

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file;

		if (!File.Exists (path)) {
			file = File.Create (path);
		} else {
			file = File.Open (path, FileMode.Open);
		}

		bf.Serialize (file, gameSettings);
		file.Close ();
	}

	// Load level progress
	private static bool LoadSettings() {
		string path = Application.persistentDataPath + "/Settings.dat";

		if (File.Exists (path)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (path, FileMode.Open);

			gameSettings = (GameSettings)bf.Deserialize (file);
			file.Close ();

			return true;
		}

		return false;
	}
}
