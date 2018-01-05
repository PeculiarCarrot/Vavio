using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

	public WipeEffect wipeEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnContinue()
	{
		Stage.paused = false;
	}

	public void OnMenu()
	{
		wipeEffect.Transition("menu");
	}
}
