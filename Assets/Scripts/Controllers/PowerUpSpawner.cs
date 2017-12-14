using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

	public GameObject prefab;
	private float spawnRate = .0001f;

	void Start () {
	}

	void Update () {
		
		if(Random.value < spawnRate)
			SpawnPowerUp();
	}

	private void SpawnPowerUp()
	{
		Vector3 spawn = new Vector3(Stage.maxX + 1, Random.Range(Stage.minY, Stage.maxY), transform.position.y);
		GameObject spawned = Object.Instantiate(prefab, spawn, prefab.transform.rotation);
		spawned.GetComponent<PowerUp>().type = (PowerUp.PowerUpType)Random.Range(1, System.Enum.GetValues(typeof(PowerUp.PowerUpType)).Length);
	}
}
