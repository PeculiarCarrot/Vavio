using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter (Collider col)
    {
		BulletProperties bullet = col.gameObject.GetComponent<BulletProperties>();
    	Enemy enemy = col.gameObject.GetComponent<Enemy>();
        if(bullet != null && !player.GetComponent<Player>().IsInvincible())
        {
			if(bullet.owner != "player")
			{
				if (bullet.destroyOnHit)
					bullet.Die(false);
				player.GetComponent<Player>().GetHurt();
			}
			return;
        }
		if(enemy != null && !player.GetComponent<Player>().IsInvincible() && enemy.CanCollide())
        {
			if(!enemy.IsInvincible())
            	enemy.GetHurt(50);
            player.GetComponent<Player>().GetHurt();
            return;
        }
    }
}
