using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MovementController {

	public GameObject boss;
	public GameObject portal;
	public Material portalMaterial;
	private Rigidbody body;

	public Animator eyeAnimator;
	public AnimationClip eyeOpeningAnimation;

	Vector3 goalPos;

	//When the portal animation begins
	private float portalStart = 16.7f;
	//Whether or not the boss has done the animation where he comes down from the top yet
	bool didIntroAnim;
	bool eyeClosed = true;
	Vector3 velocity;
	float fric = .03f;

	Vector3 portalStartScale = Vector3.zero;

	//This is used for the end of the stage. Each index progressively gets more glitchy.
	int glitchIndex;

	void Start()
	{
		body = GetComponent<Rigidbody>();
		stage = Stage.stage;
		goalPos = transform.position;
	}

	void Update()
	{
		//If it's time, grow the black portal
		if(GetStageTime() >= portalStart && portal.transform.localScale.x < 70)
		{
			portal.transform.localPosition = new Vector3(portal.transform.localPosition.x, portal.transform.localPosition.y, 60);
			if (portalStartScale == Vector3.zero)
				portalStartScale = portal.transform.localScale;
			portal.transform.localScale *= 1 + 2f * Time.deltaTime;
			portal.GetComponent<Renderer>().material = portalMaterial;
		}

		//If it's time and we haven't done the intro animation yet, start it
		if(GetStageTime() > 32 && !didIntroAnim)
		{
			didIntroAnim = true;
			goalPos = transform.position;
			GetComponent<Collider>().enabled = true;
			goalPos.y = 9;
		}

		//Play the eye-opening animation
		if(GetStageTime() > 49 && eyeClosed)
		{
			eyeClosed = false;
			eyeAnimator.Play(eyeOpeningAnimation.name);
		}

		//Progressively get glitchier, and glitch to the beat
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
		//Always be moving torward our goal position (used for intro anim)
		body.MovePosition(Vector3.Lerp(transform.position, goalPos, 3 * Time.deltaTime));
	}
}
