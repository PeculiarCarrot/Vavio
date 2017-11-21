using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet {

	public enum BulletType {
		Regular,
		Homing
	}

	public BulletType type;
	public float damage = 20;
	float speed = 5;
	float spread = 10f;
	private float accel = .03f;
	private float fric = .99f;

	// Use this for initialization
	public override void DoStart () {
		transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90 + Random.Range(-spread, spread));
		float r = -transform.rotation.eulerAngles.z;
		velocity.x = Mathf.Sin(Mathf.Deg2Rad * r) * speed;
		velocity.y = Mathf.Cos(Mathf.Deg2Rad * r) * speed;
	}
	
	// Update is called once per frame
	public override void DoUpdate () {
		switch(type)
		{
			case BulletType.Homing:
				GameObject player = stage.GetComponent<Stage>().player;
				if(player.transform.position.x < transform.position.x)
					velocity.x -= accel;
				if(player.transform.position.x > transform.position.x)
					velocity.x += accel;
				if(player.transform.position.y < transform.position.y)
					velocity.y -= accel;
				if(player.transform.position.y > transform.position.y)
					velocity.y += accel;
				velocity.x = Mathf.Min(velocity.x, -4f);
				velocity *= fric;
				 Vector3 newRot = transform.eulerAngles;
				 newRot.z = Mathf.Rad2Deg * Mathf.Atan2(velocity.y, velocity.x) + 90;
				 transform.eulerAngles = newRot;
			break;
			default:
			break;
		}
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
