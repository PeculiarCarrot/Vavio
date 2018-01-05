using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WipeEffect : MonoBehaviour {

	float spd, startSpeed = -80f;
	private bool transitioning;
	private string sceneName;

	// Use this for initialization
	void Start () {
		spd = startSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		spd *= .97f;
		transform.Translate(spd, 0, 0);
		if(spd > 0 && spd < .5)
		{
			SceneManager.LoadScene(sceneName);
		}
	}

	public void Transition(string sceneName)
	{
		transitioning = true;
		spd = -startSpeed;
		this.sceneName = sceneName;
	}
}
