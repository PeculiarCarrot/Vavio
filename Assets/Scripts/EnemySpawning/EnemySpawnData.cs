using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData {
	public float time, leave;
	public float x;
	public string type;

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
				case "x":
					esd.x = (float)j.n;
				break;
				case "type":
					esd.type = (string)j.str;
				break;
			}
		}
		return esd;
	}
}
