using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship {

	public enum EnemyType {
		Minion,
		Homing
	}

	public EnemyType type;

	private Quaternion goalRotation, startRotation;
	private Vector3 goalPos, startPos;
	int dir = 0;
	[HideInInspector]
	public float leave;
	private AudioSource song;
	private bool leaving;

	// Use this for initialization
	public override void DoStart () {
		rotAccel = 100f;
		velocity.y = -20f;
		song = stage.GetComponent<AudioSource>();
	}

	// Use this for initialization
	public void Awake () {
		accel = .08f;
		friction = .86f;
	}

	public bool IsInvincible()
	{
		return false;
	}

	// Update is called once per frame
	public void FixedUpdate () {
		DoUpdate();

		//velocity.y += (accel / 4f) * dir;
		rotSpeed += rotAccel * dir;
		if(transform.position.x < stage.GetComponent<Stage>().minX - 3)
			Destroy(gameObject);

		if(song.time >= leave)
			Leave();
		if(hp <= 0)
			Die();
		if(leaving)
			hp -= 1;
	}

	public void Leave()
	{
		leaving = true;
		velocity.y = 10;
		Component[] behaviors;

        behaviors = GetComponents(typeof(BulletBehaviorController));

        foreach (BulletBehaviorController b in behaviors)
            b.enabled = false;
	}

	public new void GetHurt(float damage)
	{
		if(!IsInvincible())
		{
			hp -= damage;
		}
	}
	
	// Update is called once per frame
	public new void DoUpdate () {
		velocity *= friction;
		rotSpeed *= rotFric;
		transform.position += velocity * Time.deltaTime;
		transform.eulerAngles = baseRot + new Vector3(rotSpeed * Time.deltaTime, 0, 0);
		if(hp <= 0)
			Die();
	}

	void OnTriggerEnter (Collider col)
    {
    	PlayerBullet bullet = col.gameObject.GetComponent<PlayerBullet>();
        if(bullet != null)
        {
            bullet.Die();
            GetHurt(bullet.GetDamage());
            return;
        }
    }

	private void NewDirection()
	{
		dir = Random.Range(-1, 2);
	}
}
