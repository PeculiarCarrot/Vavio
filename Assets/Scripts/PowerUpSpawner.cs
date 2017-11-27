using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

	public GameObject prefab;
	private Stage stage;
	private float spawnRate = .01f;

	// Use this for initialization
	void Start () {
		stage = GameObject.Find("Stage").GetComponent<Stage>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Random.value < spawnRate)
			SpawnPowerUp();
	}

	private void SpawnPowerUp()
	{
		Vector3 spawn = new Vector3(stage.maxX + 1, Random.Range(stage.minY, stage.maxY), transform.position.y);
		GameObject spawned = Object.Instantiate(prefab, spawn, prefab.transform.rotation);
		spawned.GetComponent<PowerUp>().type = (PowerUp.PowerUpType)Random.Range(1, System.Enum.GetValues(typeof(PowerUp.PowerUpType)).Length);
	}
}
