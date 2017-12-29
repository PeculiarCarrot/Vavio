using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MoonSharp.Interpreter;
using GameAnalyticsSDK;

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
			//Load in enemy models
			enemyModels.Add("circle", GetEnemyModel("circle"));
			enemyModels.Add("pepper", GetEnemyModel("pepper"));
			enemyModels.Add("capsule", GetEnemyModel("capsule"));
			enemyModels.Add("cube", GetEnemyModel("cube"));

			//Load in enemy materials
			enemyMaterials.Add("red", GetEnemyMaterial("red"));
			enemyMaterials.Add("lightRed", GetEnemyMaterial("lightRed"));
			enemyMaterials.Add("orange", GetEnemyMaterial("orange"));
			enemyMaterials.Add("aqua", GetEnemyMaterial("aqua"));
			enemyMaterials.Add("darkAqua", GetEnemyMaterial("darkAqua"));
			enemyMaterials.Add("purple", GetEnemyMaterial("purple"));
			enemyMaterials.Add("darkPurple", GetEnemyMaterial("darkPurple"));
			enemyMaterials.Add("white", GetEnemyMaterial("white"));
			enemyMaterials.Add("transparent", GetEnemyMaterial("transparent"));
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

	public TextAsset[] spawnData;

	public int level;
	private LevelSpawnData spawns;

	public bool spawningEnabled = true;
	public Stage stage;
	private List<GameObject> liveEnemies = new List<GameObject>();
	public float timeUntilNext;

	public Text stageText, musicText;
	private float prepareLevelTimer, prepareLevelTime = 4;
	private bool preparingLevel = false;

	void Start()
	{
		timeUntilNext = 2;
		stageText.text = "";
		musicText.text = "";
		level = 6;

		if(Application.isEditor)
		{
			prepareLevelTime = .1f;
			timeUntilNext = 0;
		}

		if(PlayerPrefs.HasKey("diedOnLevel"))
		{
			level = PlayerPrefs.GetInt("diedOnLevel");
			PlayerPrefs.DeleteKey("diedOnLevel");
			PlayerPrefs.Save();
		}
		stage = GameObject.Find("Stage").GetComponent<Stage>();
		level--;
	}

	public void BeginLevel()
	{
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Song " + level, 0);
		stageText.text = "";
		musicText.text = "";
		preparingLevel = false;
		spawns = LevelSpawnData.FromJSON(new JSONObject(spawnData[level].text));
		stage.GetComponent<AudioSource>().clip = stage.songs[level];
		stage.GetComponent<AudioSource>().time = 160;
		stage.GetComponent<AudioSource>().Play();
		stage.GetComponent<Stage>().Begin();
		timeUntilNext = 9999999f;
		spawns.Begin(this);
	}

	public void PrepareLevel()
	{
		prepareLevelTimer = prepareLevelTime;
		preparingLevel = true;
		stageText.text = "Stage " + (level + 1);
		stage.GetComponent<AudioSource>().Stop();
		stage.GetComponent<Stage>().Clear();
		switch(level)
		{
			case 0:
				musicText.text = "15thDimension - Suddenly Time Told";
				break;
			case 1:
				musicText.text = "15thDimension - Suddenly Time Told (Part 2)";
				break;
			case 2:
				musicText.text = "Tobu - Infectious";
				break;
			case 3:
				musicText.text = "15thDimension - Until Then";
				break;
			case 4:
				musicText.text = "CØDE - Duck Face";
				break;
			case 5:
				musicText.text = "Tobu & Itro - Sunburst";
				break;
			case 6:
				musicText.text = "Alex Skrindo - Jumbo";
				break;
			default:
				musicText.text = "give the song a name you dope";
				break;
		}
		stage.GetComponent<Stage>().player.GetComponent<Player>().Regenerate();
	}

	public static Material TryGetEnemyMaterial(string s)
	{
		Material m = null;
		enemyMaterials.TryGetValue(s, out m);
		return m;
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
		pos.z = data.z;
		if (pos.z == float.MaxValue)
			pos.z = 0;

		GameObject e = Instantiate(model, pos, Quaternion.Euler(new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, model.transform.eulerAngles.z + data.rotation)));
		e.transform.localScale = e.transform.localScale * data.scale;
			
		foreach (Renderer renderer in e.GetComponentsInChildren<Renderer>())
		{
			renderer.material = material;
			Vector3 v = e.transform.position;
			v.z = data.z;
			renderer.gameObject.transform.position = v;
		}

		if(model.GetComponent<Enemy>() != null)
		{
			Stage.AddEnemy(e);
			e.GetComponent<Enemy>().leave = data.leave;
			e.GetComponent<Enemy>().reachGoalTime = data.reachGoalTime;
			e.GetComponent<Enemy>().invul = data.invul;
			e.GetComponent<Enemy>().canCollide = data.canCollide;
			e.GetComponent<Enemy>().introMovement = data.introMovement;
			e.GetComponent<Enemy>().maxHP = data.hp;
			e.GetComponent<Enemy>().hp = data.hp;
			e.GetComponent<Enemy>().givesCharge = data.givesCharge;
			e.GetComponent<Enemy>().boss = data.boss;
			e.GetComponent<Enemy>().mat = material;
			e.GetComponent<Enemy>().SetGoalPos(goalPos);
			liveEnemies.Add(e);
		}
		if (model.GetComponent<PatternController>() != null)
		{
			//Debug.Log("SET PATTERN: " + data.pattern);
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

	private float GetLineValue(float x, float y, float x2, float y2, float xx)
	{
		float slope = (y2 - y) / (x2 - x);
		float b = y - (slope * x);
		return slope * xx + b;
	}

	private float prevTime = 0;
	void Update () {

		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				level = 0;
				PrepareLevel();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				level = 1;
				PrepareLevel();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				level = 2;
				PrepareLevel();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				level = 3;
				PrepareLevel();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				level = 4;
				PrepareLevel();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				level = 5;
				PrepareLevel();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				level = 6;
				PrepareLevel();
			}
		}

		timeUntilNext -= Time.deltaTime;
		prepareLevelTimer -= Time.deltaTime;

		//Fade level intro text in/out
		if (preparingLevel)
		{
			float pad = .5f;
			if(prepareLevelTimer > prepareLevelTime - pad)
			{
				Color c = stageText.color;
				c.a = GetLineValue(prepareLevelTime, 0, prepareLevelTime - pad, 1, prepareLevelTimer);
				stageText.color = c;
				musicText.color = c;
			}
			else if(prepareLevelTimer < prepareLevelTime - (prepareLevelTime - 1))
			{
				Color c = stageText.color;
				c.a = GetLineValue(prepareLevelTime - (prepareLevelTime - pad), 1, 0, 0, prepareLevelTimer);
				stageText.color = c;
				musicText.color = c;
			}
		}

		if (prepareLevelTimer <= 0 && preparingLevel)
			BeginLevel();

		if(spawningEnabled && spawns != null)
		{
			spawns.Update();
		}
		if((prevTime > stage.GetComponent<AudioSource>().time || timeUntilNext <= 0) && !preparingLevel)
		{
			//Debug.Log(prevTime + "    "+ stage.GetComponent<AudioSource>().time);
			prevTime = 0;
			level++;
			stage.GetComponent<AudioSource>().time = 0;
			if (level >= stage.GetComponent<Stage>().songs.Length)
			{
				Application.Quit();
			}
			else
			{
				GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Song " + level, Mathf.RoundToInt(Stage.stage.player.GetComponent<Player>().hp));
				PrepareLevel();
			}
		}
		prevTime = stage.GetComponent<AudioSource>().time;
	}
}
