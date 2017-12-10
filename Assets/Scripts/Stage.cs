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
	public AudioClip[] songs;
	public AudioSource song;

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
		//Debug.Log(minX+", "+minY +" - "+maxX+", "+maxY);
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

	public void Begin()
	{
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

	// Update is called once per frame
	void Update () {
		UpdateScreenPositions();
		//Debug.Log("S: "+deltaTime);
		//Debug.Log("T: "+Time.deltaTime);
		float f = Time.timeScale;
		if(Input.GetKey("right"))
		{
			f *= 1.01f;

			Debug.Log("Song time: " + song.time);
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
		if(f != 1)
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
}
