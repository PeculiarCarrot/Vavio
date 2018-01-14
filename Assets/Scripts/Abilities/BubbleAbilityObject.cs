using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class BubbleAbilityObject : MonoBehaviour {
	public int segments = 100;
	public float rad = 0, goalRad = 1f;
	LineRenderer line;
	float spd = 10f;
	public Material hitMaterial;

	private static GameObject deathEffect;

	public AudioClip[] hitSounds;

	private Transform target;

	void Start ()
	{
		rad = 0;
		line = gameObject.GetComponent<LineRenderer>();
		if(deathEffect == null)
			deathEffect = Resources.Load<GameObject>("Prefabs/Effects/deathEffect");
	}

	public void SetTarget(Transform t)
	{
		target = t;
	}

	void Update()
	{
		if(target != null)
		{
			transform.position = target.position;
		}

		rad = Mathf.Lerp(rad, goalRad, 5 * Time.deltaTime);

		line.positionCount = segments + 1;
		line.useWorldSpace = true;
		CreatePoints ();
	}

	public void OnTriggerEnter(Collider c)
	{
		GameObject o = c.gameObject;
		BulletProperties bullet = o.GetComponent<BulletProperties>();
		if(o == null || !o.activeInHierarchy || bullet == null || bullet.owner == "player")
			return;
		MakeBlockParticle(bullet, o.transform.position + new Vector3(0, 0, -3));
		bullet.Die(false);
	}

	void MakeBlockParticle(BulletProperties bullet, Vector3 pos)
	{
		GameObject e = Instantiate(deathEffect, pos, deathEffect.transform.rotation);
		e.GetComponent<DeathEffect>().spd = 1f;
		e.GetComponent<DeathEffect>().shrink = .5f;
		e.GetComponent<LineRenderer>().material = hitMaterial;
		EnemyAudio.Play(hitSounds);
	}

	void CreatePoints ()
	{
		float x;
		float y;

		float angle = 20f;

		for (int i = 0; i < (segments + 1); i++)
		{
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * rad * transform.localScale.x;
			y = Mathf.Cos (Mathf.Deg2Rad * angle) * rad * transform.localScale.y;

			x += transform.position.x;
			y += transform.position.y;

			line.SetPosition (i,new Vector3(x,y,0) );

			angle += (360f / segments);
		}
	}
}