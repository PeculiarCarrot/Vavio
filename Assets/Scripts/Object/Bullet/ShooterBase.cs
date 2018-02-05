using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBase : MonoBehaviour {
	public float leave;

	public float lifetime;

	void Start () {

	}

	void Update () {
		lifetime += Time.deltaTime;
	}
}