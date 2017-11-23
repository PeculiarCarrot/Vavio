using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Ship {

	public GameObject mesh;
	public GameObject core;
	private PowerUp.PowerUpType currentPowerUp;
	private float remainingPowerUpDuration;
	private float invincibilityDuration;
	private float flickerTimer, flickerDuration = .05f;

	// Use this for initialization
	public override void DoStart () {
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


	public new void GetHurt(float damage)
	{
		hp -= damage;
		invincibilityDuration = 1.5f;
		flickerTimer = flickerDuration;
	}

	public new void Die()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	
    private void ApplyPowerUp(PowerUp.PowerUpType type, float duration)
    {
    	currentPowerUp = type;
    	remainingPowerUpDuration = duration;
    }

	public new bool CanShoot()
	{
		return shootCooldown <= 0;
	}

	private float GetShootCooldownAmount()
	{
		switch(currentPowerUp)
		{
			case PowerUp.PowerUpType.FastShot:
				return .03f;
			case PowerUp.PowerUpType.HomingShot:
				return .2f;
			default:
				return shootCooldownAmount;
		}
	}

	protected override void Shoot()
	{
		GameObject bulletPrefab = this.bullet;
		if(currentPowerUp == PowerUp.PowerUpType.HomingShot)
			bulletPrefab = (GameObject)Resources.Load("PlayerBulletHoming");
		GameObject bulletInstance = null;
		switch(currentPowerUp)
		{
			case PowerUp.PowerUpType.TripleShot:
				float speed = 20f;
				for(int i = -1; i < 2; i++)
				{
					Quaternion rotation = Quaternion.Euler(bulletPrefab.transform.rotation.eulerAngles.x, bulletPrefab.transform.rotation.eulerAngles.y, bulletPrefab.transform.rotation.eulerAngles.z + 10 * i);
					bulletInstance = Object.Instantiate(bulletPrefab, bulletSpawn.transform.position, rotation);
					bulletInstance.GetComponent<PlayerBullet>().velocity.x = Mathf.Sin(Mathf.Deg2Rad * rotation.eulerAngles.z) * speed;
					bulletInstance.GetComponent<PlayerBullet>().velocity.y = Mathf.Cos(Mathf.Deg2Rad * rotation.eulerAngles.z) * speed;
				}
			break;
			case PowerUp.PowerUpType.HomingShot:
				bulletInstance = Object.Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletPrefab.transform.rotation);
				bulletInstance.GetComponent<PlayerBullet>().velocity.x = 13f;
			break;
			default:
				bulletInstance = Object.Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletPrefab.transform.rotation);
				bulletInstance.GetComponent<PlayerBullet>().velocity.x = 40f;
			break;
		}

		shootCooldown = GetShootCooldownAmount();
	}

	public bool IsInvincible()
	{
		return invincibilityDuration > 0;
	}

	// Update is called once per frame
	public void Update () {
		Cursor.visible = false;
		DoUpdate();

		if(currentPowerUp != PowerUp.PowerUpType.None)
		{
			remainingPowerUpDuration -= Time.deltaTime;
			if(remainingPowerUpDuration <= 0)
			{
				remainingPowerUpDuration = 0;
				currentPowerUp = PowerUp.PowerUpType.None;
			}
		}
		if(invincibilityDuration > 0)
		{
			invincibilityDuration -= Time.deltaTime;
			flickerTimer -= Time.deltaTime;
			if(flickerTimer < - flickerDuration)
				flickerTimer = flickerDuration;
			mesh.GetComponent<Renderer>().enabled = invincibilityDuration <= 0 || flickerTimer < 0;
			core.GetComponent<Renderer>().enabled = invincibilityDuration <= 0 || flickerTimer < 0;
		}

		/*if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
			MoveUp();
		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
			MoveDown();
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			MoveLeft();
		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			MoveRight();*/
		Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = transform.position.z;
        rotSpeed += rotAccel * (target.y - transform.position.y) * 2;
        transform.position = Vector3.Lerp(transform.position, target, .3f);

		if(CanShoot())
			Shoot();
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, stage.GetComponent<Stage>().minX, stage.GetComponent<Stage>().maxX),
			Mathf.Clamp(transform.position.y, stage.GetComponent<Stage>().minY, stage.GetComponent<Stage>().maxY), transform.position.z);
		if(hp <= 0)
			Die();
	}
}
