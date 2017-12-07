﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class PatternController : ScriptController{

	private static bool loaded;
	private static Dictionary<string, GameObject> bulletModels = new Dictionary<string, GameObject>();
	private static Dictionary<string, Material> bulletMaterials = new Dictionary<string, Material>();

	public static void Load()
	{
		if (!loaded)
		{
			//Load in bullet models
			bulletModels.Add("capsule", GetBulletModel("capsule"));

			//Load in bullet materials
			bulletMaterials.Add("red", GetBulletMaterial("red"));
		}

		loaded = true;
	}

	private static GameObject GetBulletModel(string name)
	{
		return (GameObject) Resources.Load("Prefabs/Models/Bullets/"+name);
	}

	private static Material GetBulletMaterial(string name)
	{
		return (Material) Resources.Load("Materials/Bullets/"+name);
	}

	[MoonSharpUserData]
	public struct BulletData
	{
		public float x, y, z, angle, speed, lifetime, scale;
		public string type, material, owner, movement, pattern;
		public bool destroyOnExitStage, destroyOnHit;
	}

	public PatternController() : base("Patterns/BulletSpawning/"){}

	new void Start()
	{
		base.Start();
		if (script != null)
		{
			try
			{
				CallLuaFunction("init", this);
			}
			catch (ScriptRuntimeException ex)
			{
				Debug.LogError("Whoops, there was a runtime Lua error in '" + patternPath + "'   -   " + ex.DecoratedMessage);
			}
		}
	}

	public void Print(string message)
	{
		Debug.Log(message);
	}

	public float GetAngle()
	{
		return transform.eulerAngles.z;
	}

	public void Update()
	{
		if (script != null)
		{
			try
			{
				CallLuaFunction("update", this, Stage.deltaTime);
			}
			catch (ScriptRuntimeException ex)
			{
				Debug.LogError("Whoops, there was a runtime Lua error in '" + patternPath + "'   -   " + ex.DecoratedMessage);
			}
		}
	}

	public float[] GetFireTimes(float bulletsPerSecond, float initialDelay, float leaveTime)
	{
		return GetFireTimes(bulletsPerSecond, initialDelay, leaveTime, 0, 0);
	}

	public float GetStageTime()
	{
		return (float)Stage.time;
	}

	public float[] GetFireTimes(float bulletsPerSecond, float initialDelay, float leaveTime, float secondsToFire, float secondsToPause)
	{
		float secondsPerBullet = 1 / bulletsPerSecond;
		List<float> times = new List<float>();
		float timeUntilChange = secondsToFire;
		bool paused = false;
		for(float i = initialDelay; i < leaveTime; i += secondsPerBullet)
		{
			if(secondsToPause > 0)
			{
				//Debug.Log(timeUntilChange);
				timeUntilChange -= secondsPerBullet;
				if(timeUntilChange <= 0)
				{
					paused = !paused;
					timeUntilChange = paused ? secondsToPause : secondsToFire;
				}
			}
			if(!paused)
				times.Add(i);
		}
		float[] fireTimes = new float[times.Count];
		for(int i = 0; i < fireTimes.Length; i++)
		{
			fireTimes[i] = times[i] + (float)Stage.time;
		}
		return fireTimes;
	}

	public BulletData NewBullet()
	{
		BulletData bd = new BulletData();
		bd.destroyOnExitStage = true;
		bd.speed = 1;
		bd.lifetime = 99f;
		bd.owner = "enemy";
		bd.type = "capsule";
		bd.material = "red";
		bd.movement = "General/forward";
		bd.pattern = null;
		bd.angle = 0;
		bd.destroyOnHit = true;
		bd.scale = 1;
		return bd;
	}

	public void SpawnBullet(BulletData b)
	{
		GameObject model;
		Material material;

		if(bulletModels.TryGetValue(b.type, out model) == false)
		{
			Debug.LogError("No bullet type/model named '" + b.type + "' exists");
			return;
		}
		if(bulletMaterials.TryGetValue(b.material, out material) == false)
		{
			Debug.LogError("No bullet material named '" + b.material + "' exists");
			return;
		}

		//Spawn the bullet and apply all properties to it
		GameObject bullet = Object.Instantiate(model);
		bullet.GetComponent<MovementController>().patternPath = b.movement;
		bullet.GetComponent<PatternController>().patternPath = b.pattern;
		bullet.transform.position = gameObject.transform.position + new Vector3(b.x, b.y, b.z);
		bullet.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + b.angle);
		bullet.transform.localScale = bullet.transform.localScale * b.scale;

		foreach (Renderer renderer in bullet.GetComponentsInChildren<Renderer>())
			renderer.material = material;

		BulletProperties bp = bullet.GetComponent<BulletProperties>();
		bp.destroyOnExitStage = b.destroyOnExitStage;
		bp.destroyOnHit = b.destroyOnHit;
		bp.owner = b.owner;
		bp.lifetime = b.lifetime;
	}

}