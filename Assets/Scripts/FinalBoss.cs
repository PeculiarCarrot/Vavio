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
		stage = Stage.stage;
		goalPos = transform.position;
	}

	private float portalStart = 16.7f;
	bool didIntroAnim;
	bool eyeClosed = true;
	Vector3 velocity;
	float fric = .03f;

	Vector3 portalStartScale = Vector3.zero;

	int glitchIndex;

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

		if(glitchIndex == 0 && GetStageTime() >= 202.63)
		{
			glitchIndex++;
			stage.glitchEffect.Glitch(.3f, .5f);
		}
		else if(glitchIndex == 1 && GetStageTime() >= 203.09)
		{
			glitchIndex++;
			stage.glitchEffect.Glitch(-.6f, .5f);
		}
		else if(glitchIndex == 2 && GetStageTime() >= 203.59)
		{
			glitchIndex++;
			stage.glitchEffect.Glitch(1, .5f);
		}
		else if(glitchIndex == 3 && GetStageTime() >= 204.06)
		{
			glitchIndex++;
			stage.glitchEffect.Glitch(-1.3f, .5f);
		}
		else if(glitchIndex == 4 && GetStageTime() >= 204.53)
		{
			glitchIndex++;
			stage.glitchEffect.Glitch(2, .5f);
			Destroy(gameObject);
		}
	}

	void FixedUpdate()
	{
		body.MovePosition(Vector3.Lerp(transform.position, goalPos, 3 * Time.deltaTime));
	}
}
