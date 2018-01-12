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
			enemyModels.Add("mortalsBoss", GetEnemyModel("mortalsBoss"));
			enemyModels.Add("finalBoss", GetEnemyModel("finalBoss"));

			//Load in enemy materials
			enemyMaterials.Add("red", GetEnemyMaterial("red"));
			enemyMaterials.Add("lightRed", GetEnemyMaterial("lightRed"));
			enemyMaterials.Add("orange", GetEnemyMaterial("orange"));
			enemyMaterials.Add("aqua", GetEnemyMaterial("aqua"));
			enemyMaterials.Add("darkAqua", GetEnemyMaterial("darkAqua"));
			enemyMaterials.Add("darkRed", GetEnemyMaterial("darkRed"));
			enemyMaterials.Add("purple", GetEnemyMaterial("purple"));
			enemyMaterials.Add("darkPurple", GetEnemyMaterial("darkPurple"));
			enemyMaterials.Add("white", GetEnemyMaterial("white"));
			enemyMaterials.Add("transparent", GetEnemyMaterial("transparent"));
			enemyMaterials.Add("inverted", GetEnemyMaterial("inverted"));
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

	public string[] spawnData;

	public int level;
	private LevelSpawnData spawns;

	public bool spawningEnabled = true;
	public Stage stage;
	private List<GameObject> liveEnemies = new List<GameObject>();
	public float timeUntilNext;

	public Text stageText, musicText;
	private float prepareLevelTimer, prepareLevelTime = 4;
	private bool preparingLevel = false;
	public int reasonForLevelChange;
	bool endingGame;

	public const int DEATH = 0;
	public const int COMPLETE = 1;
	public const int MANUAL = 2;

	void Start()
	{
		timeUntilNext = 2;
		stageText.text = "";
		musicText.text = "";
		level = 0;

		if(Application.isEditor)
		{
			prepareLevelTime = .1f;
			timeUntilNext = 0;
			level = 11;
		}

		if (PlayerPrefs.HasKey("levelToStart"))
		{
			level = PlayerPrefs.GetInt("levelToStart");
			PlayerPrefs.DeleteKey("levelToStart");
			PlayerPrefs.Save();
		}
		stage = GameObject.Find("Stage").GetComponent<Stage>();
		level--;
	}

	public void BeginLevel()
	{
		if (Stage.loadingSong)
			return;
		Debug.Log("START SONG " + level);
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Song " + level, 0);
		stageText.text = "";
		musicText.text = "";
		preparingLevel = false;
		spawns = LevelSpawnData.FromJSON(new JSONObject(LoadFileString(spawnData[level])));
		if (Application.isEditor)
			stage.GetComponent<AudioSource>().time = 0;
		else
			stage.GetComponent<AudioSource>().time = 0;
		stage.GetComponent<AudioSource>().Play();
		stage.GetComponent<Stage>().Begin();
		timeUntilNext = 9999999f;
		spawns.Begin(this);
	}

	public string LoadFileString(string givenPath)
	{
		givenPath = "LevelSpawnData/" + givenPath;
		//Make sure we're getting the right path regardless of operating system
		string path = "";
		if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
			path = Application.dataPath + "/StreamingAssets";
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
			path = Application.dataPath + "/Raw";
		else if(Application.platform == RuntimePlatform.Android)
			path = "jar:file://" + Application.dataPath + "!/assets/";
		string[] directories = givenPath.Split('/');
		foreach (string dir in directories)
			path = System.IO.Path.Combine(path, dir);

		return System.IO.File.ReadAllText(path);
	}

	public void PrepareLevel()
	{
		prepareLevelTimer = prepareLevelTime;
		preparingLevel = true;
		stageText.text = "Stage " + (level + 1);
		stage.GetComponent<AudioSource>().Stop();
		stage.GetComponent<Stage>().Clear();
		stage.LoadLevel(level);
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
			case 7:
				musicText.text = "Warriyo - Mortals (feat. Laura Brehm)";
				break;
			case 8:
				musicText.text = "Disfigure - Blank";
				break;
			case 9:
				musicText.text = "Kovan & Electro-Light - Skyline";
				break;
			case 10:
				musicText.text = "Inukshuk - The Long Road Home";
				break;
			case 11:
				musicText.text = "NIVIRO - The Guardian of Angels";
				break;
			default:
				musicText.text = "Song not found";
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
		if (data.absoluteZ)
			goalPos.z = data.z;

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

		if(data.absoluteZ)
		{
			Vector3 newPos = e.transform.position;
			newPos.z = data.z;
			e.transform.position = newPos;
			e.GetComponent<Rigidbody>().MovePosition(newPos);
		}
		else
		{
			foreach (Renderer renderer in e.GetComponentsInChildren<Renderer>())
			{
				renderer.material = material;
				Vector3 v = renderer.transform.localPosition;
				v.z = data.z;
				renderer.transform.localPosition = v;
			}
		}

		if(model.GetComponent<Enemy>() != null)
		{
			Stage.AddEnemy(e);
			Enemy enemy = e.GetComponent<Enemy>();
			enemy.leave = data.leave;
			enemy.reachGoalTime = data.reachGoalTime;
			enemy.invul = data.invul;
			enemy.canCollide = data.canCollide;
			enemy.canCollideWithBullets = data.canCollideWithBullets;
			enemy.introMovement = data.introMovement;
			enemy.maxHP = data.hp;
			enemy.hp = data.hp;
			enemy.givesCharge = data.givesCharge;
			enemy.boss = data.boss;
			enemy.mat = material;
			enemy.SetGoalPos(goalPos);
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

	public Texture blackTexture;
	private float blackScreenAlpha;

	void OnGUI()
	{
		Color prevColor = GUI.color;
		int prevDepth = GUI.depth;

		GUI.depth = -1;
		GUI.color = new Color(prevColor.r, prevColor.g, prevColor.b, Mathf.Min(blackScreenAlpha, 1f));
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), blackTexture);

		//GUI.depth = prevDepth;
		GUI.color = prevColor;
	}

	void Update () {

		if(endingGame)
		{
			blackScreenAlpha += 1 * Time.deltaTime;
			if (blackScreenAlpha > 2)
				SceneManager.LoadScene("credits");
			return;
		}

		if (Application.isEditor && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
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
			else if (Input.GetKeyDown(KeyCode.Alpha8))
			{
				level = 7;
				PrepareLevel();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha9))
			{
				level = 8;
				PrepareLevel();
			}
			else if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				level = 9;
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
			if(prevTime > stage.GetComponent<AudioSource>().time)
			{
				reasonForLevelChange = COMPLETE;
			}
			//Debug.Log(prevTime + "    "+ stage.GetComponent<AudioSource>().time);
			prevTime = 0;
			stage.GetComponent<AudioSource>().time = 0;
			if (level + 1 >= stage.GetComponent<Stage>().songs.Length)
			{
				EndGame();
			}
			else
			{
				if(reasonForLevelChange == COMPLETE)
				{
					Debug.Log("COMPLETE SONG " + level+" with HP: "+Mathf.RoundToInt(Stage.stage.player.GetComponent<Player>().hp));
					if(PlayerPrefs.GetInt("level") < level + 1)
					{
						PlayerPrefs.SetInt("level", level + 1);
						PlayerPrefs.Save();
					}
					GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Song " + level, Mathf.RoundToInt(Stage.stage.player.GetComponent<Player>().hp));
				}
				level++;
				PrepareLevel();
			}
		}
		prevTime = stage.GetComponent<AudioSource>().time;
	}

	void EndGame()
	{
		endingGame = true;
		PlayerPrefs.SetInt("level", 11);
		PlayerPrefs.Save();
	}
}
