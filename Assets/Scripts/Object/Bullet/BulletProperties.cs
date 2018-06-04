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

	private float edgeDist = 2;
	private float z;
	private Renderer renderer;
	private TrailRenderer trailRenderer;

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
		if(dying)
			dieTimer -= Time.deltaTime;
		if (lifetime <= 0 && !dying)
			Die(true);
		if (destroyOnExitStage && (transform.position.x < Stage.minX - edgeDist || transform.position.y < Stage.minY - edgeDist || transform.position.x > Stage.maxX + edgeDist || transform.position.y > Stage.maxY + edgeDist))
			Die(false);
		if (dieTimer <= 0 && dying)
			Die(false);
		if(dying && dieTimer > 0)
		{
			Vector3 scale = transform.localScale;
			scale = Vector3.Lerp(Vector3.zero, transform.localScale, dieTimer / dieTime);
			transform.localScale = scale;
		}
		Vector3 newPos = renderer.transform.position;
		newPos.z = z;
		renderer.transform.position = newPos;
		if(trailRenderer != null)
		{
			newPos = trailRenderer.transform.position;
			newPos.z = z;
			trailRenderer.transform.position = newPos;
		}

		if (trailRenderer != null)
		{
			trailRenderer.enabled = true;
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

		if (trailRenderer != null)
			trailRenderer.enabled = false;
	}

	public void Init()
	{
		trailRenderer = null;
		renderer = GetComponentInChildren<MeshRenderer>();
		trailRenderer = GetComponentInChildren<TrailRenderer>();

		if (trailRenderer != null)
			trailRenderer.Clear();

		if (GetComponent<MovementController>() != null)
			GetComponent<MovementController>().Init();
		if (GetComponent<PatternController>() != null)
			GetComponent<PatternController>().Init();
		z = renderer.transform.position.z;
	}

	void FixedUpdate()
	{
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
