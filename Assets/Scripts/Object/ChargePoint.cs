using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePoint : MonoBehaviour {

	public float charge = 1f;
	
	void Start () {
		transform.localScale *= charge;
	}

	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		Player player = other.GetComponentInParent<Player>();
		if(player != null)
		{
			player.AddCharge(charge);
			Destroy(gameObject);
		}
	}
}
