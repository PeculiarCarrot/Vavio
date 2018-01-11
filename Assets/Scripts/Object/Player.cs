using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class Player : Ship {

	public AudioClip hit, die;

	public EnemySpawner spawner;
	public GameObject mesh;
	public GameObject core;
	private float invincibilityDuration;
	private float flickerTimer, flickerDuration = .05f;
	public bool debug;
	[HideInInspector]
	public bool wasDebug;
	private static Texture livesTexture, progressTexture;
	public Image chargeImage;
	private bool regenerating;

	private float dieTimer, dieTime = .8f;
	private bool dying;
	//The death effect prefab for regular enemies
	private static GameObject deathEffect;

	private Rigidbody body;
	private float charge;
	private float maxCharge = 50;

	private Ability currentAbility;
	public AbilityPicker abilityPicker;

	public AudioClip charged;

	public Color notChargedColor, chargedColor;

	private Vector2 velocity;

	// Use this for initialization
	public override void DoStart () {
		wasDebug = debug;
		livesTexture = Resources.Load<Texture>("Materials/health");
		progressTexture = Resources.Load<Texture>("Materials/progress");
		body = GetComponent<Rigidbody>();
		dieTimer = 99999999;
		Time.timeScale = 1;
		if(deathEffect == null)
			deathEffect = Resources.Load<GameObject>("Prefabs/Effects/deathEffect");
	}

	public void AddCharge(float amount)
	{
		bool wasFull = charge >= maxCharge;
		charge += amount;
		if (!wasFull && charge >= maxCharge)
			PlayChargeNotification();
	}

	void FixedUpdate()
	{
		if(Options.keyboardMovement)
		{
			Vector2 target = transform.position;
			target.x += velocity.x * Time.deltaTime;
			target.y += velocity.y * Time.deltaTime;
			body.MovePosition(target);
		}
	}

	private void PlayChargeNotification()
	{
		GetComponent<AudioSource>().PlayOneShot(charged);
	}

	public bool IsUsingAbility()
	{
		return currentAbility != null;
	}

	public bool IsDying()
	{
		return dying;
	}

    void OnGUI()
    {

		chargeImage.color = charge >= maxCharge ? chargedColor : notChargedColor;
    	float size = 24;
    	for(int i = 0; i < hp; i++)
    	{
			GUI.DrawTexture(new Rect(30 + (size + 30) * i, Screen.height - 70, size, size), livesTexture);
		}
		charge = Mathf.Clamp(charge, 0, maxCharge);
		chargeImage.fillAmount = (charge / maxCharge);

		float w, h;
		float pad = 50;
		w = Screen.width - pad * 2;
		h = 5;
		GUI.DrawTexture(new Rect(pad, Screen.height - 20, w, h), progressTexture);
		GUI.DrawTexture(new Rect(pad + w * Stage.songProgress, Screen.height - 20 - h * 1.5f, 7, h * 4), progressTexture);

		if(debug)
			GUI.Label(new Rect(0, 0, 100, 100), ""+(int)(1.0f / (Time.smoothDeltaTime/Time.timeScale)));    
    }

	public void GetHurt()
	{
		if(!debug)
		{
			hp -= 1;
			if(hp > 0)
			{
				GetComponent<AudioSource>().PlayOneShot(hit);
				CameraShake.Shake(.2f, .15f);
			}
			invincibilityDuration = 2f;
			flickerTimer = flickerDuration;
		}
	}


	public new void Die()
	{
		CameraShake.Shake(.15f, .05f, 1f);
		dieTimer = dieTime;
		Time.timeScale = .2f;
		dying = true;
		GetComponent<AudioSource>().PlayOneShot(die);
		GetComponent<PatternController>().enabled = false;
		GameObject e = Instantiate(deathEffect, transform.position + new Vector3(0, 0, -3), deathEffect.transform.rotation);
		e.GetComponent<LineRenderer>().material = GetComponentInChildren<MeshRenderer>().material;
		GetComponentInChildren<MeshRenderer>().enabled = false;
		GetComponentInChildren<Collider>().enabled = false;
		//transform.position = newPos;
		Debug.Log("FAIL SONG " + spawner.level + " at: " + Mathf.RoundToInt(Stage.stage.song.time));
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Song " + spawner.level, Mathf.RoundToInt(Stage.stage.song.time));
	}

	private void Restart()
	{
		PlayerPrefs.SetInt("levelToStart", spawner.level);
		PlayerPrefs.SetInt("reasonForLevelChange", EnemySpawner.DEATH);
		PlayerPrefs.Save();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public bool IsInvincible()
	{
		return invincibilityDuration > 0 || currentAbility is BubbleAbility;
	}

	public void Regenerate()
	{
		regenerating = true;
	}

	public void UseAbility()
	{
		charge = 0;
		if (currentAbility != null)
			currentAbility.End();
		currentAbility = abilityPicker.GetNewAbility(this);
		currentAbility.Begin();
	}

	private float accel = 10f, slowAccel = 3f;
	// Update is called once per frame
	public void Update () {
		dieTimer -= Time.deltaTime;
		if(dieTimer <= 0)
		{
			dieTimer = 9999999;
			Restart();
		}
		DoUpdate();

		if (currentAbility != null)
		{
			currentAbility.MyUpdate();
			if (currentAbility.IsFinished())
				currentAbility = null;
		}

		if(!dying)
		{
			if(invincibilityDuration > 0)
			{
				invincibilityDuration -= Time.deltaTime;
				flickerTimer -= Time.deltaTime;
				if(flickerTimer < - flickerDuration)
					flickerTimer = flickerDuration;
				mesh.GetComponent<Renderer>().enabled = invincibilityDuration <= 0 || flickerTimer < 0;
			}

			if(Options.keyboardMovement)
			{
				Vector3 target = transform.position;

				velocity = Vector2.zero;
				Vector2 input = Vector2.zero;
				if (Input.GetKey(KeyCode.LeftArrow))
					input.x -= 1;
				if (Input.GetKey(KeyCode.RightArrow))
					input.x += 1;
				if (Input.GetKey(KeyCode.UpArrow))
					input.y += 1;
				if (Input.GetKey(KeyCode.DownArrow))
					input.y -= 1;
				input.Normalize();
				velocity = input * (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? slowAccel : accel);
			}
			else{
				Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				target.z = transform.position.z;
				body.MovePosition(Vector3.Lerp(transform.position, target, Options.smoothMovement ? .3f : 1f));
			}
	       /* Vector3 newRot = transform.rotation.eulerAngles;
	        newRot.y -= rotAccel * (target.x - transform.position.x);
	        if(newRot.y < 0)
	        	newRot.y += 360;
	        if(newRot.y < 180)
	        	newRot.y = Mathf.Clamp(newRot.y, 0, 45);
	        else
	        	newRot.y = Mathf.Clamp(newRot.y, 315, 360);
	        newRot.y = Mathf.SmoothDampAngle(newRot.y, 0, ref rotSpeed, .3f);
	        transform.eulerAngles = newRot;*/
			
	        if(regenerating)
	        {
	        	hp += 3f * Time.deltaTime;
	        	if(hp >= maxHP)
	        	{
	        		hp = maxHP;
	        		regenerating = false;
	        	}
	        }

			if (Input.GetKeyDown(KeyCode.Mouse1) && (charge >= maxCharge || Application.isEditor))
				UseAbility();

			transform.position = new Vector3(Mathf.Clamp(transform.position.x, Stage.minX, Stage.maxX),
				Mathf.Clamp(transform.position.y, Stage.minY, Stage.maxY), transform.position.z);
			if(hp <= 0)
				Die();
		}
	}
}
