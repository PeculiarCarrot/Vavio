using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ShooterBase {
	
	//The image for the boss health bar
	private static Texture healthBarTexture;
	//The death effect prefab for bosses
	private static GameObject bossDeathEffect;
	//The death effect prefab for regular enemies
	private static GameObject enemyDeathEffect;


	//Whether or not the enemy is doing the automatic enter/exit stage animation
	private bool leaving;
	//The goal position for the enter/exit animation
	[HideInInspector]
	private Vector3 goalPos;
	//How fast we reach the goal position
	[HideInInspector]
	public float reachGoalTime = .2f;
	//Where we started before the enter animation so we can return to it
	private Vector3 startPos;
	//Whether we've finished our enter/exit stage animation
	protected bool reachedGoal = true;

	//Whether we are marked to always be invulnerable
	public bool invul;
	//Whether we can collide with the player (if we're invincible, we won't collide with bullets either)
	public bool canCollide;
	//Whether we have the enter/exit stage animation
	public bool introMovement;
	//Whether or not we're a boss
	public bool boss;
	//Our material (used to set the color of death animations)
	public Material mat;
	//How long we don't collide with the player after we spawn
	private float noCollisionTimer = 1f;
	
	public Vector3 velocity = Vector3.zero;
	//Our acceleration
	public float accel = .9f;
	public float friction = .9f;
	protected Stage stage;

	//Our starting and maximum HP
	public float maxHP = 100;
	public float hp;
	//Whether we are flashing from taking damage
	private bool flashing;


	void Start () {
		stage = Stage.stage;
		startPos = transform.position;
		hp = maxHP;
	}

	IEnumerator CollideFlash(Renderer mainRenderer)
	{
		Material m = mainRenderer.material;
		Color c = mainRenderer.material.color;
		mainRenderer.material = null;
		mainRenderer.material.color = ChangeColorBrightness(c, .6f);
		mainRenderer.material.shader = Shader.Find("Unlit/Color");
		flashing = true;		
		yield return new WaitForSeconds(0.03f);
		mainRenderer.material = m;
		mainRenderer.material.color = c;
		flashing = false;
	}

	private Color ChangeColorBrightness(Color color, float correctionFactor)
	{
		float red = (float)color.r;
		float green = (float)color.g;
		float blue = (float)color.b;

		if (correctionFactor < 0)
		{
			correctionFactor = 1 + correctionFactor;
			red *= correctionFactor;
			green *= correctionFactor;
			blue *= correctionFactor;
		}
		else
		{
			red = (1 - red) * correctionFactor + red;
			green = (1 - green) * correctionFactor + green;
			blue = (1 - blue) * correctionFactor + blue;
		}

		return new Color(red, green, blue, color.a);
	}

	//Do a damage flash
	public void Flash()
	{
		if(!flashing)
			foreach(Renderer r in GetComponentsInChildren<Renderer>())
				StartCoroutine(CollideFlash(r));
	}

	public void SetGoalPos(Vector3 pos)
	{
		if(!introMovement)
		{
			transform.position = pos;
			reachedGoal = true;
			return;
		}
		goalPos = pos;
		reachedGoal = false;
	}

	// Use this for initialization
	public void Awake () {
		accel = .08f;
		friction = .86f;
		leave = 9999;
		if(healthBarTexture == null)
			healthBarTexture = Resources.Load<Texture>("Materials/bossHealthBar");
		if(bossDeathEffect == null)
			bossDeathEffect = Resources.Load<GameObject>("Prefabs/Effects/bossDeathEffect");
		if(enemyDeathEffect == null)
			enemyDeathEffect = Resources.Load<GameObject>("Prefabs/Effects/enemyDeathEffect");
	}

	public bool IsInvincible()
	{
		return invul || (!reachedGoal && !leaving);
	}

	//If the enemy was killed by a player, create a death effect. Otherwise, just destroy this.
	public void Die(bool player)
	{
		if(player && mat.name != "transparent")
		{
			GameObject prefab = boss ? bossDeathEffect : enemyDeathEffect;
			GameObject e = Instantiate(prefab, transform.position, prefab.transform.rotation);
			e.GetComponent<LineRenderer>().material = mat;
		}

		Stage.RemoveEnemy(gameObject);
		Destroy(gameObject);
	}

	public void Die()
	{
		Die(false);
	}
	
	public void FixedUpdate () {

		velocity *= friction;
		transform.position += velocity * (1/60f);
		if(hp <= 0)
			Die(true);

		noCollisionTimer -= Time.deltaTime;

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

		if(Stage.time >= leave && !leaving)
			Leave();
	}

	//Start the leave animation (or just die if the animation is disabled)
	public void Leave()
	{
		if(!introMovement)
			Die();
		leaving = true;
		reachedGoal = false;
		goalPos = startPos;
	}

	public void GetHurt(float damage)
	{
		if(!IsInvincible())
		{
			hp -= damage;
			Flash();
		}
	}

	//Used to draw boss health bar
	public void OnGUI()
	{
		if(boss)
		{
			float frac = hp / (float)maxHP;
			frac*= .5f;
			float padding = .5f;
			float w = Screen.width * (1 - padding);
			GUI.DrawTexture(new Rect(.5f * Screen.width - (w * frac), 20, w * frac * 2, 30), healthBarTexture);
		}
	}

	//Whether we can collide with things right now
	public bool CanCollide()
	{
		return canCollide && noCollisionTimer <= 0;
	}

	void OnTriggerEnter (Collider col)
    {
    	BulletProperties bullet = col.gameObject.GetComponent<BulletProperties>();
		if(bullet != null && bullet.owner == "player" && !(this.IsInvincible() && !CanCollide()))
        {
            bullet.Die();
			GetHurt(bullet.damage);
            return;
        }
    }
}
