using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MoonSharp.Interpreter;

public class EnemySpawner : MonoBehaviour {
	
	private static bool loaded;
	private static Dictionary<string, GameObject> enemyModels = new Dictionary<string, GameObject>();
	private static Dictionary<string, Material> enemyMaterials = new Dictionary<string, Material>();

	public static void Load()
	{
		if (!loaded)
		{

			UserData.RegisterType<GameObject>();
			UserData.RegisterType<Vector3>();
			UserData.RegisterType<Transform>();
			UserData.RegisterType<Quaternion>();
			UserData.RegisterType<LuaMath>();
			UserData.RegisterType<float[]>();
			UserData.RegisterAssembly();
			//Load in bullet models
			enemyModels.Add("circle", GetEnemyModel("circle"));
			enemyModels.Add("pepper", GetEnemyModel("pepper"));
			enemyModels.Add("cube", GetEnemyModel("cube"));

			//Load in bullet materials
			enemyMaterials.Add("red", GetEnemyMaterial("red"));
			enemyMaterials.Add("lightRed", GetEnemyMaterial("lightRed"));
			enemyMaterials.Add("orange", GetEnemyMaterial("orange"));
		}

		loaded = true;
	}

	private static GameObject GetEnemyModel(string name)
	{
		return (GameObject) Resources.Load("Prefabs/Models/Enemies/"+name);
	}

	private static Material GetEnemyMaterial(string name)
	{
		return (Material) Resources.Load("Materials/Enemies/"+name);
	}

	public TextAsset spawnData;

	public int level;
	private LevelSpawnData spawns;

	public bool spawningEnabled = true;
	public Stage stage;
	private List<GameObject> liveEnemies = new List<GameObject>();

	void Start()
	{
		level = 0;
		if(PlayerPrefs.HasKey("diedOnLevel"))
		{
			level = PlayerPrefs.GetInt("diedOnLevel");
			PlayerPrefs.DeleteKey("diedOnLevel");
			PlayerPrefs.Save();
		}
		stage = GameObject.Find("Stage").GetComponent<Stage>();
		BeginLevel(level);
	}

	public void BeginLevel(int level)
	{
		spawns = AllLevelData.FromJSON(new JSONObject(spawnData.text), level);
		stage.GetComponent<AudioSource>().clip = stage.songs[level];
		stage.GetComponent<AudioSource>().time = 123;//0;
		stage.GetComponent<AudioSource>().Play();
		stage.GetComponent<Stage>().Begin();
		stage.GetComponent<Stage>().player.GetComponent<BulletBehaviorController>().Start();
		stage.GetComponent<Stage>().player.GetComponent<Player>().Regenerate();
		//stage.GetComponent<Stage>().player.GetComponent<BulletBehaviorController>().Regenerate();
		spawns.Begin(this);
	}

	public void SpawnEnemy(EnemySpawnData data)
	{
		GameObject model;
		Material material;

		if(enemyModels.TryGetValue(data.model, out model) == false)
		{
			Debug.LogError("No enemy type/model named '" + data.model + "' exists");
			return;
		}

		if(enemyMaterials.TryGetValue(data.material, out material) == false)
		{
			Debug.LogError("No enemy material named '" + data.material + "' exists");
			return;
		}

		Vector3 goalPos = Vector3.zero;
		goalPos.x = Stage.minX + Stage.width * (data.x == float.MaxValue ? .8f : data.x);
		goalPos.y = Stage.minY + Stage.height * (data.y == float.MaxValue ? .8f : data.y);

		Vector3 pos = goalPos;
		if(data.from == "left")
			pos.x = Stage.minX - 1;
		else if(data.from == "right")
			pos.x = Stage.maxX + 1;
		else if(data.from == "down")
			pos.y = Stage.minY - 1;
		else if(data.from == "up")
			pos.y = Stage.maxY + 1;

		GameObject e = Instantiate(model, pos, Quaternion.Euler(new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, model.transform.eulerAngles.z + data.rotation)));
		e.transform.localScale = e.transform.localScale * data.scale;

		foreach (Renderer renderer in e.GetComponentsInChildren<Renderer>())
			renderer.material = material;

		if(model.GetComponent<Enemy>() != null)
		{
			e.GetComponent<Enemy>().leave = data.leave;
			e.GetComponent<Enemy>().reachGoalTime = data.reachGoalTime;
			e.GetComponent<Enemy>().invul = data.invul;
			e.GetComponent<Enemy>().SetGoalPos(goalPos);
			liveEnemies.Add(e);
		}
		if (model.GetComponent<PatternController>() != null)
		{
			e.GetComponent<PatternController>().patternPath = data.pattern;
			e.GetComponent<PatternController>().leave = data.leave;
		}
		if(model.GetComponent<MovementController>() != null)
			e.GetComponent<MovementController>().patternPath = data.movement;
	}

	public List<GameObject> GetLiveEnemies()
	{
		return liveEnemies;
	}
	private float prevTime = 0;
	void Update () {
		if(spawningEnabled)
		{
			spawns.Update();
		}
		if(prevTime > stage.GetComponent<AudioSource>().time)
		{
			//Debug.Log(prevTime + "    "+ stage.GetComponent<AudioSource>().time);
			prevTime = 0;
			level++;
			stage.GetComponent<AudioSource>().time = 0;
			if(level >= stage.GetComponent<Stage>().songs.Length)
			{
				Debug.Log("Done");
				Application.Quit();
			}
			else
				BeginLevel(level);
		}
		prevTime = stage.GetComponent<AudioSource>().time;
	}
}
