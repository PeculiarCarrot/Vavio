using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class LevelSelectButton : MonoBehaviour {

	public LevelNameFader levelNameFader;
	public Text songName;
	public int level;
	private Button button;

	// Use this for initialization
	void Start () {
		button = GetComponent<Button>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (EventSystem.current.currentSelectedGameObject == gameObject && level <= PlayerPrefs.GetInt("level"))
		{
			songName.text = Stage.GetSongName(level);
			levelNameFader.somethingSelected = true;
		}
	}
}
