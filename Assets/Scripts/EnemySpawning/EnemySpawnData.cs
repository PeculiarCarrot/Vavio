using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData {
	public float time, leave, reachGoalTime = .2f;
	public float x = float.MaxValue, y = float.MaxValue, rotation;
	public string type, from = "up";

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
				case "from":
					esd.from = (string)j.str;
				break;
			}
		}
		return esd;
	}
}
