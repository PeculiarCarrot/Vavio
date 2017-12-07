using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class PatternController : ScriptController{

	private static bool loaded;
	private static Dictionary<string, GameObject> bulletModels = new Dictionary<string, GameObject>();
	private static Dictionary<string, Material> bulletMaterials = new Dictionary<string, Material>();

	private static void Load()
	{
		//Load in bullet models
		bulletModels.Add("capsule", GetBulletModel("capsule"));

		//Load in bullet materials
		bulletMaterials.Add("red", GetBulletMaterial("red"));

		UserData.RegisterType<GameObject>();

		loaded = true;
	}

	private static GameObject GetBulletModel(string name)
	{
		return (GameObject) Resources.Load("Prefabs/BulletModels/"+name);
	}

	private static Material GetBulletMaterial(string name)
	{
		return (Material) Resources.Load("Materials/Bullets/"+name);
	}

	[MoonSharpUserData]
	public struct BulletData
	{
		public float x, y, z, angle, speed, lifetime;
		public string type, material, owner, movement;
		public bool destroyOnExitStage;
	}

	public PatternController() : base("Patterns/BulletSpawning/"){}

	new void Start()
	{
		if (!loaded)
			Load();
		
		base.Start();
		try{
			CallLuaFunction("init", this);
		}
		catch (ScriptRuntimeException ex)
		{
			Debug.LogError("Whoops, there was a runtime Lua error in '" + patternPath + "'   -   "+ex.DecoratedMessage);
		}
	}

	public float GetAngle()
	{
		return transform.eulerAngles.z;
	}

	public void Update()
	{
		try{
			CallLuaFunction("update", this, Stage.deltaTime);
		}
		catch (ScriptRuntimeException ex)
		{
			Debug.LogError("Whoops, there was a runtime Lua error in '" + patternPath + "'   -   "+ex.DecoratedMessage);
		}
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
		bd.movement = "General/forward.lua";
		bd.angle = GetAngle();
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
		bullet.transform.position = gameObject.transform.position + new Vector3(b.x, b.y, b.z);
		bullet.transform.rotation = gameObject.transform.rotation;
		bullet.transform.Rotate(0, 0, b.angle);

		foreach (Renderer renderer in bullet.GetComponentsInChildren<Renderer>())
			renderer.material = material;

		BulletProperties bp = bullet.GetComponent<BulletProperties>();
		bp.destroyOnExitStage = b.destroyOnExitStage;
		bp.owner = b.owner;
		bp.lifetime = b.lifetime;
	}

}