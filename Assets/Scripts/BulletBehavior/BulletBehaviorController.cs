using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviorController : MonoBehaviour {

	public BulletBehavior behavior;
	public float x, y;

	// Use this for initialization
	void Start () {
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
	
	// Update is called once per frame
	void Update () {
		behavior.Update();
	}

	void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3( gameObject.transform.position.x + x,  gameObject.transform.position.y + y, gameObject.transform.position.z), .1f);
    }

	[System.Serializable]
	public class BulletBehavior {

		public TextAsset behavior;
		public GameObject bulletPrefab;

		[HideInInspector]
		public int numberOfSets;
		[HideInInspector]
		public bool reverseSpin;
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
		private float currentSpread;
		private float anglePerSet, setOffset;

		//Constant timer values
		private float secondsPerBullet;

		//Timers
		private float bulletFireTimer;
		private float initialDelayTimer;
		private float pauseTimer;

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
		}

		public void LoadFromFile()
		{
			RestoreDefaults();
			JsonUtility.FromJsonOverwrite(behavior.text, this);
			currentAngle = angle;
			currentSpread = spread;
			secondsPerBullet = 1 / bulletsPerSecond;
			bulletFireTimer = secondsPerBullet;
		}

		public void Update()
		{
			Debug.Log(currentSpinSpeed);
			currentAngle += currentSpinSpeed * Time.deltaTime;
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
			else
				currentSpinSpeed = Mathf.Clamp(currentSpinSpeed, -maxSpinSpeed, maxSpinSpeed);

			if(initialDelayTimer < initialDelay)
			{
				initialDelayTimer += Time.deltaTime;
			}
			else
			{
				if(pauseTimer < secondsToFire)
					pauseTimer += Time.deltaTime;
				else
				{
					pauseTimer -= secondsToFire + secondsToPause;
					bulletFireTimer = secondsPerBullet;
				}

				if(pauseTimer > 0 || pauseTimer == 0)
				{
					anglePerSet = numberOfSets > 1 ? (currentSpread / (numberOfSets - 1)) : 0;
					setOffset = anglePerSet / 2;
					if(bulletFireTimer < secondsPerBullet)
						bulletFireTimer += Time.deltaTime;
					else
					{
						bulletFireTimer = 0;
						FireSet();
					}
				}
			}
		}

		private void FireSet()
		{
			if(numberOfSets == 1)
				FireBullet(0);
			else
				for(float i = -numberOfSets / 2f; i < numberOfSets / 2f; i++)
					FireBullet(i);
		}

		private void FireBullet(float i)
		{
			GameObject b = SpawnBullet();
			float bAngle = currentAngle + i * anglePerSet + setOffset;
			b.transform.Rotate(0, 0, bAngle);
			BulletBase bullet = b.GetComponent<BulletBase>();
			bullet.velocity.x = (bulletSpeed * Mathf.Sin(Mathf.Deg2Rad * bAngle));
			bullet.velocity.y = -(bulletSpeed * Mathf.Cos(Mathf.Deg2Rad * bAngle));
			bullet.SetVelocityMultiplier(bulletSpeedMultiplier);
			bullet.SetLifetime(bulletLifetime);
		}

		public GameObject SpawnBullet()
		{
			return Object.Instantiate(bulletPrefab, controller.gameObject.transform.position + new Vector3(controller.x, controller.y, 0), controller.gameObject.transform.rotation);
		}

	}
}