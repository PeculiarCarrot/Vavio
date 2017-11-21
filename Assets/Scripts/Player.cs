using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Ship {

	// Use this for initialization
	public override void DoStart () {
		Cursor.visible = false;
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
	public void Update () {
		DoUpdate();
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
	}
}
