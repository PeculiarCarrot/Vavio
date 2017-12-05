using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviorController : MonoBehaviour {

	public BulletBehavior behavior;
	public float x, y;
	[HideInInspector]
	public ShooterBase entity;
	Vector3 rotatedPos;

	// Use this for initialization
	public void Start () {
		behavior.SetController(this);
		behavior.LoadFromFile();
	}

	void OnValidate()
	{
		if(behavior != null && behavior.bulletPrefab != null && behavior.bulletPrefab.GetComponent<BulletBase>() == null)
		{
			behavior.bulletPrefab = null;
			Debug.LogError("The bullet prefab must have a Bullet component attached!");
		}
	}

	public void SetPos(float x, float y)
	{
		this.x = x;
		this.y = y;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateRotatedPos();
		behavior.Update();
	}

	private void UpdateRotatedPos()
	{
		rotatedPos = new Vector3(x, y, 0);
		float r = -gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
		rotatedPos = new Vector3(rotatedPos.x * Mathf.Cos(r) - rotatedPos.y * Mathf.Sin(r), rotatedPos.x * Mathf.Sin(r) + rotatedPos.y * Mathf.Cos(r), 0);
		rotatedPos = Vector3.Scale(rotatedPos, gameObject.transform.localScale);
	}

	void OnDrawGizmosSelected() {
		UpdateRotatedPos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3( gameObject.transform.position.x + rotatedPos.x,  gameObject.transform.position.y + rotatedPos.y, gameObject.transform.position.z + rotatedPos.z), .1f);
    }

	[System.Serializable]
	public class BulletBehavior {

		public TextAsset behavior;
		public GameObject bulletPrefab;

		[HideInInspector]
		public int numberOfSets;
		[HideInInspector]
		public bool reverseSpin, synced;
		[HideInInspector]
		public float angle,
		spread, spreadMin, spreadMax,
		secondsPerSpreadPulse,
		bulletsPerSecond, bulletSpeed, bulletSpeedMultiplier, bulletLifetime,
		spinSpeed, maxSpinSpeed, spinAcceleration, reverseSpinSpeed,
		secondsToFire, secondsToPause, initialDelay;

		private BulletBehaviorController controller;

		private float currentSpinSpeed;
		private bool spinningClockwise;

		//Changing values
		private float currentAngle;
		private float currentSpread, goalSpread, spreadSpeed;
		private float anglePerSet, setOffset;

		//Constant timer values
		private float secondsPerBullet;

		//Timers
		private float bulletFireTimer;
		private float initialDelayTimer;
		private float pauseTimer;

		private float[] fireTimes;
		private int fireIndex;

		public void SetController(BulletBehaviorController controller)
		{
			this.controller = controller;
		}

		public void RestoreDefaults()
		{
			numberOfSets = 1;
			reverseSpin = false;
			angle = 0;
			spread = 0;
			spreadMin = 0;
			spreadMax = 0;
			secondsPerSpreadPulse = 0;
			bulletsPerSecond = 3;
			bulletSpeed = 0;
			bulletSpeedMultiplier = 1;
			bulletLifetime = 30;
			spinSpeed = 0;
			maxSpinSpeed = 0;
			spinAcceleration = 0;
			reverseSpinSpeed = .95f;
			secondsToFire = 3;
			secondsToPause = 0;
			initialDelay = 0;
			fireTimes = null;
			synced = true;
		}

		public void LoadFromFile()
		{
			RestoreDefaults();
			JsonUtility.FromJsonOverwrite(behavior.text, this);
			currentAngle = angle;
			currentSpread = spread;
			currentSpinSpeed = spinSpeed;
			secondsPerBullet = (float)(1 / (double)bulletsPerSecond);
			bulletFireTimer = secondsPerBullet;
			goalSpread = (spreadMin == 0 && spreadMax == 0) ? spread : spreadMax;
			initialDelay += 1f;
			pauseTimer = 0;
			initialDelayTimer = 0;
			fireTimes = null;
		}

		private void GetFireTimes()
		{
			List<float> times = new List<float>();
			float timeUntilChange = secondsToFire;
			bool paused = false;
			for(float i = initialDelay; i < controller.entity.leave; i += secondsPerBullet)
			{
				if(secondsToPause > 0)
				{
					//Debug.Log(timeUntilChange);
					timeUntilChange -= secondsPerBullet;
					if(timeUntilChange <= 0)
					{
						paused = !paused;
						timeUntilChange = paused ? secondsToPause : secondsToFire;
					}
				}
				if(!paused)
					times.Add(i);
			}
			fireTimes = new float[times.Count];
			for(int i = 0; i < fireTimes.Length; i++)
			{
				fireTimes[i] = times[i] + (float)Stage.time;
			}
		}

		public void Update()
		{
			if(!(controller == null || controller.entity == null) && fireTimes == null && synced)
				GetFireTimes();

			currentSpread = Mathf.SmoothDamp(currentSpread, goalSpread, ref spreadSpeed, secondsPerSpreadPulse);
			if(Mathf.Abs(currentSpread - goalSpread) < 5f && !(spreadMin == 0 && spreadMax == 0))
			{
				goalSpread = Mathf.Abs(currentSpread - spreadMax) < Mathf.Abs(currentSpread - spreadMin) ? spreadMin : spreadMax;
			}

			currentAngle += currentSpinSpeed * Stage.deltaTime;
			currentSpinSpeed += spinAcceleration * (spinningClockwise ? -1 : 1);
			if(reverseSpin)
			{
				if(currentSpinSpeed < 0 != spinningClockwise)
					currentSpinSpeed *= reverseSpinSpeed;
				if(currentSpinSpeed >= maxSpinSpeed)
					spinningClockwise = true;
				else if(currentSpinSpeed <= -maxSpinSpeed)
					spinningClockwise = false;
			}
			else if(maxSpinSpeed != 0)
				currentSpinSpeed = Mathf.Clamp(currentSpinSpeed, -maxSpinSpeed, maxSpinSpeed);

			if(initialDelayTimer < initialDelay)
			{
				initialDelayTimer += Stage.deltaTime;
			}
			if(synced || (!synced && initialDelayTimer >= initialDelay))
			{
				if(secondsToPause != 0)
				{
					if(pauseTimer < secondsToFire)
						pauseTimer += Stage.deltaTime;
					else
					{
						pauseTimer = -secondsToPause;
						bulletFireTimer = secondsPerBullet;
					}
				}
				if(((secondsToPause == 0 || pauseTimer > 0) && !synced) || synced)
				{
					anglePerSet = numberOfSets > 1 ? (currentSpread / (numberOfSets)) : 0;
					setOffset = anglePerSet / 2;
					if((synced && Stage.time >= fireTimes[fireIndex]))
					{
						fireIndex++;
						FireSet();
					}
					if(!synced)
					{
						if(bulletFireTimer < secondsPerBullet)
							bulletFireTimer += Stage.deltaTime;
						else
						{
							bulletFireTimer = 0;
							FireSet();
						}
					}
				}
			}
		}

		private void FireSet()
		{
			if(numberOfSets == 1)
				FireBullet(0);
			else
			{
				for(float i = -numberOfSets / 2f; i < numberOfSets / 2f; i++)
				{
					FireBullet(i);
				}
			}
		}

		private void FireBullet(float i)
		{
			GameObject b = SpawnBullet();
			float bAngle = currentAngle + i * anglePerSet + setOffset + b.transform.rotation.eulerAngles.z;
			b.transform.rotation = Quaternion.Euler(0, 0, bAngle);
			BulletBase bullet = b.GetComponent<BulletBase>();
			bullet.velocity.x = (bulletSpeed * Mathf.Sin(Mathf.Deg2Rad * bAngle));
			bullet.velocity.y = -(bulletSpeed * Mathf.Cos(Mathf.Deg2Rad * bAngle));
			bullet.SetVelocityMultiplier(bulletSpeedMultiplier);
			bullet.SetLifetime(bulletLifetime);
		}

		public GameObject SpawnBullet()
		{
			return Object.Instantiate(bulletPrefab, controller.gameObject.transform.position + controller.rotatedPos, controller.gameObject.transform.rotation);
		}

	}
}