using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet {

	public float damage = 20;

	// Use this for initialization
	public override void DoStart () {
		velocity.x = -10f;
	}
	
	// Update is called once per frame
	public override void DoUpdate () {
		if(transform.position.x < stage.GetComponent<Stage>().minX)
			Destroy(gameObject);
	}

	public float GetDamage()
	{
		return damage;
	}

	public void Die()
	{
		Destroy(gameObject);
	}
}
