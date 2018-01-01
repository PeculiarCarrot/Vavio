using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortalsBoss : MovementController {

	public GameObject boss;
	public GameObject portal;
	public Material portalMaterial;
	public Material[] colors;
	private int colorIndex;

	float changeColorTimer, changeColorTime = .2f;

	void Start()
	{

	}

	private float portalStart = 113.3f, portalEnd = 117.6f;

	Vector3 portalStartScale = Vector3.zero, portalGoalScale = new Vector3(3, 3, 3);

	void Update()
	{
		if(GetStageTime() > 91.8)
		{
			changeColorTimer += Time.deltaTime;
			if(changeColorTimer >= changeColorTime)
			{
				changeColorTimer -= changeColorTime;
				colorIndex++;
				if (colorIndex > colors.Length - 1)
					colorIndex = 0;
				boss.GetComponent<Renderer>().material = colors[colorIndex];
			}
			if (GetStageTime() < 96.4)
				transform.localScale *= 1 + .17f * Time.deltaTime;
			transform.Rotate(0, 0, -60 * Time.deltaTime);

			if(GetStageTime() > portalStart && portal.transform.localScale.x < 70)
			{
				portal.transform.localPosition = new Vector3(0, 0, 60);
				if (portalStartScale == Vector3.zero)
					portalStartScale = portal.transform.localScale;
				portal.transform.localScale *= 1 + .99f * Time.deltaTime;
				portal.GetComponent<Renderer>().material = portalMaterial;
			}
		}
		if(GetStageTime() > 219.4 && GetStageTime() < 221)
		{
			Vector3 newPos = transform.position;
			newPos.y += 10 * Stage.deltaTime;
			transform.position = newPos;
		}
		if(GetStageTime() > 219.0 && portalStartScale.x >= 0)
		{
			portal.transform.localScale = portal.transform.localScale * .9f;
		}
	}
}
