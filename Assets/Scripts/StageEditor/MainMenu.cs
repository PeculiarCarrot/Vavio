using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public EditorController editorController;
	public GameObject newStageButton;
	public GameObject loadStageButton;
	public GameObject quitButton;
	public GameObject backButton;
	public GameObject chooseSongButton;
	public GameObject createButton;

	// Use this for initialization
	void Start () {
		createButton.SetActive(false);
		if (EditorController.editingMode == 0)
			editorController.DisableEditor();
		else
			editorController.ToMenu();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NewStage()
	{
		newStageButton.SetActive(false);
		loadStageButton.SetActive(false);
		quitButton.SetActive(false);
		backButton.SetActive(true);
		chooseSongButton.SetActive(true);
		createButton.SetActive(false);
	}

	public void Back()
	{
		newStageButton.SetActive(true);
		loadStageButton.SetActive(true);
		quitButton.SetActive(true);
		backButton.SetActive(false);
		chooseSongButton.SetActive(false);
		createButton.SetActive(false);
	}

	public void ChooseSong()
	{
		Cursor.visible = true;
		string path = StageFileManager.OpenLoadAudioDialog();
		if(path != "")
		{
			StageFileManager.audioFilePath = path;
			createButton.SetActive(true);
		}
	}

	public void StartWithNewSong()
	{
		editorController.spawner.CreateEmptySpawnData();
		LoadInLevel();
	}

	public void LoadInLevel()
	{
		editorController.ToEditor();
		editorController.LoadSong();
	}

	public void LoadStage()
	{
		string path = StageFileManager.OpenLoadStageDialog();
		if(path != "")
		{
			LoadInLevel();
		}
	}

	public void Quit()
	{
		Application.Quit();
	}
}
