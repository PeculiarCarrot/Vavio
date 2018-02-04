using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAbility : Ability {

	public TimeAbility(Player player) : base(player) {}

	public override void Begin()
	{
		SetTimeScale(.5f);
		duration = 9 * Time.timeScale;
	}

	public override void End()
	{
		SetTimeScale(1f);
	}

	private void SetTimeScale(float f)
	{
		Time.timeScale = f;
		Time.fixedDeltaTime = (1 / 60f) * Time.timeScale;
	}

	public override void DoUpdate()
	{
		//If we are reaching the end of the ability, slowly speed time back up to normal
		if(timeAlive > duration - Time.timeScale)
		{
			SetTimeScale(Mathf.Lerp(Time.timeScale, 1, .01f));
		}
	}

}
