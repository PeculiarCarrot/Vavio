using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;

public class StageFileManager {

	private static StageData stageData;
	public static string audioFilePath;

	public static void NewStageData()
	{

	}

	public static void LoadStageData(string path)
	{

	}

	public static void SaveStageData(string path)
	{

	}

	public static string OpenLoadAudioDialog()
	{
		var extensions = new [] {
			new ExtensionFilter("Sound Files", "mp3", "wav", "ogg")
		};
		string[] paths = StandaloneFileBrowser.OpenFilePanel("Choose background song", "", extensions, false);

		return paths.Length > 0 ? paths[0] : "";
	}

	public static string OpenLoadStageDialog()
	{
		var extensions = new [] {
			new ExtensionFilter("Vavio Stage File ", "stage" )
		};
		string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);

		return paths.Length > 0 ? paths[0] : "";
	}

    public static IEnumerator LoadStageSong(string path, AudioSource source)
	{
		Stage.loadingSong = true;
		EditorController.Loaded = false;
		WWW www = new WWW ("file://" + path);
		while(!www.isDone){
			yield return 0;
		}
		source.clip = NAudioPlayer.FromMp3Data(www.bytes);
		Debug.Log("Loaded "+path);
		EditorController.Loaded = true;
    }

}