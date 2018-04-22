using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjT : MonoBehaviour {
	public int id;
	public enum obj {player_character, player_bullet, enemy_character, enemy_bullet, player_explosion, enemy_explosion}
	[SerializeField]
	public obj typ;
}
