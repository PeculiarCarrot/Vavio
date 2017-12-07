using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaMath {

	public float PI = Mathf.PI;
	public float Rad2Deg = Mathf.Rad2Deg;
	public float Deg2Rad = Mathf.Deg2Rad;
	
	public float RandomValue()
	{
		return Random.value;
	}

	public float RandomRange(float x, float y)
	{
		return Random.Range(x, y);
	}

	public float Sin(float x)
	{
		return Mathf.Sin(x);
	}

	public float Cos(float x)
	{
		return Mathf.Cos(x);
	}

	public float Tan(float x)
	{
		return Mathf.Tan(x);
	}

	public float Atan2(float x, float y)
	{
		return Mathf.Atan2(x, y);
	}

	public float LerpAngle(float start, float goal, float percent)
	{
		return Mathf.LerpAngle(start, goal, percent);
	}

}
