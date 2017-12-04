using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBase {

	public enum EnemyType {
		Minion,
		Homing,
		Laser,
		LaserWarning
	}

	public EnemyType type;

	int dir = 0;
	[HideInInspector]
	public float leave;
	private AudioSource song;
	private bool leaving;
	[HideInInspector]
	private Vector3 goalPos;
	[HideInInspector]
	public float reachGoalTime = .2f;
	private Vector3 goalVelocity = Vector3.zero;
	private Vector3 startPos;
	public bool invul;
	private EnemyBehavior behavior;

	public void SetBehavior(EnemyBehavior b)
	{
		behavior = b;
		b.SetEnemy(this);
	}

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
		if(type == EnemyType.LaserWarning || type == EnemyType.Laser)
			transform.position = goalPos;
	}

	// Use this for initialization
	public void Awake () {
		accel = .08f;
		friction = .86f;
		leave = 9999;
	}

	public bool IsInvincible()
	{
		return invul || (!reachedGoal && !leaving) || type == EnemyType.LaserWarning || type == EnemyType.Laser;
	}

	// Update is called once per frame
	public void FixedUpdate () {
		if(behavior != null)
			behavior.Update();
		DoUpdate();

		if(!reachedGoal)
		{
		
			if(type == EnemyType.LaserWarning || type == EnemyType.Laser)
				transform.position = goalPos;	transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, reachGoalTime);
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
		if(type == EnemyType.LaserWarning)
		{
			Vector3 scale = transform.localScale;
			scale.x += 2f * Stage.deltaTime;
			transform.localScale = scale;
		}
		if(type == EnemyType.Laser)
		{
			Vector3 scale = transform.localScale;
			scale.x *= .8f;
			transform.localScale = scale;
			if(scale.x < .2f)
				Die();
		}
	}

	public void Leave()
	{
		if(type == EnemyType.LaserWarning || type == EnemyType.Laser)
			Die();
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
			Flash();
		}
	}
	
	// Update is called once per frame
	public new void DoUpdate () {
		velocity *= friction;
		rotSpeed *= rotFric;
		transform.position += velocity * (1/60f);
		transform.eulerAngles = baseRot + new Vector3(rotSpeed * Time.deltaTime, 0, 0);
		if(hp <= 0)
			Die();
	}

	void OnTriggerEnter (Collider col)
    {
    	PlayerBullet bullet = col.gameObject.GetComponent<PlayerBullet>();
        if(bullet != null && !(type == EnemyType.LaserWarning || type == EnemyType.Laser))
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
