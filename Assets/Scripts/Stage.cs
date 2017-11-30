using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {

	public static float deltaTime;

	public GameObject player;
	public GameObject spawner;
	[HideInInspector]
	public float minX, minY, maxX, maxY, width, height;
	public AudioClip[] songs;
	private AudioSource song;
	private float lastTime;

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
		song = GetComponent<AudioSource>();
		//Debug.Log(minX+", "+minY +" - "+maxX+", "+maxY);
	}
	
	// Update is called once per frame
	void Update () {
		deltaTime = song.time - lastTime;
		lastTime = song.time;
	}
}
