using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class BubbleAbilityObject : MonoBehaviour {
	public int segments = 100;
	public float xradius = 5;
	public float yradius = 5;
	LineRenderer line;
	float spd = 10f;

	private Transform target;

	void Start ()
	{
		xradius = 1f;
		yradius = 1f;
		line = gameObject.GetComponent<LineRenderer>();
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
		Vector3 newScale = transform.localScale;
		//newScale.x += spd * Time.deltaTime;
		//newScale.y += spd * Time.deltaTime;
		//newScale.z += spd * Time.deltaTime;
		transform.localScale = newScale;

		//spd *= 1.1f;

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
		bullet.Die(false);
	}

	void CreatePoints ()
	{
		float x;
		float y;

		float angle = 20f;

		for (int i = 0; i < (segments + 1); i++)
		{
			x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius * transform.localScale.x;
			y = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius * transform.localScale.y;

			x += transform.position.x;
			y += transform.position.y;

			line.SetPosition (i,new Vector3(x,y,0) );

			angle += (360f / segments);
		}
	}
}