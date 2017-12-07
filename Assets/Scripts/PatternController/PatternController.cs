using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class PatternController : MonoBehaviour{

	private static Dictionary<string, GameObject> bulletModels = new Dictionary<string, GameObject>();
	private static Dictionary<string, Material> bulletMaterials = new Dictionary<string, Material>();

	static PatternController()
	{
		//Load in bullet models
		bulletModels.Add("capsule", GetBulletModel("capsule"));

		//Load in bullet materials
		bulletMaterials.Add("red", GetBulletModel("red"));
	}

	private static GameObject GetBulletModel(string name)
	{
		return (GameObject) Resources.Load("Prefabs/BulletModels/"+name);
	}

	private static Material GetBulletMaterial(string name)
	{
		return (Material) Resources.Load("Materials/Bullets/"+name);
	}

	public struct BulletData
	{
		public float x, y, z, angle, speed;
		public string type, material;
	}
	
	public string patternPath;

	private Script script;

	public PatternController()
	{
		UserData.RegisterAssembly();

		//Make sure we're getting the right path regardless of operating system
		string path = Application.streamingAssetsPath;
		string[] directories = patternPath.Split('/');
		foreach (string dir in directories)
			path = System.IO.Path.Combine(path, dir);

		string code = System.IO.File.ReadAllText(path);
		script = new Script();
		script.DoString(code);
	}

	public void Start()
	{
		CallLuaFunction("init", this);
	}

	public void Update()
	{
		CallLuaFunction("update", this, Stage.deltaTime);
	}

	public DynValue CallLuaFunction(string function, params object[] parameters)
	{
		object func = script.Globals[function];
		if (func == null)
		{
			Debug.Log("'" + function + "' does not exist");
			return null;
		}

		return script.Call(func, parameters);
	}

	public BulletData NewBullet()
	{
		return new BulletData();
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

		GameObject bullet = Object.Instantiate(model);
		bullet.transform.position = gameObject.transform.position + new Vector3(b.x, b.y, b.z);
		bullet.transform.rotation = gameObject.transform.rotation;
		bullet.transform.Rotate(0, 0, b.angle);
		foreach (Renderer renderer in bullet.GetComponentsInChildren<Renderer>())
			renderer.material = material;
	}

}