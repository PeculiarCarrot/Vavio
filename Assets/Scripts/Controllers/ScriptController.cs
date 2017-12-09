using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public class ScriptController : MonoBehaviour {

	protected Stage stage;
	public string patternPath;
	private string defaultPath;
	private static Dictionary<string, Script> cachedFiles = new Dictionary<string, Script>();

	protected Script script;

	private Script GetScript(string filePath)
	{
		Script script = null;
		if (cachedFiles.ContainsKey(filePath))
		{
			if (!cachedFiles.TryGetValue(filePath, out script))
				Debug.LogError("?????");
			return script;
		}
		script = new Script();
		try{
			script.DoString(System.IO.File.ReadAllText(filePath));
		}
		catch (SyntaxErrorException ex)
		{
			Debug.LogError("Whoops, there was a syntax Lua error in '" + filePath + "'   -   " + ex.DecoratedMessage);
		}
		cachedFiles.Add(filePath, script);
		return script;
	}

	protected void SetDefaultPath(string dp)
	{
		defaultPath = dp;
	}

	void OnDisable() {
		if (script != null)
		{
			//LuaScriptFactory.SetUsable(script);
		}
	}

	public void DoInit()
	{
		stage = GameObject.Find("Stage").GetComponent<Stage>();
		if (patternPath == null || patternPath == "" || patternPath.StartsWith("$"))
		{
			return;
		}
		patternPath = defaultPath + patternPath;

		//Make sure we're getting the right path regardless of operating system
		string path = Application.streamingAssetsPath;
		string[] directories = patternPath.Split('/');
		foreach (string dir in directories)
			path = System.IO.Path.Combine(path, dir);
		path += ".lua";
		script = GetScript(path);

		//Debug.Log((System.DateTime.Now.Millisecond - m));
	}

	public DynValue CallLuaFunction(string function, params object[] parameters)
	{
		object func = script.Globals[function];
		if (func == null)
		{
			Debug.LogError("'" + function + "' does not exist in "+patternPath);
			return null;
		}

		return script.Call(func, parameters);
	}

}
