using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum direction {left, right, up, down, still};

public class ScrollManager : MonoBehaviour {

	private static ScrollManager _instance;
	//public static ScrollManager Instance { get { return _instance; } }

	// Settings
	[SerializeField]
	private float accelTime = 2f, initSpeed = 0f;
	[SerializeField]
	private direction initDirection;

	// Other variables
	public static Vector2 scrollVelo { get { return _scrollVelo; } }
	private static Vector2 _scrollVelo;
	private static Vector2 targetVelo;
	private static float timer;


	// Initialize _instance
	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}

		if (transform.parent == null) {
			DontDestroyOnLoad (this);
		}
	}

	void Start() {
		setScrollVelo (initDirection, initSpeed);
	}
		
	// Called once per frame after all updates
	void LateUpdate () {
		if (timer < accelTime) {
			timer += Time.deltaTime;
		} else {
			timer = accelTime;
		}

		_scrollVelo = Vector2.Lerp (Vector2.zero, targetVelo, timer/accelTime);
	}

	// Change the scroll direction and speed
	public static void setScrollVelo(direction dir, float speed) {
		Vector2 veloDir = Vector2.zero;
		if (dir == direction.down) {
			veloDir = Vector2.down;
		} else if (dir == direction.up) {
			veloDir = Vector2.up;
		} else if (dir == direction.left) {
			veloDir = Vector2.left;
		} else if (dir == direction.right) {
			veloDir = Vector2.right;
		}

		targetVelo = veloDir * speed;
		timer = 0f;
	}
}
