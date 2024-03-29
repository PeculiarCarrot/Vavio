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
			bulletModels.Add("capsule", LoadBulletModel("capsule"));
			bulletModels.Add("circle", LoadBulletModel("circle"));
			bulletModels.Add("cube", LoadBulletModel("cube"));
			bulletModels.Add("playerBullet", LoadBulletModel("playerBullet"));

			//Load in bullet materials
			bulletMaterials.Add("red", LoadBulletMaterial("red"));
			bulletMaterials.Add("lightRed", LoadBulletMaterial("lightRed"));
			bulletMaterials.Add("darkRed", LoadBulletMaterial("darkRed"));
			bulletMaterials.Add("orange", LoadBulletMaterial("orange"));
			bulletMaterials.Add("aqua", LoadBulletMaterial("aqua"));
			bulletMaterials.Add("darkAqua", LoadBulletMaterial("darkAqua"));
			bulletMaterials.Add("darkPurple", LoadBulletMaterial("darkPurple"));
			bulletMaterials.Add("purple", LoadBulletMaterial("purple"));
			bulletMaterials.Add("white", LoadBulletMaterial("white"));
			bulletMaterials.Add("lightAqua", LoadBulletMaterial("lightAqua"));
			bulletMaterials.Add("green", LoadBulletMaterial("green"));
			bulletMaterials.Add("inverted", LoadBulletMaterial("inverted"));
		}

		loaded = true;
	}

	public static GameObject GetBulletModel(string type)
	{
		GameObject model;
		if(bulletModels.TryGetValue(type, out model) == false)
		{
			Debug.LogError("No bullet type/model named '" + type + "' exists");
		}
		return model;
	}

	public static Material GetBulletMaterial(string type)
	{
		Material material;
		if(bulletMaterials.TryGetValue(type, out material) == false)
		{
			Debug.LogError("No bullet material named '" + type + "' exists");
		}
		return material;
	}

	private static GameObject LoadBulletModel(string name)
	{
		return (GameObject) Resources.Load("Prefabs/Models/Bullets/"+name);
	}

	private static Material LoadBulletMaterial(string name)
	{
		return (Material) Resources.Load("Materials/Bullets/"+name);
	}

	[MoonSharpUserData]
	public struct BulletData
	{
		public float x, y, z, angle, speed, lifetime, scale, speedMultiplier, damage, turn;
		public string type, material, owner, movement, pattern;
		public bool destroyOnExitStage, destroyOnHit, synced;
	}

	[HideInInspector]
	public float leave;
	private bool blank = true;

	private LuaMath luaMath = new LuaMath();

	public void Reset()
	{
		//if (script != null)
		//	CallLuaFunction("init", this, GetInstanceID());
		leave = 0;
		script = null;
		patternPath = null;
		blank = true;
	}

	public float StageWidth()
	{
		return Stage.width;
	}

	public float StageHeight()
	{
		return Stage.height;
	}

	void Start()
	{
		Init();
	}

	public LuaMath Math()
	{
		return luaMath;
	}

	public void Init()
	{
		if (blank)
		{
			SetDefaultPath("Patterns/BulletSpawning/");
			base.DoInit();
			if (script != null)
			{
				try
				{
					CallLuaFunction("init", this, GetInstanceID());
				}
				catch (ScriptRuntimeException ex)
				{
					Debug.LogError("Whoops, there was a runtime Lua error in '" + patternPath + "'   -   " + ex.DecoratedMessage);
				}
			}
		}
		blank = false;
	}

	public float StageMinX()
	{
		return Stage.minX;
	}

	public float StageMinY()
	{
		return Stage.minY;
	}

	public void Print(string message)
	{
		Debug.Log(message);
	}

	public float GetAngle()
	{
		return transform.eulerAngles.z;
	}

	public float RandomValue()
	{
		return Random.value;
	}

	public float RandomRange(float x, float y)
	{
		return Random.Range(x, y);
	}

	public void Update()
	{
		if (script != null)
		{
			try
			{
				CallLuaFunction("update", this, GetInstanceID(), Time.deltaTime);
			}
			catch (ScriptRuntimeException ex)
			{
				Debug.LogError("Whoops, there was a runtime Lua error in '" + patternPath + "'   -   " + ex.DecoratedMessage);
			}
		}
	}

	public float GetStageDeltaTime()
	{
		return Stage.deltaTime;
	}

	public float GetStageTime()
	{
		return (float)Stage.time;
	}

	public float GetRealDeltaTime()
	{
		return Time.deltaTime;
	}

	public float GetAngleToPlayer()
	{
		Vector3 p = Stage.stage.player.transform.position;
		return Mathf.Atan2(p.y - transform.position.y, p.x - transform.position.x) * Mathf.Rad2Deg;
	}

	public float[] GetFireTimes(float bulletsPerSecond, float initialDelay)
	{
		return GetFireTimes(bulletsPerSecond, initialDelay, 0, 0);
	}

	public float[] GetFireTimes(float bulletsPerSecond, float initialDelay, float secondsToFire, float secondsToPause)
	{
		float secondsPerBullet = 1 / bulletsPerSecond;
		List<float> times = new List<float>();
		float timeUntilChange = secondsToFire;
		bool paused = false;
		for(float i = initialDelay; i < leave + initialDelay; i += secondsPerBullet)
		{
			times.Add(i);
		}

		float k = secondsToFire;
		bool firing = true;

		if(secondsToPause > 0 && secondsToFire > 0)
			for(float i = initialDelay; i < leave + initialDelay; i += secondsToFire)
			{
				for(int j = times.Count - 1; j >= 0; j--)
				{
					float f = times[j];
					if(f > i && f <= i + k && !firing)
					{
						times.RemoveAt(j);
					}
				}
				if (firing)
				{
					k = secondsToPause;
					firing = false;
				}
				else
				{
					k = secondsToFire;
					firing = true;
				}
			}

		float[] fireTimes = new float[times.Count];
		for(int i = 0; i < fireTimes.Length; i++)
		{
			fireTimes[i] = times[i] + (float)Stage.time;
		}
		return fireTimes;
	}

	public float GetX()
	{
		return transform.position.x;
	}

	public float GetY()
	{
		return transform.position.y;
	}

	public BulletData NewBullet()
	{
		BulletData bd = new BulletData();
		bd.destroyOnExitStage = true;
		bd.speed = 1;
		bd.lifetime = 30f;
		bd.owner = "enemy";
		bd.type = "capsule";
		bd.material = "red";
		bd.movement = "$forward";
		bd.pattern = null;
		bd.angle = 0;
		bd.destroyOnHit = true;
		bd.scale = 1;
		bd.speedMultiplier = 1;
		bd.synced = true;
		bd.damage = 20;
		bd.turn = 0;
		return bd;
	}

	public void SpawnBullet(BulletData b)
	{
		//Debug.Log("BULLET "+patternPath);
		Stage.AddBullet(BulletFactory.Create(transform, b));
	}

}