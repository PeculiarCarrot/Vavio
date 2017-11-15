using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet {

	public float damage = 20;

	// Use this for initialization
	public override void DoStart () {
		velocity.x = 40f;
	}
	
	// Update is called once per frame
	public override void DoUpdate () {
		if(transform.position.x > 30)
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
