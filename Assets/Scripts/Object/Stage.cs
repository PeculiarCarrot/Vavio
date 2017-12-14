using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {

	public static float deltaTime;
	public static double lastTime, timeOffset, time;
	private static double beginTime;

	public GameObject player;
	public GameObject spawner;
	[HideInInspector]
	public static float minX, minY, maxX, maxY, width, height;
	public static Stage stage;
	public AudioClip[] songs;
	public AudioSource song;

	private static List<GameObject> bullets = new List<GameObject>();
	private static List<GameObject> enemies = new List<GameObject>();

	void Awake()
	{
		PatternController.Load();
		EnemySpawner.Load();
	}

	// Use this for initialization
	void Start () {

		UpdateScreenPositions();
		deltaTime = 0;
		time = 0;
		lastTime = 0;
		stage = this;
		//Debug.Log(minX+", "+minY +" - "+maxX+", "+maxY);
	}

	public static void AddEnemy(GameObject o)
	{
		enemies.Add(o);
	}

	public static void RemoveEnemy(GameObject o)
	{
		enemies.Remove(o);
	}

	public static void AddBullet(GameObject o)
	{
		bullets.Add(o);
	}

	public static void RemoveBullet(GameObject o)
	{
		bullets.Remove(o);
	}

	public static List<GameObject> GetBullets()
	{
		return bullets;
	}

	public static List<GameObject> GetEnemies()
	{
		return enemies;
	}

	public static void UpdateScreenPositions()
	{
		Vector3 max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
		Vector3 min = Camera.main.ScreenToWorldPoint(Vector3.zero);
		maxX = max.x;
		maxY = max.y;
		minX = min.x;
		minY = min.y;
		width = maxX - minX;
		height = maxY - minY;
	}

	public void Clear()
	{
		foreach (GameObject b in bullets)
		{
			Destroy(b);
		}
		foreach (GameObject b in enemies)
		{
			Destroy(b);
		}
	}

	public void Begin()
	{
		bullets.Clear();
		enemies.Clear();
		BulletFactory.ClearPool();
		LuaScriptFactory.ClearPool();
		beginTime = song.time - song.timeSamples/song.clip.frequency;
		time = song.time - beginTime;
		lastTime = time;
		deltaTime = 0;
		timeOffset = 0;//AudioSettings.dspTime - song.timeSamples/song.clip.frequency;
	}

	void FixedUpdate()
	{
		time = song.time - beginTime;
		deltaTime = (float)(time - lastTime);
		lastTime = time;
	}

	private float timeVelocity;
	
	void Update () {
		UpdateScreenPositions();

		float f = Time.timeScale;
		if(Input.GetKey("right"))
		{
			f *= 1.01f;
		}
		else if(Input.GetKey("left"))
		{
			f -= .01f;
		}
		else
		{
			f = Mathf.Max(f, .2f);
			f = Mathf.SmoothDamp(f, 1f, ref timeVelocity, .3f);
		}

		f = Mathf.Clamp(f, 0, 100);
		if(Mathf.Abs(f - 1) > .1)
		{
			player.GetComponent<Player>().debug = true;
		}
		else
			player.GetComponent<Player>().debug = player.GetComponent<Player>().wasDebug;
		Time.timeScale = f;
		song.pitch = Time.timeScale;

		 if(Input.GetKeyDown(KeyCode.Escape)){
		 	Application.Quit();
		 }
	}

	void OnGUI()
	{
		if(player.GetComponent<Player>().debug)
		{
			GUI.Label(new Rect(0, 20, 200, 100), "Song time: "+song.time);  
		}
	}
}
