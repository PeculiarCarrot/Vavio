using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleAbility : Ability {

	private GameObject shield;

	public BubbleAbility(Player player) : base(player) {}

	public override void Begin()
	{
		duration = 4;
		GameObject prefab = Resources.Load<GameObject>("Prefabs/bubbleAbility");
		shield = GameObject.Instantiate(prefab, player.transform.position, prefab.transform.rotation);
		shield.GetComponent<BubbleAbilityObject>().SetTarget(player.transform);
	}

	public override void End()
	{
		GameObject.Destroy(shield);
	}

	public override void DoUpdate()
	{
		
	}

}
