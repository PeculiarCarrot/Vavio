using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship {

	public enum EnemyType {
		Minion,
		Homing
	}

	public EnemyType type;

	int dir = 0;
	[HideInInspector]
	public float leave;
	private AudioSource song;
	private bool leaving;
	[HideInInspector]
	private Vector3 goalPos;
	private bool reachedGoal = true;
	[HideInInspector]
	public float reachGoalTime = .2f;
	private Vector3 goalVelocity = Vector3.zero;
	private Vector3 startPos;
	public bool invul;

	// Use this for initialization
	public override void DoStart () {
		rotAccel = 100f;
		Stage s = stage.GetComponent<Stage>();
		song = stage.GetComponent<AudioSource>();
		startPos = transform.position;
	}

	public void SetGoalPos(Vector3 pos)
	{
		goalPos = pos;
		reachedGoal = false;
	}

	// Use this for initialization
	public void Awake () {
		accel = .08f;
		friction = .86f;
		leave = 9999;
	}

	public bool IsInvincible()
	{
		return invul || (!reachedGoal && !leaving);
	}

	// Update is called once per frame
	public void FixedUpdate () {
		DoUpdate();

		if(!reachedGoal)
		{
			transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, reachGoalTime);
			if(Vector3.Distance(transform.position, goalPos) < .2f)
			{
				reachedGoal = true;
				velocity *= 2;
				if(leaving)
					Die();
			}
		}
		//velocity.y += (accel / 4f) * dir;
		rotSpeed += rotAccel * dir;
		if(Stage.time >= leave && !leaving)
			Leave();
		if(hp <= 0)
			Die();
	}

	public void Leave()
	{
		leaving = true;
		reachedGoal = false;
		goalPos = startPos;
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
