using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet {

	public enum BulletType {
		Regular,
		Homing,
		IndependentHoming,
		HurtyBall
	}

	public float damage = 20;
	public BulletType type;
	private float fric = .98f;
	GameObject target;
	private float turnVelocity;
	private float spd;
	private float turnSpd;

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
		spd = Random.value * 2 + 2;
		if(type == BulletType.IndependentHoming)
		{
			fric = .9f;
			spd = .25f + Random.value * .5f;
			SetLifetime(20);
		}
		turnSpd = type == BulletType.Homing ? 1f : .5f + Random.value * .5f;
	}

	private void Track()
	{
		if(target != null)
			{
				float speed = type == BulletType.IndependentHoming? spd : 3f;
				float dx = target.transform.position.x - transform.position.x;
				float dy = target.transform.position.y - transform.position.y;
				float a = Mathf.Atan2(dy, dx) + Mathf.PI / 2;
				 transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.SmoothDampAngle(transform.eulerAngles.z, a * Mathf.Rad2Deg, ref turnVelocity, turnSpd));
				 if(type == BulletType.IndependentHoming)
				 {
				 	velocity.x += speed * Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.z);
				 	velocity.y += -speed * Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.z);
				 }
				 else
				 {
				 	velocity.x = speed * Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.z);
				 	velocity.y = -speed * Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.z);
				 }
				 velocity *= fric;
			}
	}
	
	// Update is called once per frame
	public override void DoUpdate () {
		switch(type)
		{
			case BulletType.Homing:
				Track();
			break;
			case BulletType.IndependentHoming:
				Track();
			break;
			case BulletType.HurtyBall:
			{
				float speed = spd;
				velocity.x = speed * Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.z);
				velocity.y = -speed * Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.z);
			}
			break;
			default:
			break;
		}
		if(transform.position.x < Stage.minX - 5 || transform.position.y < Stage.minY - 5 || transform.position.x > Stage.maxX + 5 || transform.position.y > Stage.maxY + 5)
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
