using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class DeathEffect : MonoBehaviour {
	public int segments = 100;
	public float xradius = 5;
	public float yradius = 5;
	public LineRenderer line;
	public float spd = 2f;
	public float shrink = .3f;

	void Awake ()
	{
		xradius = 1f;
		yradius = 1f;
		line = gameObject.GetComponent<LineRenderer>();
	}

	void Update()
	{
		Vector3 newScale = transform.localScale;
		newScale.x += spd * Time.deltaTime;
		newScale.y += spd * Time.deltaTime;
		newScale.z += spd * Time.deltaTime;
		transform.localScale = newScale;

		//spd *= 1.1f;

		line.positionCount = segments + 1;
		line.startWidth -= shrink * Time.deltaTime;
		line.endWidth -= shrink * Time.deltaTime;

		if(line.startWidth <= 0)
		{
			Destroy(gameObject);
			return;
		}

		line.useWorldSpace = true;
		CreatePoints ();

		Vector3 v1 = transform.position;
		v1.z = 0;
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

			line.SetPosition (i,new Vector3(x,y,transform.position.z) );

			angle += (360f / segments);
		}
	}
}