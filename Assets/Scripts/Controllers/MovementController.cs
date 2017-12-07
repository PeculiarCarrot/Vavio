using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class MovementController : ScriptController {

	[HideInInspector]
	public Vector3 pos = Vector3.zero;
	[HideInInspector]
	public float friction = 1f;
	[HideInInspector]
	public bool resetPosOnUpdate = true;
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
		CallLuaFunction("init", this);
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

	public void SetPos(float x)
	{
		pos = new Vector3(x, 0, 0);
	}

	public float GetSpeed()
	{
		return speed;
	}

	public void SetPos(float x, float y)
	{
		pos = new Vector3(x, y, 0);
	}

	public float GetPosX()
	{
		return pos.x;
	}

	public float GetPosY()
	{
		return pos.y;
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
		if (resetPosOnUpdate)
			pos = Vector3.zero;
		CallLuaFunction("update", this, Stage.deltaTime);

		if(!ignoreAngle)
		{
			pos = Quaternion.Euler(0, 0, transform.eulerAngles.z) * pos;
		}

		transform.position = transform.position + pos;
		pos *= friction;
	}
}
