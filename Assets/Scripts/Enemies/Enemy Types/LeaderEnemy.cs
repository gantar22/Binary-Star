using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderEnemy : MonoBehaviour {

	public EnemyType followerType;
	public int followerTotal;
	public int followRadius;

	// Spawn followers
	void Awake () {
		GameObject prefab = EnemyIdentifier.GetEnemyPrefab (followerType);
		GameObject previous = gameObject;
		for (int i = 0; i < followerTotal; i++) {
			GameObject newFollower = Instantiate (prefab);
			FollowerEnemy FComponent = newFollower.AddComponent<FollowerEnemy> ();
			FComponent.objToFollow = previous;
			FComponent.followRadius = followRadius;
			previous = newFollower;
		}
	}
}
