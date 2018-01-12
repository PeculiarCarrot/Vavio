using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour {

	public static bool smoothMovement;
	public static bool keyboardMovement;
	public static bool screenShake;
	public static float musicVolume;
	public static float sfxVolume;
	public static int antialiasing;

	public Toggle smoothToggle;
	public Toggle screenShakeToggle;
	public Toggle keyboardMovementToggle;
	public Slider musicSlider;
	public Slider sfxSlider;
	public Dropdown antialiasingChooser;
	public WipeEffect wipeEffect;

	// Use this for initialization
	void Start () {
		Load();
		SetUI();
	}

	public static void Load()
	{
		SetDefaults();
		LoadPrefs();
	}

	static void SetDefaults()
	{
		smoothMovement = true;
		screenShake = true;
		keyboardMovement = false;
		musicVolume = .3f;
		sfxVolume = 1f;
		antialiasing = 3;
	}

	public void ToMenu()
	{
		Save();
		wipeEffect.Transition("menu");
	}

	public void ToCredits()
	{
		Save();
		wipeEffect.Transition("credits");
	}

	public static void Save()
	{
		PlayerPrefs.SetInt("smoothMovement", Options.smoothMovement ? 1 : 0);
		PlayerPrefs.SetInt("keyboardMovement", Options.keyboardMovement ? 1 : 0);
		PlayerPrefs.SetInt("antialiasing", Options.antialiasing);
		PlayerPrefs.SetInt("screenShake", Options.screenShake ? 1 : 0);
		PlayerPrefs.SetFloat("musicVolume", Options.musicVolume);
		PlayerPrefs.SetFloat("sfxVolume", Options.sfxVolume);
		PlayerPrefs.Save();
	}

	static void LoadPrefs()
	{
		if (PlayerPrefs.HasKey("smoothMovement"))
			smoothMovement = PlayerPrefs.GetInt("smoothMovement") == 1;
		if (PlayerPrefs.HasKey("keyboardMovement"))
			keyboardMovement = PlayerPrefs.GetInt("keyboardMovement") == 1;
		if (PlayerPrefs.HasKey("antialiasing"))
			antialiasing = PlayerPrefs.GetInt("antialiasing");
		if (PlayerPrefs.HasKey("screenShake"))
			screenShake = PlayerPrefs.GetInt("screenShake") == 1;
		if (PlayerPrefs.HasKey("musicVolume"))
			musicVolume = PlayerPrefs.GetFloat("musicVolume");
		if (PlayerPrefs.HasKey("sfxVolume"))
			sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
	}

	void SetUI()
	{
		smoothToggle.isOn = smoothMovement;
		keyboardMovementToggle.isOn = keyboardMovement;
		screenShakeToggle.isOn = screenShake;
		musicSlider.value = musicVolume;
		sfxSlider.value = sfxVolume;
		antialiasingChooser.value = antialiasing;
	}

	public void OnMusicChange()
	{
		musicVolume = musicSlider.value;
	}

	public void OnSFXChange()
	{
		sfxVolume = sfxSlider.value;
		AudioListener.volume = sfxVolume;
	}

	public void OnAntialiasingChange()
	{
		antialiasing = antialiasingChooser.value;
		SetAntialiasing(antialiasing);
	}

	public void SetAntialiasing(int chosen)
	{
		int num = 0;
		if (chosen == 1)
			num = 2;
		else if (chosen == 2)
			num = 4;
		else if (chosen == 3)
			num = 8;
		QualitySettings.antiAliasing = num;
	}

	public void OnSmoothMovementChange()
	{
		smoothMovement = smoothToggle.isOn;
	}

	public void OnKeyboardMovementChange()
	{
		keyboardMovement = keyboardMovementToggle.isOn;
	}

	public void OnScreenShakeChange()
	{
		screenShake = screenShakeToggle.isOn;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
