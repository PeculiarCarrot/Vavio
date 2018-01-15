using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDestroyParticles : MonoBehaviour {

	void Start () {
		ParticleSystem ps = GetComponent<ParticleSystem>();
		Destroy(this.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
	}

	void Update () {

	}
}
