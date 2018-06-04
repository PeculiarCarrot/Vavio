using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityPicker : MonoBehaviour {

	public Camera cam;

	[System.Serializable]
	public struct TextureAbility
	{
		public string ability;
		public Texture texture;
	}

	public TextureAbility[] abilities;
	public int selectedIndex;
	float timeSinceMoved = 99f;

	//The pixel offset of the ability icons from the player
	float offset, startOffset;
	
	void Start () {
		SetSelectedIndex(PlayerPrefs.GetInt("chosenAbility"));
	}

	//Creates a new ability object based on the selected index
	public Ability GetNewAbility(Player player)
	{
		switch(abilities[selectedIndex].ability)
		{
			case "bubble":
				return new BubbleAbility(player);
			case "homing":
				return new HomingAbility(player);
			case "laser":
				return new LaserAbility(player);
			case "time":
				return new TimeAbility(player);
		}
		return null;
	}

	//Render the ability icons
	void OnGUI()
	{
		if (timeSinceMoved > 2f || Stage.paused)
			return;
		float size = 24;
		float dist = 15;
		offset = Mathf.SmoothStep(startOffset, -selectedIndex * (size + dist), timeSinceMoved / .1f);
		Color old = GUI.color;
		for(int i = 0; i < abilities.Length; i++)
		{
			Color c = Color.white;
			c.a = (i - selectedIndex == 0) ? 1 :.5f;
			GUI.color = c;
			GUI.DrawTexture(new Rect(cam.WorldToScreenPoint(transform.position).x - size / 2 + i * (size + dist) + offset, -cam.WorldToScreenPoint(transform.position).y + cam.scaledPixelHeight + 20, size, size), abilities[i].texture);
		}
		GUI.color = old;
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceMoved += Time.deltaTime;

		//Listen for inputs that can switch abilities
		if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 || (Input.GetKeyDown(KeyCode.RightArrow) && !Options.keyboardMovement))
		{
			SetSelectedIndex(selectedIndex + 1);
		}
		else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 || (Input.GetKeyDown(KeyCode.LeftArrow) && !Options.keyboardMovement))
		{
			SetSelectedIndex(selectedIndex - 1);
		}

		if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.A))
		{
			SetSelectedIndex(0);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.S))
		{
			SetSelectedIndex(1);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.D))
		{
			SetSelectedIndex(2);
		}

		//Clamp our selection only to possible indexes
		selectedIndex = Mathf.Clamp(selectedIndex, 0, abilities.Length - 1);
	}

	private void SetSelectedIndex(int index)
	{
		timeSinceMoved = 0;
		selectedIndex = index;
		startOffset = offset;
	}
}
