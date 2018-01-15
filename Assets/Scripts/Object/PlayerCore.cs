using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour {

	public GameObject player;
	private Vector3 lastPos;

	// Use this for initialization
	void Start () {
		lastPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void OnTriggerEnter (Collider col)
    {
		TestTouch(col);
    }

	void TestTouch(Collider col)
	{
		Enemy enemy = col.gameObject.GetComponent<Enemy>();
		if(enemy != null && !player.GetComponent<Player>().IsInvincible() && enemy.CanCollide())
		{
			if(!enemy.IsInvincible())
				enemy.GetHurt(50);
			player.GetComponent<Player>().GetHurt();
			return;
		}

		BulletProperties bullet = col.gameObject.GetComponent<BulletProperties>();
		if(bullet != null && !player.GetComponent<Player>().IsInvincible())
		{
			if(bullet.owner != "player")
			{
				if (bullet.destroyOnHit)
					bullet.Die(false);
				player.GetComponent<Player>().GetHurt();
				player.GetComponent<Player>().MakeBulletEffect(bullet);
			}
			return;
		}
	}

	void OnTriggerStay(Collider col)
	{
		TestTouch(col);
	}
}
