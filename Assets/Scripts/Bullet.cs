using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour {

	protected Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		DoStart();
	}
	
	// Update is called once per frame
	void Update () {
		DoUpdate();
		transform.position += velocity * Time.deltaTime;
	}

	public abstract void DoUpdate();
	public abstract void DoStart();
}
