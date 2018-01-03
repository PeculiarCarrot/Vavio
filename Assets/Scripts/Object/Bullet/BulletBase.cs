using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour {

	[HideInInspector]
	public Vector3 velocity = Vector3.zero;
	private float velocityMultiplier = 1;
	private float remainingLife = 30;

	void Awake () {
		Initialize();
	}

	void Update () {
		UpdateBullet();
	}

	public void SetVelocityMultiplier(float velocityMultiplier)
	{
		this.velocityMultiplier = velocityMultiplier - 1;
	}

	public void SetLifetime(float lifetime)
	{
		remainingLife = lifetime;
	}

	public void Initialize()
	{

	}

	public void OnDie()
	{

	}

	public void Die()
	{
		OnDie();
		Destroy(gameObject);
	}

	public void UpdateBullet () {
		remainingLife -= Time.deltaTime;
		velocity += velocity * velocityMultiplier * Time.fixedDeltaTime; 
		transform.position += velocity * Time.deltaTime;

		if(remainingLife <= 0)
			Die();
	}
}