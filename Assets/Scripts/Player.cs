using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Ship {

	// Use this for initialization
	public override void DoStart () {
		
	}

	void OnTriggerEnter (Collider col)
    {
    	EnemyBullet bullet = col.gameObject.GetComponent<EnemyBullet>();
        if(bullet != null)
        {
            bullet.Die();
            GetHurt(bullet.GetDamage());
            return;
        }
    }
	
	// Update is called once per frame
	public override void DoUpdate () {
		if(Input.GetKey(KeyCode.UpArrow))
			MoveUp();
		if(Input.GetKey(KeyCode.DownArrow))
			MoveDown();
		if(Input.GetKey(KeyCode.LeftArrow))
			MoveLeft();
		if(Input.GetKey(KeyCode.RightArrow))
			MoveRight();
		if(Input.GetKey(KeyCode.Z) && CanShoot())
			Shoot();
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, stage.GetComponent<Stage>().minX, stage.GetComponent<Stage>().maxX),
			Mathf.Clamp(transform.position.y, stage.GetComponent<Stage>().minY, stage.GetComponent<Stage>().maxY), transform.position.z);
	}
}
