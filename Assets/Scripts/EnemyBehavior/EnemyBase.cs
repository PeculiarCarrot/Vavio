using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : ShooterBase {

	public Vector3 velocity = Vector3.zero;
	public float accel = .9f;
	public float friction = .9f;
	protected float rotSpeed, rotAccel = 170f, rotFric = .99f;
	protected Vector3 baseRot;
	public static GameObject stage;

	public float maxHP = 100;
	protected float hp;
	private bool flashing;
	protected bool reachedGoal = true;

	// Use this for initialization
	void Start () {
		if(stage == null)
			stage = GameObject.Find("Stage");
		DoStart();
		baseRot = transform.eulerAngles;
		hp = maxHP;
	}

	public bool CanDoBehavior()
	{
		return reachedGoal;
	}

	IEnumerator CollideFlash(Renderer mainRenderer)
	{
		Material m = mainRenderer.material;
		Color c = mainRenderer.material.color;
		mainRenderer.material = null;
		mainRenderer.material.color = ChangeColorBrightness(c, .2f);
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

	public void Flash()
	{
		if(!flashing)
			foreach(Renderer r in GetComponentsInChildren<Renderer>())
				StartCoroutine(CollideFlash(r));
	}
	
	// Update is called once per frame
	public void DoUpdate () {
		velocity *= friction;
		transform.position += velocity * (1 / 60f);
		rotSpeed *= rotFric;
		//transform.eulerAngles = baseRot + new Vector3(0, rotSpeed * Time.deltaTime, 0);
	}


	public void GetHurt(float damage)
	{
		hp -= damage;
	}

	public void Die()
	{
		Destroy(gameObject);
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
}
