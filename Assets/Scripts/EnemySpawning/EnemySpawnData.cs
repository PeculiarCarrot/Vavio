using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData {
	public float time, leave, reachGoalTime = .2f;
	public float x = float.MaxValue, y = float.MaxValue, rotation, scale = 1;
	public string type, from = "up", movement = null, pattern = "General/basic", model = "circle", material = "red";
	public bool invul;

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
				case "reachGoalTime":
					esd.reachGoalTime = (float)j.n;
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
			}
		}
		return esd;
	}
}
