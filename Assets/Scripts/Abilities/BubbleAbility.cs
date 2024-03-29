﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleAbility : Ability {

	private GameObject shield;
	private BubbleAbilityObject bubble;

	public BubbleAbility(Player player) : base(player) {}

	public override void Begin()
	{
		duration = 5;
		GameObject prefab = Resources.Load<GameObject>("Prefabs/bubbleAbility");
		shield = GameObject.Instantiate(prefab, player.transform.position, prefab.transform.rotation);
		bubble = shield.GetComponent<BubbleAbilityObject>();
		bubble.SetTarget(player.transform);
	}

	public override void End()
	{
		GameObject.Destroy(shield);
	}

	public override void DoUpdate()
	{
		if (timeAlive > duration - .5f)
			bubble.goalRad = 0;
	}

}
