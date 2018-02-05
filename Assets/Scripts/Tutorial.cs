using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	public Text text;
	public EnemySpawner spawner;
	//Whether or not the tutorial is on
	private bool going;

	// Use this for initialization
	public void Begin () {
		going = false;
		if(spawner.level == -1)
		{
			PlayerPrefs.SetInt("didTutorial", 1);
			PlayerPrefs.Save();
			going = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (going)
		{
			if (Stage.time > 35)
			{
				if(text.text != "")
					text.text = "";
			}
			else if (Stage.time > 22.43)
				text.text = Options.keyboardMovement ? "You can use your selected ability with Z or F when your charge meter is full." : "Right click to use your selected ability when your charge meter is full.";
			else if (Stage.time > 16.03)
				text.text = Options.keyboardMovement ? "You can select your ability with the 1-3 keys, ASD, or the scroll wheel." : "You can select your ability with the scroll wheel, the 1-3 keys, or the arrow keys.";
			else if (Stage.time > 6.423)
				text.text = "Shooting enemies gives you charge.";
			else if(Stage.time > 0)
				text.text = Options.keyboardMovement ? "Use the arrow keys to move around. Shift makes you move slower." : "Use the mouse to move around.";
		}
	}
}
