using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(BulletBehaviorController))]
public class Player : Ship {

	public GameObject mesh;
	public GameObject core;
	private PowerUp.PowerUpType currentPowerUp;
	private float remainingPowerUpDuration;
	private float invincibilityDuration;
	private float flickerTimer, flickerDuration = .05f;
	public bool debug;

	// Use this for initialization
	public override void DoStart () {
		rotFric = 1;
		rotAccel = 6;
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
		if(!debug)
		{
			hp -= damage;
			invincibilityDuration = 1.5f;
			flickerTimer = flickerDuration;
		}
	}

	public new void Die()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	
    private void ApplyPowerUp(PowerUp.PowerUpType type, float duration)
    {
    	currentPowerUp = type;
    	remainingPowerUpDuration = duration;

    	switch(type)
    	{
    		case PowerUp.PowerUpType.FastShot:
    		GetComponent<BulletBehaviorController>().behavior.behavior = Resources.Load("BulletBehaviors/playerFastShot") as TextAsset;
    		GetComponent<BulletBehaviorController>().behavior.bulletPrefab = Resources.Load("Prefabs/PlayerBullet") as GameObject;
    		break;
    		case PowerUp.PowerUpType.TripleShot:
    		GetComponent<BulletBehaviorController>().behavior.behavior = Resources.Load("BulletBehaviors/playerTripleShot") as TextAsset;
    		GetComponent<BulletBehaviorController>().behavior.bulletPrefab = Resources.Load("Prefabs/PlayerBullet") as GameObject;
    		break;
    		case PowerUp.PowerUpType.HomingShot:
    		GetComponent<BulletBehaviorController>().behavior.behavior = Resources.Load("BulletBehaviors/playerHomingShot") as TextAsset;
    		GetComponent<BulletBehaviorController>().behavior.bulletPrefab = Resources.Load("Prefabs/PlayerBulletHoming") as GameObject;
    		break;
    		default:
    		GetComponent<BulletBehaviorController>().behavior.behavior = Resources.Load("BulletBehaviors/player") as TextAsset;
    		GetComponent<BulletBehaviorController>().behavior.bulletPrefab = Resources.Load("Prefabs/PlayerBullet") as GameObject;
    		break;
    	}
    	GetComponent<BulletBehaviorController>().behavior.LoadFromFile();
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
				ApplyPowerUp(PowerUp.PowerUpType.None, 0);
			}
		}
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
        transform.position = Vector3.Lerp(transform.position, target, .3f);

        GetComponent<BulletBehaviorController>().enabled = !debug;

		transform.position = new Vector3(Mathf.Clamp(transform.position.x, stage.GetComponent<Stage>().minX, stage.GetComponent<Stage>().maxX),
			Mathf.Clamp(transform.position.y, stage.GetComponent<Stage>().minY, stage.GetComponent<Stage>().maxY), transform.position.z);
		if(hp <= 0)
			Die();
	}
}
