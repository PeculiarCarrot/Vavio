using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MoonSharp.Interpreter;
using GameAnalyticsSDK;
using System.IO;

public class EnemySpawner : MonoBehaviour {
	
	private static bool loaded;
	private static Dictionary<string, GameObject> enemyModels = new Dictionary<string, GameObject>();
	private static Dictionary<string, Material> enemyMaterials = new Dictionary<string, Material>();

	//Loads in all of the resources that we will need to spawn in enemies and store them in a dictionary
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

	//Retrieves a loaded enemy model
	private static GameObject GetEnemyModel(string name)
	{
		return (GameObject) Resources.Load("Prefabs/Models/Enemies/"+name);
	}

	//Retrieves a loaded enemy material
	private static Material GetEnemyMaterial(string name)
	{
		return (Material) Resources.Load("Materials/Enemies/"+name);
	}

	//Filename of each level's spawn data
	public string[] spawnData;

	//The stage we're currently are (Stage 1 means this = 0)
	public int level;

	//All of the enemy spawn data for this level
	private LevelSpawnData spawns;

	//Debugging variable for if we want to disable spawns
	public bool spawningEnabled = true;
	public Stage stage;

	//List of all enemies on the stage
	private List<GameObject> liveEnemies = new List<GameObject>();

	//The text at the beginning of the stage that shows the song & artist
	public Text stageText, musicText;

	//How long to show the stage's title for
	private float prepareLevelTimer, prepareLevelTime = 4;
	//Whether we're showing the title at the beginning of the level
	private bool preparingLevel = false;
	//If we just finished the last stage, we use this to fade the screen to black
	bool endingGame;
	public float timeUntilNext;

	//This is used for analytical purposes. We store the reason the stage changed.
	public int reasonForLevelChange;

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
			//prepareLevelTime = .1f;
			level = 5;
		}

		//We save the requested stage to disk so we can find it when scenes change. I could do a persistent object here but bleh
		if (PlayerPrefs.HasKey("levelToStart"))
		{
			level = PlayerPrefs.GetInt("levelToStart");
			PlayerPrefs.DeleteKey("levelToStart");
			PlayerPrefs.Save();
		}
		stage = GameObject.Find("Stage").GetComponent<Stage>();
		//Subtracting a level here because beginning the level adds to it so it's kind of hacky
		level--;
	}

	public void CreateEmptySpawnData()
	{
		spawns = new LevelSpawnData();
	}

	public Tutorial tutorial;
	public string tutorialSpawnData;

	//Actually begin playing the song and spawning enemies
	public void BeginLevel()
	{
		//Wait for the song to be loaded from disk before we start the stage
		if (Stage.loadingSong)
			return;
		
		tutorial.Begin();
		Debug.Log("START SONG " + level);
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Song " + level, 0);
		stageText.text = "";
		musicText.text = "";
		preparingLevel = false;

		//Clear everything out so we don't have (too many) memory issues as we progress in the game
		stage.Clear();
		BulletFactory.ClearPool();

		//Load this stage's spawn data from disk
		if (EditorController.editingMode == 0)
			spawns = LevelSpawnData.FromJSON(new JSONObject(LoadFileString(level == -1 ? tutorialSpawnData : spawnData[level])));
		else
			level = -2;

		//This is used to test different parts of songs in the editor
		if (Application.isEditor)
			stage.GetComponent<AudioSource>().time = 0;
		else
			stage.GetComponent<AudioSource>().time = 0;

		//BEGIN
		if(EditorController.editingMode == 0)
			stage.GetComponent<AudioSource>().Play();
		stage.Begin();
		timeUntilNext = 9999999f;
		spawns.Begin(this);
	}

	//Read the string from the given text file that resides in /LevelSpawnData/
	public string LoadFileString(string givenPath)
	{
		givenPath = Path.Combine("LevelSpawnData", givenPath);

		//Make sure we're getting the right path regardless of operating system
		string path = "";
		if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
			path = Application.streamingAssetsPath;
		else if(Application.platform == RuntimePlatform.IPhonePlayer)
			path = Application.dataPath + "/Raw";
		else if(Application.platform == RuntimePlatform.Android)
			path = "jar:file://" + Application.dataPath + "!/assets/";
		string[] directories = givenPath.Split('/');
		foreach (string dir in directories)
			path = Path.Combine(path, dir);

		return File.ReadAllText(path);
	}

	//Start the introduction text for the stage
	public void PrepareLevel()
	{
		//If we need to do the tutorial, set the level to -1
		if(level == 0 && PlayerPrefs.GetInt("didTutorial") == 0)
		{
			level = -1;
		}
		prepareLevelTimer = prepareLevelTime;
		preparingLevel = true;
		stageText.text = level == -1 ? "Tutorial" : "Stage " + (level + 1);
		stage.GetComponent<AudioSource>().Stop();
		
		stage.LoadLevel(level);
		musicText.text = level == -1 ? "15thDimension - Loss for Words" : Stage.GetSongName(level);
		stage.player.GetComponent<Player>().Regenerate();
	}

	public static Material TryGetEnemyMaterial(string s)
	{
		Material m = null;
		enemyMaterials.TryGetValue(s, out m);
		return m;
	}

	//Spawn an enemy with the correct properties
	public void SpawnEnemy(EnemySpawnData data)
	{
		GameObject model;
		Material material;

		//Grab the correct enemy model and throw an error if it doesn't exist
		if(enemyModels.TryGetValue(data.model, out model) == false)
		{
			Debug.LogError("No enemy type/model named '" + data.model + "' exists");
			return;
		}

		//Grab the correct enemy material and throw an error if it doesn't exist
		if(enemyMaterials.TryGetValue(data.material, out material) == false)
		{
			Debug.LogError("No enemy material named '" + data.material + "' exists");
			return;
		}

		//Positions are stored in the spawn data as percentages of the screen width and height, here we convert them to world coords
		Vector3 goalPos = Vector3.zero;
		goalPos.x = Stage.minX + Stage.width * (data.x == float.MaxValue ? .8f : data.x);
		goalPos.y = Stage.minY + Stage.height * (data.y == float.MaxValue ? .8f : data.y);
		if (data.absoluteZ)
			goalPos.z = data.z;

		//Determining where the enemy should spawn from
		Vector3 pos = goalPos;
		if(data.from == "left")
			pos.x = Stage.minX - 1;
		else if(data.from == "right")
			pos.x = Stage.maxX + 1;
		else if(data.from == "down")
			pos.y = Stage.minY - 1;
		else if(data.from == "up")
			pos.y = Stage.maxY + 1;

		//Spawn the dude
		GameObject e = Instantiate(model, pos, Quaternion.Euler(new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, model.transform.eulerAngles.z + data.rotation)));
		e.transform.localScale = e.transform.localScale * data.scale;

		//absolute Z means we move the entire object rather than just the renderer
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


		//Apply all the property stuff
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
			enemy.growsOnHit = data.growsOnHit;
			enemy.mat = material;
			enemy.SetGoalPos(goalPos);
			liveEnemies.Add(e);
		}

		//Set controller properties of the enemy
		if (model.GetComponent<PatternController>() != null)
		{
			e.GetComponent<PatternController>().patternPath = data.pattern;
			e.GetComponent<PatternController>().leave = data.leave;
		}
		if(model.GetComponent<MovementController>() != null)
			e.GetComponent<MovementController>().patternPath = data.movement;
	}

	//Returns all currently living enemies
	public List<GameObject> GetLiveEnemies()
	{
		return liveEnemies;
	}

	//A simple function to get a value from a defined line
	private float GetLineValue(float x, float y, float x2, float y2, float xx)
	{
		float slope = (y2 - y) / (x2 - x);
		float b = y - (slope * x);
		return slope * xx + b;
	}

	//The timestamp of the song last update
	private float prevTime = 0;

	public Texture blackTexture;
	private float blackScreenAlpha;

	//Draw the fading to black screen after the final level
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

		if (EditorController.inMenu)
			return;

		//If we just finished the last level, start fading to black
		if(endingGame)
		{
			blackScreenAlpha += 1 * Time.deltaTime;
			if (blackScreenAlpha > 2)
				SceneManager.LoadScene("credits");
			return;
		}

		//Keyboard shortcuts to quickly swap between levels in the editor
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
			if (EditorController.editingMode != 0)
				prepareLevelTimer = 0;
		}

		if (prepareLevelTimer <= 0 && preparingLevel)
			BeginLevel();

		if(spawningEnabled && spawns != null)
			spawns.Update();

		//If we're done with the song, start preparing the next level
		if((prevTime > stage.GetComponent<AudioSource>().time || timeUntilNext <= 0) && !preparingLevel)
		{
			if(prevTime > stage.GetComponent<AudioSource>().time)
				reasonForLevelChange = COMPLETE;

			prevTime = 0;
			stage.GetComponent<AudioSource>().time = 0;

			if (level + 1 >= stage.songs.Length)
				EndGame();
			else
			{
				if(reasonForLevelChange == COMPLETE)
				{
					Debug.Log("COMPLETE SONG " + level+" with HP: "+Mathf.RoundToInt(Stage.stage.player.GetComponent<Player>().hp));
					if(PlayerPrefs.GetInt("level") < level + 1)
					{
						PlayerPrefs.SetInt("level", level + 1);
						stage.player.GetComponent<Player>().SetChosenAbility();
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
		stage.player.GetComponent<Player>().SetChosenAbility();
		PlayerPrefs.Save();
	}
}
