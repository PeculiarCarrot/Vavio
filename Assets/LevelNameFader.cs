using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNameFader : MonoBehaviour {

	public Text text;
	public bool somethingSelected;
	private float goalAlpha;

	void Start () {
		
	}

	void Update () {
		if (somethingSelected)
			goalAlpha = 1;
		else
			goalAlpha = 0;

		Color c = text.color;
		c.a = Mathf.SmoothStep(text.color.a, goalAlpha, 30f * Time.deltaTime);
		text.color = c;
		
		somethingSelected = false;
	}
}
