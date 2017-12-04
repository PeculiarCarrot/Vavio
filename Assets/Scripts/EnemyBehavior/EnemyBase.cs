using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour {

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
		Color32 c = mainRenderer.material.color;
		mainRenderer.material = null;
		mainRenderer.material.color = Color.white;
		mainRenderer.material.shader = Shader.Find("Unlit/Color");
		flashing = true;		
		yield return new WaitForSeconds(0.05f);
		mainRenderer.material = m;
		mainRenderer.material.color = c;
		flashing = false;
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
