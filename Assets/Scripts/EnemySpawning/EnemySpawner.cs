using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour {

	[System.Serializable]
	public struct NamedEnemy
	{
		public string name;
		public GameObject prefab;
	}

	public TextAsset spawnData;

	public NamedEnemy[] enemy;

	private int level = 0;
	private LevelSpawnData spawns;

	public bool spawningEnabled = true;
	public Stage stage;
	private List<GameObject> liveEnemies = new List<GameObject>();

	void Start () {
		stage = GameObject.Find("Stage").GetComponent<Stage>();
		BeginLevel(level);
	}

	public void BeginLevel(int level)
	{
		spawns = AllLevelData.FromJSON(new JSONObject(spawnData.text), level);
		spawns.Begin(this);
	}

	public void SpawnEnemy(string type, float x, float leave)
	{
		GameObject prefab = GetEnemyFromName(type);
		if(prefab != null)
		{
			Vector3 pos = Vector3.zero;
			pos.x = stage.minX + stage.width * x;
			pos.y = stage.maxY + 1;
			GameObject e = Instantiate(prefab, pos, prefab.transform.rotation);
			e.GetComponent<Enemy>().leave = leave;
			liveEnemies.Add(e);
		}
		else
		{
			Debug.LogError("No enemy type exists: " + type);
		}
	}

	public GameObject GetEnemyFromName(string name)
	{
		foreach(NamedEnemy e in enemy)
			if(e.name == name)
				return e.prefab;
		return null;
	}

	public List<GameObject> GetLiveEnemies()
	{
		return liveEnemies;
	}
	
	void Update () {
		if(spawningEnabled)
		{
			spawns.Update();
		}
	}
}
