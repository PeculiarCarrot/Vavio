using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;

[MoonSharpUserData]
public class MovementController : ScriptController {

	[HideInInspector]
	public Vector3 move;
	[HideInInspector]
	public float friction;
	[HideInInspector]
	public bool resetMoveOnUpdate;
	[HideInInspector]
	public bool ignoreAngle;
	[HideInInspector]
	public float speed, speedMultiplier, turn;
	[HideInInspector]
	public string targetType;
	private GameObject target;

	private LuaMath luaMath = new LuaMath();

	private bool blank = true;
	public bool synced;

	public void Reset()
	{
		if (script != null)
			CallLuaFunction("init", this, GetInstanceID());
		move = Vector3.zero;
		friction = 1f;
		resetMoveOnUpdate = true;
		ignoreAngle = false;
		speed = 1;
		speedMultiplier = 1;
		synced = true;
		targetType = null;
		target = null;
		script = null;
		patternPath = null;
		blank = true;
		turn = 0;
	}

	public MovementController(){
	}

	public void Print(string message)
	{
		Debug.Log(message);
	}

	void Start()
	{
		Init();
	}

	public float[] GetFireTimes(float bulletsPerSecond, float initialDelay, float duration)
	{
		return GetFireTimes(bulletsPerSecond, initialDelay, duration, 0, 0);
	}

	public float[] GetFireTimes(float bulletsPerSecond, float initialDelay, float duration, float secondsToFire, float secondsToPause)
	{
		float secondsPerBullet = 1 / bulletsPerSecond;
		List<float> times = new List<float>();
		float timeUntilChange = secondsToFire;
		bool paused = false;
		for(float i = initialDelay; i < initialDelay + duration; i += bulletsPerSecond)
		{
			if(secondsToPause > 0)
			{
				//Debug.Log(timeUntilChange);
				timeUntilChange -= secondsPerBullet;
				if(timeUntilChange <= 0)
				{
					paused = !paused;
					timeUntilChange = paused ? secondsToPause : secondsToFire;
				}
			}
			if(!paused)
				times.Add(i);
		}
		float[] fireTimes = new float[times.Count];
		for(int i = 0; i < fireTimes.Length; i++)
		{
			fireTimes[i] = times[i] + (float)Stage.time;
		}
		return fireTimes;
	}

	// Use this for initialization
	public void Init () {
		if (blank)
		{
			SetDefaultPath("Patterns/Movement/");
			base.DoInit();
			if (script != null)
			{
				try
				{
					CallLuaFunction("init", this, GetInstanceID());
				}
				catch (ScriptRuntimeException ex)
				{
					Debug.LogError("Whoops, there was a runtime Lua error in '" + patternPath + "'   -   " + ex.DecoratedMessage);
				}
			}
			else{
				switch(patternPath)
				{
					case "$forward":
						friction = 1f;
						resetMoveOnUpdate = false;
						move = new Vector3(speed, 0, 0);
						break;
					default:
						break;
				}
			}
		}
		blank = false;
	}

	public LuaMath Math()
	{
		return luaMath;
	}

	public float GetStageDeltaTime()
	{
		return Stage.deltaTime;
	}

	public float GetStageTime()
	{
		return (float)Stage.time;
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

	public bool TargetExists()
	{
		return target != null;
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

	public void SetPos(float x, float y)
	{
		Vector3 v = transform.position;
		v.x = x;
		v.y = y;
		transform.position = v;
	}

	public float GetX()
	{
		return transform.position.x;
	}

	public float GetY()
	{
		return transform.position.y;
	}

	public float GetAngle()
	{
		return transform.eulerAngles.z;
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

	public float GetScaleX()
	{
		return transform.localScale.x;
	}

	public float GetScaleY()
	{
		return transform.localScale.y;
	}

	public float GetScaleZ()
	{
		return transform.localScale.z;
	}

	public void Die()
	{
		if (GetComponent<Enemy>() != null)
			GetComponent<Enemy>().Die();
	}

	public void SetScaleX(float x)
	{
		Vector3 newScale = transform.localScale;
		newScale.x = x;
		transform.localScale = newScale;
	}

	public void SetScaleY(float y)
	{
		Vector3 newScale = transform.localScale;
		newScale.y = y;
		transform.localScale = newScale;
	}

	public void SetScaleZ(float z)
	{
		Vector3 newScale = transform.localScale;
		newScale.z = z;
		transform.localScale = newScale;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log("UPDATE  -  " + gameObject.GetInstanceID() + ": " + script);
		if (resetMoveOnUpdate)
			move = Vector3.zero;
		if (script != null)
		{
			try
			{
				CallLuaFunction("update", this, GetInstanceID(), Time.deltaTime);
			}
			catch (ScriptRuntimeException ex)
			{
				Debug.LogError("Whoops, there was a runtime Lua error in '" + patternPath + "'   -   " + ex.DecoratedMessage);
			}
		}
		else{
			switch(patternPath)
			{
				case "$forward":
					resetMoveOnUpdate = false;
					//move += new Vector3(speed * Time.deltaTime, 0, 0);
					break;
				default:
					break;
			}
		}
		Vector3 realMove = move;
		if(!ignoreAngle)
		{
			realMove = Quaternion.Euler(0, 0, transform.eulerAngles.z) * move;
		}

		transform.position = transform.position + realMove * Time.deltaTime;
		transform.Rotate(new Vector3(0, 0, turn * Time.deltaTime));
		move *= friction;
		move *= speedMultiplier;
	}
}
