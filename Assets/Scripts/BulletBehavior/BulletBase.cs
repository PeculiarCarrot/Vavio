using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour {

	public Vector3 velocity = Vector3.zero;
	//public float velocityMultiplier = 1;

	void Awake () {
		Initialize();
	}

	void Update () {
		UpdateBullet();
	}

	public void Initialize()
	{

	}
	
	public void UpdateBullet () {
		transform.position += velocity * Time.deltaTime;
	}
}
