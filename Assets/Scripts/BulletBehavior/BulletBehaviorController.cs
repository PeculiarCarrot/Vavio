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
		public float angle,
		spread, spreadMin, spreadMax,
		secondsPerSpreadPulse,
		bulletsPerSecond, bulletSpeed, bulletSpeedMultiplier, bulletLifetime,
		degreesPerSecond, maxAngleDifference,
		secondsToFire, secondsToPause, initialDelay;

		private BulletBehaviorController controller;

		//Calculated constant values

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

		public void LoadFromFile()
		{
			JsonUtility.FromJsonOverwrite(behavior.text, this);
			currentAngle = angle;
			currentSpread = spread;
			secondsPerBullet = 1 / bulletsPerSecond;
			bulletFireTimer = secondsPerBullet;
		}

		public void Update()
		{
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

				if(pauseTimer > 0)
				{
					anglePerSet = numberOfSets > 1 ? (currentSpread / (numberOfSets - 1)) : 0;
					setOffset = anglePerSet / 2;
					if(bulletFireTimer < secondsPerBullet)
						bulletFireTimer += Time.deltaTime;
					else
					{
						bulletFireTimer -= secondsPerBullet;
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
			bullet.velocity.x = (float)(bulletSpeed * Mathf.Sin(Mathf.Deg2Rad * bAngle));
			bullet.velocity.y = -(float)(bulletSpeed * Mathf.Cos(Mathf.Deg2Rad * bAngle));
			bullet.SetVelocityMultiplier(bulletSpeedMultiplier);
			bullet.SetLifetime(bulletLifetime);
		}

		public GameObject SpawnBullet()
		{
			return Object.Instantiate(bulletPrefab, controller.gameObject.transform.position + new Vector3(controller.x, controller.y, 0), controller.gameObject.transform.rotation);
		}

	}
}