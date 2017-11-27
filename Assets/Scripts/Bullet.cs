using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : BulletBase {
	public static GameObject stage;

	// Use this for initialization
	void Awake () {
		Initialize();
		if(stage == null)
		{
			stage = GameObject.Find("Stage");
		}
		DoStart();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateBullet();
		DoUpdate();
	}

	public abstract void DoUpdate();
	public abstract void DoStart();
}
