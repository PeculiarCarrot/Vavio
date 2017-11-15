using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Ship {

	// Use this for initialization
	public override void DoStart () {
		
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
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9, 9), Mathf.Clamp(transform.position.y, -4.5f, 4.5f), transform.position.z);
	}
}
