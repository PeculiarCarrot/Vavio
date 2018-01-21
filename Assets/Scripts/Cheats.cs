using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour {

	public AudioClip unlockAllLevelsSound;
	private KeyCode[] unlockAllLevels;
	private int index;
	private KeyCode[] currentCode;

	void Start() {
		// Code is "idkfa", user needs to input this in the right order
		unlockAllLevels = new KeyCode[] {KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow};
		index = 0;    
	}

	void Update() {
		if (Input.anyKeyDown) {
			if (index < unlockAllLevels.Length && Input.GetKeyDown(unlockAllLevels[index]))
			{
				index++;
			}
			else {
				index = 0;    
			}
		}

		if (index == unlockAllLevels.Length && PlayerPrefs.GetInt("level") < 11) {
			PlayerPrefs.SetInt("level", 11);
			PlayerPrefs.Save();
			GetComponent<AudioSource>().PlayOneShot(unlockAllLevelsSound);
			index = 0;
		}
	}
}
