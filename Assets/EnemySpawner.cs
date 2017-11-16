using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemy;
	private Stage stage;

	// Use this for initialization
	void Start () {
		stage = GameObject.Find("Stage").GetComponent<Stage>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Random.value < .007)
			SpawnEnemy();
	}

	private void SpawnEnemy()
	{
		Object.Instantiate(enemy, GetSpawnLocation(), enemy.transform.rotation);
	}

	private Vector3 GetSpawnLocation()
	{
		return new Vector3(stage.maxX + 2, Random.Range(stage.minY, stage.maxY), transform.position.y);
	}
}
