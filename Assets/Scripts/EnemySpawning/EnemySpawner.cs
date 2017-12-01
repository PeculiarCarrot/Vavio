using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

	void Start()
	{
		stage = GameObject.Find("Stage").GetComponent<Stage>();
		BeginLevel(level);
	}

	public void BeginLevel(int level)
	{
		spawns = AllLevelData.FromJSON(new JSONObject(spawnData.text), level);
		stage.GetComponent<AudioSource>().clip = stage.songs[level];
		stage.GetComponent<AudioSource>().time = 0;
		stage.GetComponent<AudioSource>().Play();
		spawns.Begin(this);
	}

	public void SpawnEnemy(EnemySpawnData data)
	{
		GameObject prefab = GetEnemyFromName(data.type);
		if(prefab != null)
		{
			Vector3 goalPos = Vector3.zero;
			goalPos.x = stage.minX + stage.width * (data.x == float.MaxValue ? .8f : data.x);
			goalPos.y = stage.minY + stage.height * (data.y == float.MaxValue ? .8f : data.y);

			Vector3 pos = goalPos;
			if(data.from == "left")
				pos.x = stage.minX - 1;
			else if(data.from == "right")
				pos.x = stage.maxX + 1;
			else if(data.from == "down")
				pos.y = stage.minY - 1;
			else if(data.from == "up")
				pos.y = stage.maxY + 1;

			GameObject e = Instantiate(prefab, pos, Quaternion.Euler(new Vector3(prefab.transform.eulerAngles.x, prefab.transform.eulerAngles.x, prefab.transform.eulerAngles.z + data.rotation)));
			e.GetComponent<Enemy>().leave = data.leave;
			e.GetComponent<Enemy>().reachGoalTime = data.reachGoalTime;
			e.GetComponent<Enemy>().invul = data.invul;
			e.GetComponent<Enemy>().SetGoalPos(goalPos);
			liveEnemies.Add(e);
		}
		else
		{
			Debug.LogError("No enemy type exists: " + data.type);
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
