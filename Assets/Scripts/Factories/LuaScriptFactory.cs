using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

public class LuaScriptFactory {
	
	private static List<Script> usableObjects = new List<Script>();

	public static Script GetUnused()
	{
		if(usableObjects.Count > 0)
		{
			Script s = usableObjects[usableObjects.Count - 1];
			usableObjects.RemoveAt(usableObjects.Count - 1);
			return s;
		}
		return new Script();
	}

	public static void SetUsable(Script s)
	{
		s.Reset();	
		usableObjects.Add(s);
	}

	public static void ClearPool()
	{
		usableObjects.Clear();
	}
}
