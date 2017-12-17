using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProperties : MonoBehaviour {

	[HideInInspector]
	public string owner;
	[HideInInspector]
	public float lifetime, damage;
	[HideInInspector]
	public bool destroyOnExitStage, destroyOnHit;

	private Vector3 startScale;
	private float dieTimer, dieTime = 1f;
	private bool dying;

	public void Awake()
	{
		startScale = transform.localScale;
	}

	public bool inList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		lifetime -= Time.deltaTime;
		dieTimer -= Time.deltaTime;
		if (lifetime <= 0 && !dying)
			Die(true);
		if (transform.position.x < Stage.minX - 2 || transform.position.y < Stage.minY - 2 || transform.position.x > Stage.maxX + 2 || transform.position.y > Stage.maxY + 2)
			Die(false);
		if (dieTimer <= 0 && dying)
			Die(false);
		if(dying && dieTimer > 0)
		{
			Vector3 scale = transform.localScale;
			scale = Vector3.Lerp(Vector3.zero, transform.localScale, dieTimer / dieTime);
			transform.localScale = scale;
		}
	}

	public void Reset()
	{
		owner = null;
		lifetime = 0;
		destroyOnHit = false;
		destroyOnExitStage = false;
		damage = 20;
		dying = false;
		dieTimer = 0;
		transform.localScale = startScale;
		if (GetComponent<MovementController>() != null)
			GetComponent<MovementController>().Reset();
		if (GetComponent<PatternController>() != null)
			GetComponent<PatternController>().Reset();
	}

	public void Init()
	{
		if (GetComponent<MovementController>() != null)
			GetComponent<MovementController>().Init();
		if (GetComponent<PatternController>() != null)
			GetComponent<PatternController>().Init();
	}

	public void Die(bool animated)
	{
		if(!animated)
		{
			Stage.RemoveBullet(gameObject);
			gameObject.SetActive(false);
		}
		else
		{
			dieTimer = dieTime;
			dying = true;
		}
		//Destroy(gameObject);
	}
}
