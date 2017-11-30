using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {

	public GameObject player;
	public GameObject spawner;
	[HideInInspector]
	public float minX, minY, maxX, maxY, width, height;

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
		//Debug.Log(minX+", "+minY +" - "+maxX+", "+maxY);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
