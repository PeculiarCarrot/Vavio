using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	public Text text;
	public EnemySpawner spawner;
	private bool going;

	// Use this for initialization
	public void Begin () {
		going = false;
		if(spawner.level == 0 && PlayerPrefs.GetInt("didTutorial") == 0)
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
			if (Stage.time > 30)
			{
				if(text.text != "")
					text.text = "";
			}
			else if (Stage.time > 22)
				text.text = Options.keyboardMovement ? "You can use your selected ability with Q or F." : "Right click to use your selected ability.";
			else if (Stage.time > 13)
				text.text = Options.keyboardMovement ? "You can select your ability with the 1-3 keys, ASD, or the scroll wheel." : "You can select your ability with the scroll wheel, the 1-3 keys, or the arrow keys.";
			else if (Stage.time > 5)
				text.text = "Shooting enemies gives you charge.";
			else if(Stage.time > 0)
				text.text = Options.keyboardMovement ? "Use the arrow keys to move around. Shift makes you move slower." : "Use the mouse to move around.";
		}
	}
}
