using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!PlayerPrefs.HasKey("level"))
		{
			PlayerPrefs.SetInt("level", 0);
			PlayerPrefs.SetInt("didTutorial", 0);
			PlayerPrefs.SetInt("chosenAbility", 0);
			PlayerPrefs.Save();
		}
	}
	
	// Update is called once per frame
	void Update () {
		Cursor.visible = true;
	}

	public void OnQuit()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
