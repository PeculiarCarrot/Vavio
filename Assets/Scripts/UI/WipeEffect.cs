using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WipeEffect : MonoBehaviour {

	public RectTransform rectTransform;

	float spd, startSpeed = -20f;
	private bool transitioning;
	private string sceneName;
	private Vector3 goal, start;
	private Vector3 spawnPoint;
	float percent;

	public bool IsTransitioning()
	{
		return transitioning;
	}

	void Awake()
	{
		GetComponent<Image>().enabled = true;
	}

	// Use this for initialization
	void Start () {
		spd = startSpeed;
		start = transform.position;
		spawnPoint = start;
		goal = start + new Vector3(-Screen.width, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPos = transform.position;
		percent += 4 * Time.deltaTime;
		newPos = Vector3.Lerp(start, goal, percent);
		transform.position = newPos;

		if(transitioning && percent >= 1)
		{
			SceneManager.LoadScene(sceneName);
		}
		transform.SetAsLastSibling();
	}

	public void Transition(string sceneName)
	{
		transform.position = spawnPoint + new Vector3(Screen.width, 0, 0);
		start = transform.position;
		goal = spawnPoint;
		percent = 0;
		transitioning = true;
		this.sceneName = sceneName;
	}
}
