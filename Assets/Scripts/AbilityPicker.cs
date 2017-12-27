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
	float offset, startOffset;

	// Use this for initialization
	void Start () {
		
	}

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
		}
		return null;
	}

	void OnGUI()
	{
		if (timeSinceMoved > 2f)
			return;
		float size = 32;
		offset = Mathf.SmoothStep(startOffset, -selectedIndex * size, timeSinceMoved / .1f);
		Color old = GUI.color;
		for(int i = 0; i < abilities.Length; i++)
		{
			Color c = Color.white;
			c.a = (i - selectedIndex == 0) ? 1 :.5f;
			GUI.color = c;
			GUI.DrawTexture(new Rect(cam.WorldToScreenPoint(transform.position).x - size / 2 + i * size + offset, -cam.WorldToScreenPoint(transform.position).y + cam.scaledPixelHeight + 10, size, size), abilities[i].texture);
		}
		GUI.color = old;
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceMoved += Time.deltaTime;
		if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 || Input.GetKeyDown(KeyCode.RightArrow))
		{
			SetSelectedIndex(selectedIndex + 1);
		}
		else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			SetSelectedIndex(selectedIndex - 1);
		}

		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			SetSelectedIndex(0);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			SetSelectedIndex(1);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			SetSelectedIndex(2);
		}
		selectedIndex = Mathf.Clamp(selectedIndex, 0, abilities.Length - 1);
	}

	private void SetSelectedIndex(int index)
	{
		timeSinceMoved = 0;
		selectedIndex = index;
		startOffset = offset;
	}
}
