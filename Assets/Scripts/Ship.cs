using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ship : MonoBehaviour {

	protected Vector3 velocity = Vector3.zero;
	protected float accel = .6f;
	protected float friction = .94f;
	protected float rotSpeed, rotAccel = 150f, rotFric = .93f;
	private Vector3 baseRot;

	public GameObject bullet, bulletSpawn;

	private float shootCooldown, shootCooldownAmount = .17f;
	public float maxHP = 100;
	private float hp;

	// Use this for initialization
	void Start () {
		DoStart();
		baseRot = transform.eulerAngles;
		hp = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
		DoUpdate();
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

	public abstract void DoUpdate();
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
