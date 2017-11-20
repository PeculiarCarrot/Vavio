using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemy;
	private Stage stage;
	private float spawnRate = .0007f;
	private float timeUntilCombine = 1f;

	// Use this for initialization
	void Start () {
		stage = GameObject.Find("Stage").GetComponent<Stage>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Random.value < spawnRate)
			SpawnEnemy();
		spawnRate += .000001f;
		timeUntilCombine -= Time.deltaTime;
		if(timeUntilCombine <= 0)
			Combine();
	}

	private void Combine()
	{
		GameObject ship1 = Object.Instantiate(enemy, GetSpawnLocation(), enemy.transform.rotation);
		GameObject ship2 = Object.Instantiate(enemy, GetSpawnLocation(), enemy.transform.rotation);
		ship1.GetComponent<Enemy>().Combine(ship2);
		ship2.GetComponent<Enemy>().Combine(ship1);
		ship1.GetComponent<Enemy>().accel = .15f;
		ship2.GetComponent<Enemy>().accel = .15f;
		timeUntilCombine = 3;
	}

	private void SpawnEnemy()
	{
		Object.Instantiate(enemy, GetSpawnLocation(), enemy.transform.rotation);
	}

	private Vector3 GetSpawnLocation()
	{
		return new Vector3(stage.maxX + 1, Random.Range(stage.minY, stage.maxY), transform.position.y);
	}
}
