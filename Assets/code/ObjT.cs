using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjT : MonoBehaviour {
	public int id;
	public enum obj {player_character,player_bullet,enemy_character}
	[SerializeField]
	public obj typ;
}
