using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public class ScriptController : MonoBehaviour {

	protected Stage stage;
	public string patternPath;
	private string defaultPath;

	protected Script script;

	public ScriptController(string defaultPath)
	{
		this.defaultPath = defaultPath;
	}

	public void Start()
	{
		stage = GameObject.Find("Stage").GetComponent<Stage>();

		UserData.RegisterType<Vector3>();
		UserData.RegisterType<Transform>();
		UserData.RegisterType<Quaternion>();
		UserData.RegisterAssembly();
		patternPath = defaultPath + patternPath;

		//Make sure we're getting the right path regardless of operating system
		string path = Application.streamingAssetsPath;
		string[] directories = patternPath.Split('/');
		foreach (string dir in directories)
			path = System.IO.Path.Combine(path, dir);
		
		string code = System.IO.File.ReadAllText(path);
		script = new Script();
		script.DoString(code);
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
