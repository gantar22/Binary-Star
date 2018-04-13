using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderEnemy : MonoBehaviour {

	public EnemyType followerType;
	public int followerTotal = 3;
	public float followRadius = 0.5f;
	public Sprite followerSprite;

	// Spawn followers
	void Start () {
		GameObject prefab = EnemyIdentifier.GetEnemyPrefab (followerType);
		GameObject previous = gameObject;
		for (int i = 0; i < followerTotal; i++) {
			GameObject newFollower = Instantiate (prefab);
			GM.Instance.Spawn (newFollower);
			newFollower.transform.position = transform.position;

			LeaderEnemy LE = newFollower.GetComponent<LeaderEnemy> ();
			if (LE != null && LE.enabled) {
				LE.enabled = false;
			}

			FollowerEnemy FComponent = newFollower.AddComponent<FollowerEnemy> () as FollowerEnemy;
			FComponent.objToFollow = previous;
			FComponent.followRadius = followRadius;
			FComponent.followerSprite = followerSprite;
			FComponent.leaderSprite = newFollower.GetComponentInChildren<SpriteRenderer>().sprite;

			previous = newFollower;
		}
	}
}
