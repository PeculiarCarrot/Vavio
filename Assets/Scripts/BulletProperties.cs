using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProperties : MonoBehaviour {

	[HideInInspector]
	public string owner;
	[HideInInspector]
	public float lifetime;
	[HideInInspector]
	public bool destroyOnExitStage, destroyOnHit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		lifetime -= Time.deltaTime;
		if (lifetime <= 0)
			Die();
		if(transform.position.x < Stage.minX - 5 || transform.position.y < Stage.minY - 5 || transform.position.x > Stage.maxX + 5 || transform.position.y > Stage.maxY + 5)
			Destroy(gameObject);
	}

	public void Die()
	{
		Destroy(gameObject);
	}
}
