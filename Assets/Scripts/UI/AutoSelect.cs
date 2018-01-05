using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoSelect : MonoBehaviour {

	public EventSystem eventSystem;
	public GameObject toSelect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(eventSystem.currentSelectedGameObject == null)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
				eventSystem.SetSelectedGameObject(toSelect);
		}
	}
}
