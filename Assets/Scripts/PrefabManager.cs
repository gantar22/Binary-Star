using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

	// Prefabs go here
	public Explosion explosion;
	public OffScreenWarning OffScreenWarningPrefab;
	public AOE_Damage aoe_damage;

	// Singleton instance setup
	private static PrefabManager _instance;
	public static PrefabManager Instance { get { return _instance; } }

	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}

		if (transform.parent == null) {
			DontDestroyOnLoad (this);
		}
	}
}
