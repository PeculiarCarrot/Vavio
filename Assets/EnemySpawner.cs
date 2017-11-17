using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemy;
	private Stage stage;
	private float spawnRate = .007f;

	// Use this for initialization
	void Start () {
		stage = GameObject.Find("Stage").GetComponent<Stage>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Random.value < spawnRate)
			SpawnEnemy();
		spawnRate += .000001f;
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
