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
	private PowerUp.PowerUpType currentPowerUp;
	private float remainingPowerUpDuration;
	private float invincibilityDuration;
	private float flickerTimer, flickerDuration = .05f;
	public bool debug;
	[HideInInspector]
	public bool wasDebug;
	private static Texture livesTexture, progressTexture;
	public Image chargeImage;
	private bool regenerating;

	private float dieTimer, dieTime = 4f;
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

	void OnTriggerEnter (Collider col)
    {
    	PowerUp powerUp = col.gameObject.GetComponent<PowerUp>();
        if(powerUp != null)
        {
            powerUp.Die();
            ApplyPowerUp(powerUp.type, powerUp.GetDuration());
            return;
        }
    }

    void OnGUI()
    {

		chargeImage.color = charge >= maxCharge ? chargedColor : notChargedColor;
    	float size = 24;
    	for(int i = 0; i < hp; i++)
    	{
			GUI.DrawTexture(new Rect(30 + (size + 30) * i, Screen.height - 56, size, size), livesTexture);
		}
		charge = Mathf.Clamp(charge, 0, maxCharge);
		chargeImage.fillAmount = (charge / maxCharge);

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
		Vector3 newPos = new Vector3(999, 999, 999);
		transform.position = newPos;
		Debug.Log("FAIL SONG " + spawner.level + " at: " + Mathf.RoundToInt(Stage.stage.song.time));
		GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Song " + spawner.level, Mathf.RoundToInt(Stage.stage.song.time));
	}

	private void Restart()
	{
		PlayerPrefs.SetInt("diedOnLevel", spawner.level);
		PlayerPrefs.SetInt("reasonForLevelChange", EnemySpawner.DEATH);
		PlayerPrefs.Save();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	
    private void ApplyPowerUp(PowerUp.PowerUpType type, float duration)
    {
    	currentPowerUp = type;
    	remainingPowerUpDuration = duration;
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

	// Update is called once per frame
	public void Update () {
		dieTimer -= 1 / 60f;
		if(dieTimer <= 0)
		{
			dieTimer = 9999999;
			Restart();
		}
		DoUpdate();
		if(currentPowerUp != PowerUp.PowerUpType.None)
		{
			remainingPowerUpDuration -= Time.deltaTime;
			if(remainingPowerUpDuration <= 0)
			{
				remainingPowerUpDuration = 0;
				ApplyPowerUp(PowerUp.PowerUpType.None, 0);
			}
		}

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
			Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	        target.z = transform.position.z;
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
			body.MovePosition(Vector3.Lerp(transform.position, target, Options.smoothMovement ? .3f : 1f));
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
