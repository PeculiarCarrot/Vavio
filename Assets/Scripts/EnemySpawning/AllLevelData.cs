using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllLevelData {
	public LevelSpawnData[] levelSpawnData;

	public static AllLevelData FromJSON(JSONObject o)
    {
    	AllLevelData sl = new AllLevelData();
		sl.levelSpawnData = new LevelSpawnData[o.list.Count];
		for(int i = 0; i < o.list.Count; i++)
		{
			string key = (string)o.keys[i];
			JSONObject j = (JSONObject)o.list[i];
			sl.levelSpawnData[int.Parse(key.Remove(0, 1))] = LevelSpawnData.FromJSON(j);
		}
		return sl;
	}

	public static LevelSpawnData FromJSON(JSONObject o, int level)
    {
		for(int i = 0; i < o.list.Count; i++)
		{
			if(int.Parse((string)o.keys[i].Remove(0, 1)) == level)
			{
				JSONObject j = (JSONObject)o.list[i];
				return LevelSpawnData.FromJSON(j);
			}
		}
		return null;
	}
}
