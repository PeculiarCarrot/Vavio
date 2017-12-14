using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet {

	public enum BulletType {
		Regular,
		Homing
	}

	
	public float damage = 20;
	public BulletType type;
	private float fric = .99f;
	private GameObject target;

	// Use this for initialization
	public override void DoStart () {
		switch(type)
		{
			case BulletType.Homing:
				target = GetNearestEnemy();
				fric = .95f;
			break;
			default:
			break;
		}
	}

	private GameObject GetNearestEnemy()
	{
		List<GameObject> enemies = stage.GetComponent<Stage>().spawner.GetComponent<EnemySpawner>().GetLiveEnemies();
		float closestDistance = 0;
		GameObject closest = null;
		foreach(GameObject e in enemies)
		{
			if(e != null/* && !e.GetComponent<Enemy>().invul*/)
			{
				float dist = Vector3.Distance(e.transform.position, transform.position);
				if(dist < closestDistance || closest == null)
				{
					closest = e;
					closestDistance = dist;
				}
			}
		}
		return closest;
	}
	
	// Update is called once per frame
	public override void DoUpdate () {
		switch(type)
		{
			case BulletType.Homing:
			if(target == null)
				target = GetNearestEnemy();
			if(target != null)
			{
				float speed = 1f;
				float dx = target.transform.position.x - transform.position.x;
				float dy = target.transform.position.y - transform.position.y;
				float a = Mathf.Atan2(dy, dx) + Mathf.PI / 2;
				 velocity.x += speed * Mathf.Sin(a);
				 velocity.y += -speed * Mathf.Cos(a);
				 velocity *= fric;
				 //velocity.x = Mathf.Min(-1f, velocity.x);
				 transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 90);
			}
			break;
			default:
			break;
		}
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
