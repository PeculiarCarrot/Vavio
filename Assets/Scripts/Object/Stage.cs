﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Stage : MonoBehaviour {

	//Song delta time
	public static float deltaTime;
	//Time tracking
	public static double lastTime, timeOffset, time;
	private static double beginTime;
	public static float songProgress;

	public GameObject player;
	public GameObject spawner;
	[HideInInspector]
	public static float minX, minY, maxX, maxY, width, height;
	public static Stage stage;
	public string[] songs;
	public AudioSource song;

	public static bool loadingSong;

	public static bool paused;

	private bool hasFocus = true;
	private float prePauseTimeScale;

	//The living bullets and entities right now
	private static List<GameObject> bullets = new List<GameObject>();
	private static List<GameObject> enemies = new List<GameObject>();

	public GameObject darken;
	public WipeEffect wipeEffect;
	private bool wasPaused;

	void Awake()
	{
		songProgress = 0;
		Options.Load();
		loadingSong = false;
		bullets.Clear();
		enemies.Clear();
		paused = false;
		PatternController.Load();
		EnemySpawner.Load();
	}

	void Start () {
		UpdateScreenPositions();
		deltaTime = 0;
		time = 0;
		lastTime = 0;
		stage = this;
		song.volume = Options.musicVolume;
	}

	public void LoadLevel(int level)
	{
		loadingSong = true;
		StartCoroutine(LoadTrack(Path.Combine(Application.streamingAssetsPath, songs[level])));
	}

	IEnumerator LoadTrack(string filename)
	{
		var www = new WWW("file://"+filename);

		while(www.progress < 1)
		{
			yield return new WaitForSeconds(0.1f);
		}
		var clip = www.GetAudioClip(false, true);

		song.clip = clip;
		loadingSong = false;
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
		timeOffset = 0;
	}

	void FixedUpdate()
	{
		time = song.time - beginTime;
		deltaTime = (float)(time - lastTime);

		if (!hasFocus)
		{
			deltaTime = 0;
			Time.timeScale = 0;
		}
		lastTime = time;
	}
		
	void OnApplicationFocus(bool hasFocus)
	{
		this.hasFocus = hasFocus;
		//Time.timeScale = prePauseTimeScale;
	}

	void OnApplicationPause(bool hasFocus)
	{
		this.hasFocus = hasFocus;
		prePauseTimeScale = Time.timeScale;
	}

	private float timeVelocity;
	
	void Update () {
		UpdateScreenPositions();

		float f = Time.timeScale;
		if(Application.isEditor && Input.GetKeyDown("space"))
		{
			Application.targetFrameRate = Application.targetFrameRate == -1 ? 30 : -1;
		}
		if (!player.GetComponent<Player>().IsDying())
		{
			if(Application.isEditor && !player.GetComponent<Player>().IsUsingAbility())
			{
				if (Input.GetKey("right") && Input.GetKey(KeyCode.LeftControl))
				{
					f = 99;
				}
				else if (Input.GetKey("left") && Input.GetKey(KeyCode.LeftControl))
				{
					f -= .01f;
				}
				else
				{
					f = Mathf.Max(f, .2f);
					f = Mathf.SmoothDamp(f, 1f, ref timeVelocity, .3f);
				}

				f = Mathf.Clamp(f, 0, 99);
				if (Mathf.Abs(f - 1) > .1)
				{
					player.GetComponent<Player>().debug = true;
				}
				else
					player.GetComponent<Player>().debug = player.GetComponent<Player>().wasDebug;
			}
		}
		else{
			if (song.isPlaying)
				song.Pause();
		}
		if (float.IsNaN(f))
			f = 1;
		Time.timeScale = hasFocus ? f : 0;
		Time.fixedDeltaTime = (1 / 50f) * f;
		song.pitch = Time.timeScale;
		Debug.Log(song.pitch);

		 if(Input.GetKeyDown(KeyCode.Escape)){
			paused = !paused;
		}

		if (paused)
		{
			if(!wasPaused)
			{
				if (song.isPlaying)
					song.Pause();
				darken.SetActive(true);
			}
			Time.timeScale = wipeEffect.IsTransitioning() ? 1 : 0;
			Cursor.visible = true;
		}
		else
		{
			if(wasPaused)
			{
				darken.SetActive(false);
				if (!song.isPlaying)
					song.UnPause(); 
				Time.timeScale = 1;
			}
			Cursor.visible = false;
		}
		wasPaused = paused;
		if(song != null && song.clip != null)
			songProgress = song.time / song.clip.length;
	}

	void OnGUI()
	{
		if(player.GetComponent<Player>().debug)
		{
			GUI.Label(new Rect(0, 20, 200, 100), "Song time: "+song.time);  
		}
	}
}
