using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProperties : MonoBehaviour {

	[HideInInspector]
	public string owner;
	[HideInInspector]
	public float lifetime;
	[HideInInspector]
	public bool destroyOnExitStage, destroyOnHit;

	private Vector3 startScale;

	public void Awake()
	{
		startScale = gameObject.transform.localScale;
	}

	public bool inList;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		lifetime -= Time.deltaTime;
		if (lifetime <= 0)
			Die();
		if (transform.position.x < Stage.minX - 2 || transform.position.y < Stage.minY - 2 || transform.position.x > Stage.maxX + 2 || transform.position.y > Stage.maxY + 2)
			Die();
	}

	public void Reset()
	{
		owner = null;
		lifetime = 0;
		destroyOnHit = false;
		destroyOnExitStage = false;
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

	public void Die()
	{
		Stage.RemoveBullet(gameObject);
		gameObject.SetActive(false);
		//Destroy(gameObject);
	}
}
