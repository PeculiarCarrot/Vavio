﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship {

	public enum EnemyType {
		Minion,
		Homing
	}

	public EnemyType type;
	public GameObject lightningPrefab;

	public GameObject combineLocation;
	private GameObject partner;
	private bool combining;
	private Quaternion goalRotation, startRotation;
	private Vector3 goalPos, startPos;
	int dir = 0;
	private float combinePercent, combineSpeed;
	GameObject myLightning;

	// Use this for initialization
	public override void DoStart () {
		rotAccel = 100f;
	}

	// Use this for initialization
	public void Awake () {
		accel = .15f + Random.value * .05f;
	}
	
	// Update is called once per frame
	public void Update () {
		DoUpdate();
		if(combining && transform.position.x < stage.GetComponent<Stage>().maxX - 3)
		{	
			if(partner != null)
			{
				if(combinePercent == 0)
				{
					startPos = transform.position;
					startRotation = transform.rotation;
					goalPos = Vector3.Lerp(partner.GetComponent<Enemy>().combineLocation.transform.position, combineLocation.transform.position, 0.5f);
					goalPos.x = transform.position.x;
					goalRotation = Quaternion.Euler(partner.transform.position.y > transform.position.y ? -90 : 90, 180, 180);
					combineSpeed = Mathf.Min(1 / (Vector3.Distance(transform.position, partner.transform.position) * .3f), 1);
					myLightning = Object.Instantiate(lightningPrefab);
				}
				float increment = Time.deltaTime * combineSpeed;
				combinePercent += increment;
				if(combinePercent != increment)
				{
					if(combinePercent > 1)
						combinePercent = 1;
					
					transform.rotation = Quaternion.Slerp(startRotation, goalRotation, combinePercent);
					transform.position = Vector3.Lerp(startPos, goalPos, combinePercent);
					if(combinePercent >= 1)
					{
						baseRot = transform.rotation.eulerAngles;
						combining = false;
						combinePercent = 0;
						if(partner.GetComponent<Enemy>().combining)
						{
							maxHP = hp + partner.GetComponent<Enemy>().hp;
							partner.GetComponent<Enemy>().maxHP = maxHP;
							partner.GetComponent<Enemy>().hp = hp;
						}
						hp = maxHP;
						accel *= .75f;
						Destroy(myLightning);
					}
				}
			}
			else
			{
				combining = false;
			}
		}
		else
			MoveLeft();

		if(myLightning != null && partner != null)
			myLightning.GetComponent<Lightning>().SetPoints(transform.position, partner.transform.position);
		//if(Random.value < .05)
			//NewDirection();
		if(type == EnemyType.Minion)
		{
			if(CanShoot() && Random.value < .03)
				Shoot();
		}
		else if(type == EnemyType.Homing)
		{
			if(CanShoot() && Random.value < .01)
				Shoot();
		}

		velocity.y += (accel / 4f) * dir;
		rotSpeed += rotAccel * dir;
		if(transform.position.x < stage.GetComponent<Stage>().minX - 3)
			Destroy(gameObject);
	}

	public void GetHurt(float damage)
	{
		hp -= damage;
		if(partner != null && !combining)
			partner.GetComponent<Enemy>().hp -= damage;
	}
	
	// Update is called once per frame
	public void DoUpdate () {
		velocity *= friction;
		rotSpeed *= rotFric;
		if(combinePercent == 0)
		{
			transform.position += velocity * Time.deltaTime;
			transform.eulerAngles = baseRot + new Vector3(rotSpeed * Time.deltaTime, 0, 0);
		}
		shootCooldown += Time.deltaTime;
		if(hp <= 0)
			Die();
	}

	public void Combine(GameObject partner)
	{
		combinePercent = 0;
		combining = true;
		this.partner = partner;
	}

	void OnTriggerEnter (Collider col)
    {
    	PlayerBullet bullet = col.gameObject.GetComponent<PlayerBullet>();
        if(bullet != null)
        {
            bullet.Die();
    		if(combinePercent == 0)
            	GetHurt(bullet.GetDamage());
            return;
        }
    	Player player = col.gameObject.GetComponent<Player>();
    	if(player != null)
    	{
    		player.GetHurt(50);
    		GetHurt(50);
    		return;
    	}
    }

	private void NewDirection()
	{
		dir = Random.Range(-1, 2);
	}
}
