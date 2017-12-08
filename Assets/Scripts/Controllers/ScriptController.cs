using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public class ScriptController : MonoBehaviour {

	protected Stage stage;
	public string patternPath;
	private string defaultPath;
	private static Dictionary<string, string> cachedFiles = new Dictionary<string, string>();

	protected Script script;

	public ScriptController(string defaultPath)
	{
		this.defaultPath = defaultPath;
	}

	private string GetCode(string filePath)
	{
		string code = "";
		if (cachedFiles.ContainsKey(filePath))
		{
			if (!cachedFiles.TryGetValue(filePath, out code))
				Debug.LogError("?????");
			return code;
		}
		code = System.IO.File.ReadAllText(filePath);
		cachedFiles.Add(filePath, code);
		return code;
	}

	public void Start()
	{
		stage = GameObject.Find("Stage").GetComponent<Stage>();
		if (patternPath == null || patternPath == "" || patternPath.StartsWith("$"))
			return;
		patternPath = defaultPath + patternPath;

		//Make sure we're getting the right path regardless of operating system
		string path = Application.streamingAssetsPath;
		string[] directories = patternPath.Split('/');
		foreach (string dir in directories)
			path = System.IO.Path.Combine(path, dir);
		path += ".lua";

		string code = GetCode(path);
		script = new Script();
		//int m = System.DateTime.Now;
		script.DoString(code);
		//Debug.Log((System.DateTime.Now.Millisecond - m));
	}

	public DynValue CallLuaFunction(string function, params object[] parameters)
	{
		object func = script.Globals[function];
		if (func == null)
		{
			Debug.Log("'" + function + "' does not exist in "+patternPath);
			return null;
		}

		return script.Call(func, parameters);
	}

}
