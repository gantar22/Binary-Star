﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderEnemy : MonoBehaviour {

	public EnemyType followerType;
	public int followerTotal = 3;
	public float followRadius = 0.5f;

	// Spawn followers
	void Start () {
		GameObject prefab = EnemyIdentifier.GetEnemyPrefab (followerType);
		GameObject previous = gameObject;
		for (int i = 0; i < followerTotal; i++) {
			GameObject newFollower = Instantiate (prefab);
			newFollower.transform.position = transform.position;
			FollowerEnemy FComponent = newFollower.AddComponent<FollowerEnemy> () as FollowerEnemy;
			FComponent.objToFollow = previous;
			FComponent.followRadius = followRadius;
			previous = newFollower;
		}
	}
}
