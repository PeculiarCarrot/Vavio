using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

	public AudioSource song;
	float spd;

	// Use this for initialization
	void Start () {
		spd = Screen.height * .15f;
		Options.Load();
		song.volume = Options.musicVolume;
	}
	
	// Update is called once per frame
	void Update () {
		Cursor.visible = false;
		transform.Translate(new Vector3(0, spd * Time.deltaTime, 0));
		if (transform.position.y > Screen.height || Input.GetKeyDown(KeyCode.Escape))
			SceneManager.LoadScene("menu");
	}
}
