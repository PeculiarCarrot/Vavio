﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData {
	public float time, leave = 999f, reachGoalTime = .2f, hp = 300;
	public float x = float.MaxValue, y = float.MaxValue, z = 0, rotation, scale = 1;
	public string type, from = "up", movement = null, pattern = "General/none", model = "circle", material = "red";
	public bool invul, canCollide = true, introMovement = true, boss, givesCharge = true, canCollideWithBullets = true, absoluteZ = false, growsOnHit = true;

	public static EnemySpawnData FromJSON(JSONObject o)
	{
		EnemySpawnData esd = new EnemySpawnData();
		for(int i = 0; i < o.list.Count; i++)
		{
			string key = (string)o.keys[i];
			JSONObject j = (JSONObject)o.list[i];
			switch(key)
			{
				case "time":
					esd.time = (float)j.n;
					break;
				case "hp":
					esd.hp = (float)j.n;
					break;
				case "leave":
					esd.leave = (float)j.n;
				break;
				case "rotation":
					esd.rotation = (float)j.n;
				break;
				case "x":
					esd.x = (float)j.n;
					break;
				case "y":
					esd.y = (float)j.n;
					break;
				case "z":
					esd.z = (float)j.n;
					break;
				case "reachGoalTime":
					esd.reachGoalTime = (float)j.n;
					break;
				case "scale":
					esd.scale = (float)j.n;
					break;
				case "type":
					esd.type = (string)j.str;
					break;
				case "movement":
					esd.movement = (string)j.str;
					break;
				case "pattern":
					esd.pattern = (string)j.str;
					break;
				case "model":
					esd.model = (string)j.str;
					break;
				case "material":
					esd.material = (string)j.str;
					break;
				case "from":
					esd.from = (string)j.str;
					break;
				case "invul":
					esd.invul = (bool)j.b;
					break;
				case "boss":
					esd.boss = (bool)j.b;
					break;
				case "canCollide":
					esd.canCollide = (bool)j.b;
					break;
				case "givesCharge":
					esd.givesCharge = (bool)j.b;
					break;
				case "absoluteZ":
					esd.absoluteZ = (bool)j.b;
					break;
				case "introMovement":
					esd.introMovement = (bool)j.b;
					break;
				case "canCollideWithBullets":
					esd.canCollideWithBullets = (bool)j.b;
					break;
				case "growsOnHit":
					esd.growsOnHit = (bool)j.b;
					break;
			}
		}
		return esd;
	}
}
