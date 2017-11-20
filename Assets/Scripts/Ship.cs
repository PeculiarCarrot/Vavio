using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ship : MonoBehaviour {

	protected Vector3 velocity = Vector3.zero;
	public float accel = .9f;
	protected float friction = .9f;
	protected float rotSpeed, rotAccel = 170f, rotFric = .93f;
	protected Vector3 baseRot;
	public static GameObject stage;

	public GameObject bullet, bulletSpawn;

	protected float shootCooldown, shootCooldownAmount = .1f;
	public float maxHP = 100;
	protected float hp;

	// Use this for initialization
	void Start () {
		if(stage == null)
			stage = GameObject.Find("Stage");
		DoStart();
		baseRot = transform.eulerAngles;
		hp = maxHP;
	}
	
	// Update is called once per frame
	public void DoUpdate () {
		velocity *= friction;
		transform.position += velocity * Time.deltaTime;
		rotSpeed *= rotFric;
		transform.eulerAngles = baseRot + new Vector3(rotSpeed * Time.deltaTime, 0, 0);
		shootCooldown += Time.deltaTime;
		if(hp <= 0)
			Die();
	}


	public void GetHurt(float damage)
	{
		hp -= damage;
	}

	public void Die()
	{
		Destroy(gameObject);
	}

	public bool CanShoot()
	{
		return shootCooldown > shootCooldownAmount;
	}

	public abstract void DoStart();

	public void MoveUp()
	{
		velocity.y += accel;
		rotSpeed += rotAccel;
	}

	public void MoveLeft()
	{
		velocity.x -= accel;
	}

	public void MoveRight()
	{
		velocity.x += accel;
	}

	public void MoveDown()
	{
		velocity.y -= accel;
		rotSpeed -= rotAccel;
	}

	protected void Shoot()
	{
		Object.Instantiate(bullet, bulletSpawn.transform.position, bullet.transform.rotation);
		shootCooldown = 0;
	}
}
