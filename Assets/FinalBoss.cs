using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MovementController {

	public GameObject boss;
	public GameObject portal;
	public Material portalMaterial;
	private int colorIndex;
	private Rigidbody body;

	public Animator eyeAnimator;
	public AnimationClip eyeOpeningAnimation;

	float changeColorTimer, changeColorTime = .2f;

	Vector3 goalPos;

	void Start()
	{
		body = GetComponent<Rigidbody>();
		goalPos = transform.position;
	}

	private float portalStart = 17f;
	bool didIntroAnim;
	bool eyeClosed = true;
	Vector3 velocity;
	float fric = .03f;

	Vector3 portalStartScale = Vector3.zero;

	void Update()
	{
		
		if(GetStageTime() >= portalStart && portal.transform.localScale.x < 70)
		{
			portal.transform.localPosition = new Vector3(portal.transform.localPosition.x, portal.transform.localPosition.y, 60);
			if (portalStartScale == Vector3.zero)
				portalStartScale = portal.transform.localScale;
			portal.transform.localScale *= 1 + 2f * Time.deltaTime;
			portal.GetComponent<Renderer>().material = portalMaterial;
		}

		if(GetStageTime() > 32 && !didIntroAnim)
		{
			didIntroAnim = true;
			goalPos = transform.position;
			GetComponent<Collider>().enabled = true;
			goalPos.y = 9;
		}

		if(GetStageTime() > 49 && eyeClosed)
		{
			eyeClosed = false;
			eyeAnimator.Play(eyeOpeningAnimation.name);
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

	void FixedUpdate()
	{
		body.MovePosition(Vector3.Lerp(transform.position, goalPos, 3 * Time.deltaTime));
	}
}
