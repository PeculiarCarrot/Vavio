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
    	EnemyBullet bullet = col.gameObject.GetComponent<EnemyBullet>();
    	Enemy enemy = col.gameObject.GetComponent<Enemy>();
        if(bullet != null && !player.GetComponent<Player>().IsInvincible())
        {
            bullet.Die();
            player.GetComponent<Player>().GetHurt(bullet.GetDamage());
            return;
        }
        if(enemy != null && !player.GetComponent<Player>().IsInvincible())
        {
            enemy.GetHurt(50);
            player.GetComponent<Player>().GetHurt(50);
            return;
        }
    }
}
