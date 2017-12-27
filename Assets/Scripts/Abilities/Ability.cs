using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability {

	protected float timeAlive, duration;
	protected Player player;

	public Ability(Player player)
	{
		this.player = player;
	}

	public bool IsFinished()
	{
		return timeAlive >= duration;
	}

	public void MyUpdate()
	{
		timeAlive += Time.deltaTime;
		DoUpdate();

		if (IsFinished())
			End();
	}

	public abstract void Begin();
	public abstract void End();
	public abstract void DoUpdate();

}
