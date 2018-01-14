using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAbility : Ability {

	public AudioClip shootSound;
	private float delay;

	public LaserAbility(Player player) : base(player) {}

	public override void Begin()
	{
		duration = 10;
		player.GetComponent<PatternController>().Reset();
		player.GetComponent<PatternController>().patternPath = "Player/laser";
		player.GetComponent<PatternController>().Init();
	}

	public override void End()
	{
		player.GetComponent<PatternController>().Reset();
		player.GetComponent<PatternController>().patternPath = "Player/standard";
		player.GetComponent<PatternController>().Init();
	}

	public override void DoUpdate()
	{
		delay += Time.deltaTime;
		if(delay > .1)
		{
			delay = 0;
			EnemyAudio.Play(shootSound);
		}
	}

}
