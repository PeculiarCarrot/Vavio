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
	public float minX, minY, maxX, maxY, width, height;
	public AudioClip[] songs;
	public AudioSource song;

	// Use this for initialization
	void Start () {
		Vector3 max = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
		Vector3 min = Camera.main.ScreenToWorldPoint(Vector3.zero);
		maxX = max.x;
		maxY = max.y;
		minX = min.x;
		minY = min.y;
		width = maxX - minX;
		height = maxY - minY;
		deltaTime = 0;
		time = 0;
		lastTime = 0;
		//Debug.Log(minX+", "+minY +" - "+maxX+", "+maxY);
	}

	public void Begin()
	{
		beginTime = AudioSettings.dspTime - song.timeSamples/song.clip.frequency;
		time = AudioSettings.dspTime - beginTime;
		lastTime = time;
		deltaTime = 0;
		timeOffset = 0;//AudioSettings.dspTime - song.timeSamples/song.clip.frequency;

	}

	void FixedUpdate()
	{
		time = AudioSettings.dspTime - beginTime;
		deltaTime = (float)(time - lastTime);
		lastTime = time;
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log("S: "+deltaTime);
		//Debug.Log("T: "+Time.deltaTime);
		 if(Input.GetKeyDown(KeyCode.Escape)){
		 	Application.Quit();
		 }
	}
}
