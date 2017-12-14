using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ship : MonoBehaviour {

	protected Vector3 velocity = Vector3.zero;
	public float accel = .9f;
	protected float friction = .9f;
	protected Vector3 baseRot;
	public static GameObject stage;

	public float maxHP = 100;
	protected float hp;
	private bool flashing;
	
	void Start () {
		if(stage == null)
			stage = GameObject.Find("Stage");
		DoStart();
		baseRot = transform.eulerAngles;
		hp = maxHP;
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

	public void DoUpdate () {
		velocity *= friction;
		transform.position += velocity * (1 / 60f);
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
}
