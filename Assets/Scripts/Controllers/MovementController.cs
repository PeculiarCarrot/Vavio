using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class MovementController : ScriptController {

	[HideInInspector]
	public Vector3 move = Vector3.zero;
	[HideInInspector]
	public float friction = 1f;
	[HideInInspector]
	public bool resetMoveOnUpdate = true;
	[HideInInspector]
	public bool ignoreAngle = false;
	[HideInInspector]
	public float speed = 1;
	[HideInInspector]
	public string targetType;
	private GameObject target;

	public MovementController() : base("Patterns/Movement/"){}

	// Use this for initialization
	new void Start () {
		base.Start();
		try{
			CallLuaFunction("init", this);
		}
		catch (ScriptRuntimeException ex)
		{
			Debug.LogError("Whoops, there was a runtime Lua error in '" + patternPath + "'   -   "+ex.DecoratedMessage);
		}
	}

	public void FindTarget()
	{
		if(targetType == "player")
		{
			target = stage.player;
		}
		else if(targetType == "nearestEnemy")
		{
			target = GetNearestEnemy();
		}
	}

	private GameObject GetNearestEnemy()
	{
		List<GameObject> enemies = stage.spawner.GetComponent<EnemySpawner>().GetLiveEnemies();
		float closestDistance = 0;
		GameObject closest = null;
		foreach(GameObject e in enemies)
		{
			if(e != null/* && !e.GetComponent<Enemy>().invul*/)
			{
				float dist = Vector3.Distance(e.transform.position, transform.position);
				if(dist < closestDistance || closest == null)
				{
					closest = e;
					closestDistance = dist;
				}
			}
		}
		return closest;
	}

	public float GetTargetX()
	{
		if(target == null)
		{
			Debug.LogError("Target is null but its X value was requested: " + patternPath);
			return 0;
		}
		return target.transform.position.x;
	}

	public float GetTargetY()
	{
		if(target == null)
		{
			Debug.LogError("Target is null but its Y value was requested: " + patternPath);
			return 0;
		}
		return target.transform.position.y;
	}

	public float GetX()
	{
		return transform.position.x;
	}

	public float GetY()
	{
		return transform.position.y;
	}

	public void SetMove(float x)
	{
		move = new Vector3(x, 0, 0);
	}

	public void SetMove(float x, float y)
	{
		move = new Vector3(x, y, 0);
	}

	public float GetSpeed()
	{
		return speed;
	}

	public float GetMoveX()
	{
		return move.x;
	}

	public float GetMoveY()
	{
		return move.y;
	}

	public void Rotate(float angle)
	{
		transform.Rotate(new Vector3(0, 0, angle));
	}

	public void SetRotation(float angle)
	{
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle);
	}
	
	// Update is called once per frame
	void Update () {
		if (resetMoveOnUpdate)
			move = Vector3.zero;
		
		try{
			CallLuaFunction("update", this, Stage.deltaTime);
		}
		catch (ScriptRuntimeException ex)
		{
			Debug.LogError("Whoops, there was a runtime Lua error in '" + patternPath + "'   -   "+ex.DecoratedMessage);
		}

		if(!ignoreAngle)
		{
			move = Quaternion.Euler(0, 0, transform.eulerAngles.z) * move;
		}

		transform.position = transform.position + move;
		move *= friction;
	}
}
