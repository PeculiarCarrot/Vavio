using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSpawnData {

	private float beginTime;
	private EnemySpawner spawner;
	public List<EnemySpawnData> enemySpawnData;

	public static LevelSpawnData FromJSON(JSONObject o)
	{
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
		beginTime = Time.time;
	}

	public void Update()
	{
		float t = spawner.stage.GetComponent<AudioSource>().time;
		for(int i = enemySpawnData.Count - 1; i >= 0; i--)
		{
			EnemySpawnData esd = enemySpawnData[i];
			if(esd.time + beginTime < t)
			{
				enemySpawnData.RemoveAt(i);
				if(Mathf.Abs(esd.time + beginTime - t) < 1f)
					spawner.SpawnEnemy(esd);
			}
		}
	}
}
