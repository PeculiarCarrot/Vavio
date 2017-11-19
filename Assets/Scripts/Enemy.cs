using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship {

	public GameObject combineLocation;
	private GameObject partner;
	private bool combining;
	private Quaternion goalRotation, startRotation;
	private Vector3 goalPos, startPos;
	int dir = 0;
	private float rotPercent;

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
		if(combining && transform.position.x < stage.GetComponent<Stage>().maxX - 2)
		{
			float increment = Time.deltaTime * 1f;
			if(rotPercent == 0)
			{
				startPos = transform.position;
				startRotation = transform.rotation;
				goalPos = Vector3.Lerp(partner.GetComponent<Enemy>().combineLocation.transform.position, combineLocation.transform.position, 0.5f);
				goalPos.x = transform.position.x;
				goalRotation = Quaternion.Euler(partner.transform.position.y > transform.position.y ? -90 : 90, 180, 180);
			}
			rotPercent += increment;
			if(rotPercent != increment)
			{
				if(rotPercent > 1)
					rotPercent = 1;
				
				transform.rotation = Quaternion.Slerp(startRotation, goalRotation, rotPercent);
				transform.position = Vector3.Lerp(startPos, goalPos, rotPercent);
				if(rotPercent >= 1)
				{
					baseRot = transform.rotation.eulerAngles;
					combining = false;
					rotPercent = 0;
					maxHP = 1000;
					hp = maxHP;
				}
			}
		}
		else
			MoveLeft();
		//if(Random.value < .05)
			//NewDirection();
		if(CanShoot() && Random.value < .01)
			Shoot();

		velocity.y += (accel / 4f) * dir;
		rotSpeed += rotAccel * dir;
		if(transform.position.x < stage.GetComponent<Stage>().minX - 3)
			Destroy(gameObject);
	}

	public void GetHurt(float damage)
	{
		hp -= damage;
		if(partner != null)
			partner.GetComponent<Enemy>().hp -= damage;
	}
	
	// Update is called once per frame
	public void DoUpdate () {
		velocity *= friction;
		rotSpeed *= rotFric;
		if(rotPercent == 0)
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
		rotPercent = 0;
		combining = true;
		this.partner = partner;
	}

	void OnTriggerEnter (Collider col)
    {
    	PlayerBullet bullet = col.gameObject.GetComponent<PlayerBullet>();
        if(bullet != null)
        {
            bullet.Die();
    		if(!combining)
            	GetHurt(bullet.GetDamage());
            return;
        }
    	Player player = col.gameObject.GetComponent<Player>();
    	if(player != null)
    	{
    		player.GetHurt(50);
    		if(!combining)
    			Die();
    		return;
    	}
    }

	private void NewDirection()
	{
		dir = Random.Range(-1, 2);
	}
}
