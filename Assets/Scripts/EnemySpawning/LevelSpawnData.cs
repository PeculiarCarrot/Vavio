using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSpawnData {

	private EnemySpawner spawner;
	public List<EnemySpawnData> enemySpawnData;

	public static LevelSpawnData FromJSON(JSONObject o)
	{
		o = o.list[0];
		LevelSpawnData lsd = new LevelSpawnData();
		lsd.enemySpawnData = new List<EnemySpawnData>();
		for(int i = 0; i < o.list.Count; i++)
		{
			JSONObject j = (JSONObject)o.list[i];
			lsd.enemySpawnData.Add(EnemySpawnData.FromJSON(j));
		}
		return lsd;
	}

	public void Begin(EnemySpawner spawner)
	{
		this.spawner = spawner;
	}

	public void Update()
	{
		float t = (float)Stage.time;
		for(int i = enemySpawnData.Count - 1; i >= 0; i--)
		{
			EnemySpawnData esd = enemySpawnData[i];
			if(esd.time < t)
			{
				enemySpawnData.RemoveAt(i);
				if(Mathf.Abs(esd.time - t) < 1f || esd.type == "boss-until-then")
					spawner.SpawnEnemy(esd);
			}
		}
	}
}
