using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet {

	public enum BulletType {
		Regular,
		Homing
	}

	public float damage = 20;
	public BulletType type;
	float speed = 5;
	float spread = 0f;
	private float accel = .03f;
	private float fric = .99f;
	GameObject target;

	public void SetAngleOffset(float angleOffset)
	{
		transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z - 90 + angleOffset + Random.Range(-spread, spread));
		float r = -transform.rotation.eulerAngles.z;
		velocity.x = Mathf.Sin(Mathf.Deg2Rad * (r + 180)) * speed;
		velocity.y = Mathf.Cos(Mathf.Deg2Rad * (r + 180)) * speed;
	}

	// Use this for initialization
	public override void DoStart () {
		target = stage.GetComponent<Stage>().player;
	}
	
	// Update is called once per frame
	public override void DoUpdate () {
		switch(type)
		{
			case BulletType.Homing:
			if(target != null)
			{
				float speed = .04f;
				float dx = target.transform.position.x - transform.position.x;
				float dy = target.transform.position.y - transform.position.y;
				float a = Mathf.Atan2(dy, dx) + Mathf.PI / 2;
				 velocity.x += speed / 4f * Mathf.Sin(a);
				 velocity.y += -speed * Mathf.Cos(a);
				 //velocity.x = Mathf.Min(-1f, velocity.x);
				 transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 90);
			}
			break;
			default:
			break;
		}
		if(transform.position.x < stage.GetComponent<Stage>().minX || transform.position.y < stage.GetComponent<Stage>().minY || transform.position.y > stage.GetComponent<Stage>().maxX)
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
