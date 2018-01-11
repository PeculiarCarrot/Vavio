using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyeball : MonoBehaviour {

	public GameObject eyeball;
	public Material eyeballMaterial;
	private GameObject player;
	Quaternion last;
	private bool watching;

	// Use this for initialization
	void Start () {
		last = eyeball.transform.rotation;
		player = Stage.stage.player;
		eyeball.GetComponent<MeshRenderer>().material = eyeballMaterial;
	}

	public void StartWatch()
	{
		watching = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(watching)
			eyeball.transform.rotation = Quaternion.Lerp(last, Quaternion.LookRotation(eyeball.transform.position - player.transform.position), 3f * Time.deltaTime);
		last = eyeball.transform.rotation;
	}
}
