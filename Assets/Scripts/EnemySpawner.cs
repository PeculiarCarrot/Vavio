using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject[] enemy;
	public bool enabled = true;
	private Stage stage;
	private float spawnRate = .04f;
	private float timeUntilCombine = 1f;
	private List<GameObject> liveEnemies = new List<GameObject>();
	private Vector3 nullVector = new Vector3(12345.123f, 12345.123f);

	// Use this for initialization
	void Start () {
		stage = GameObject.Find("Stage").GetComponent<Stage>();
	}

	public List<GameObject> GetLiveEnemies()
	{
		return liveEnemies;
	}
	
	// Update is called once per frame
	void Update () {
		if(enabled)
		{
			if(Random.value < spawnRate)
				SpawnEnemy();
			spawnRate += .000001f;
			timeUntilCombine -= Time.deltaTime;
			//if(timeUntilCombine <= 0)
			//	Combine();
		}
	}

	private GameObject GetNewEnemy()
	{
		return enemy[Random.Range(0, enemy.Length)];
	}

	private void Combine()
	{
		Vector3 spawn = TryGetSpawnLocation();
		if(spawn == nullVector)
			return;
		Vector3 spawn2 = TryGetSpawnLocation(spawn);
		if(spawn2 == nullVector)
			return;

		GameObject o1 = GetNewEnemy();
		GameObject o2 = GetNewEnemy();

		GameObject ship1 = Object.Instantiate(o1, spawn, o1.transform.rotation);
		GameObject ship2 = Object.Instantiate(o2, spawn2, o2.transform.rotation);

		ship1.GetComponent<Enemy>().Combine(ship2);
		ship2.GetComponent<Enemy>().Combine(ship1);
		ship2.GetComponent<Enemy>().accel = ship1.GetComponent<Enemy>().accel;
		liveEnemies.Add(ship1);
		liveEnemies.Add(ship2);
		timeUntilCombine = 6;
	}

	private void SpawnEnemy()
	{
		Vector3 spawn = TryGetSpawnLocation();
		if(spawn == nullVector)
			return;
		GameObject enemy = GetNewEnemy();
		liveEnemies.Add(Object.Instantiate(enemy, spawn, enemy.transform.rotation));
	}

	private Vector3 TryGetSpawnLocation()
	{
		return TryGetSpawnLocation(nullVector);
	}

	private Vector3 TryGetSpawnLocation(Vector3 avoid)
	{
		for(int i = 0; i < 25; i++)
		{
			Vector3 v = GetSpawnLocation(avoid);
			if(v != nullVector)
				return v;
		}
		return nullVector;
	}

	private Vector3 GetSpawnLocation(Vector3 avoid)
	{
		Vector3 v = new Vector3(Random.Range(stage.minX, stage.maxX), stage.maxY - 3, transform.position.y);
		if(avoid != nullVector && Vector3.Distance(v, avoid) < 2f)
			return nullVector;

		foreach(GameObject o in liveEnemies)
		{
			if(o != null && Vector3.Distance(v, o.transform.position) < 2f)
				return nullVector;
		}
		return v;
	}

}
