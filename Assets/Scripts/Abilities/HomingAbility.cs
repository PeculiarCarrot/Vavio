using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingAbility : Ability {

	public HomingAbility(Player player) : base(player) {}

	public override void Begin()
	{
		duration = 10;
		player.GetComponent<PatternController>().Reset();
		player.GetComponent<PatternController>().patternPath = "Player/homing";
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
		
	}

}
