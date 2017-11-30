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
	private float fric = .98f;
	GameObject target;
	private float turnVelocity;

	public void SetAngleOffset(float angleOffset)
	{
		/*transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z - 90 + angleOffset + Random.Range(-spread, spread));
		float r = -transform.rotation.eulerAngles.z;
		velocity.x = Mathf.Sin(Mathf.Deg2Rad * (r + 180)) * speed;
		velocity.y = Mathf.Cos(Mathf.Deg2Rad * (r + 180)) * speed;*/
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
				float speed = 3f;
				float dx = target.transform.position.x - transform.position.x;
				float dy = target.transform.position.y - transform.position.y;
				float a = Mathf.Atan2(dy, dx) + Mathf.PI / 2;
				 transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.SmoothDampAngle(transform.eulerAngles.z, a * Mathf.Rad2Deg, ref turnVelocity, 1f));
				 velocity.x = speed * Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.z);
				 velocity.y = -speed * Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.z);
				 velocity *= fric;
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

	public new void OnDie()
	{
		Destroy(gameObject);
	}
}
