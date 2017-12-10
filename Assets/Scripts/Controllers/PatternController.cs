using System.Collections;
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
		public float x, y, z, angle, speed, lifetime, scale, speedMultiplier;
		public string type, material, owner, movement, pattern;
		public bool destroyOnExitStage, destroyOnHit, synced;
	}

	[HideInInspector]
	public float leave;
	private bool blank = true;

	public void Reset()
	{
		if (script != null)
			CallLuaFunction("init", this, GetInstanceID());
		leave = 0;
		script = null;
		patternPath = null;
		blank = true;
	}

	void Start()
	{
		Init();
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
				CallLuaFunction("update", this, GetInstanceID(), Stage.deltaTime);
			}
			catch (ScriptRuntimeException ex)
			{
				Debug.LogError("Whoops, there was a runtime Lua error in '" + patternPath + "'   -   " + ex.DecoratedMessage);
			}
		}
	}

	public float GetStageTime()
	{
		return (float)Stage.time;
	}

	public float GetRealDeltaTime()
	{
		return Time.deltaTime;
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
		bd.movement = "$forward";
		bd.pattern = null;
		bd.angle = 0;
		bd.destroyOnHit = true;
		bd.scale = 1;
		bd.speedMultiplier = 1;
		bd.synced = true;
		return bd;
	}

	public void SpawnBullet(BulletData b)
	{
		//Debug.Log("BULLET "+patternPath);
		BulletFactory.Create(transform, b);
	}

}