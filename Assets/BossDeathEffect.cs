using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class BossDeathEffect : MonoBehaviour {
	public int segments = 100;
	public float xradius = 5;
	public float yradius = 5;
	LineRenderer line;
	float spd = 10f;

	List<GameObject> objects = new List<GameObject>();

	void Start ()
	{
		xradius = 1f;
		yradius = 1f;
		line = gameObject.GetComponent<LineRenderer>();
		foreach (GameObject o in Stage.GetBullets())
			objects.Add(o);
		foreach (GameObject o in Stage.GetEnemies())
			objects.Add(o);
		GameObject.Find("Spawner").GetComponent<EnemySpawner>().timeUntilNext = 4;
		GameObject.Find("Stage").GetComponent<Stage>().song.Pause();
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
		line.useWorldSpace = true;
		CreatePoints ();

		Vector3 v1 = transform.position;
		v1.z = 0;
		List<GameObject> toRemove = new List<GameObject>();
		foreach(GameObject o in objects)
		{
			if(!o.activeInHierarchy)
			{
				toRemove.Add(o);
				continue;
			}
			Vector3 v2 = o.transform.position;
			v2.z = 0;
			if(Vector3.Distance(v1, v2) <= xradius * transform.localScale.x)
			{
				BulletProperties bullet = o.GetComponent<BulletProperties>();
				Enemy enemy = o.GetComponent<Enemy>();
				if(enemy != null)
				{
					enemy.Die();
					toRemove.Add(o);
				}
				if(bullet != null)
				{
					bullet.Die();
					toRemove.Add(o);
				}
			}
		}

		foreach (GameObject o in toRemove)
		{
			objects.Remove(o);
		}
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