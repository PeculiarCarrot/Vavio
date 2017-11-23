using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Lightning : MonoBehaviour {

	[HideInInspector]
	public Vector3 pos1, pos2;
	public float randomWidth = .5f;

	// Use this for initialization
	void Start () {
	}

	public void SetPoints(Vector3 point1, Vector3 point2)
	{
		int numPoints = Mathf.Max((int)(Vector3.Distance(point1, point2) * 5), 4);
		pos1 = point1;
		pos2 = point2;
		Vector3[] points = new Vector3[numPoints];
		points[0] = pos1;
		points[numPoints - 1] = pos2;

		for(int i = 1; i < numPoints - 1; i++)
		{
			points[i] = Vector3.Lerp(pos1, pos2, i / ((float)numPoints + 1));
			points[i].x += Random.Range(-randomWidth, randomWidth);
			if(Random.value < .1)
				points[i].x += Random.Range(-randomWidth, randomWidth);
		}

		GetComponent<LineRenderer>().positionCount = points.Length;
		GetComponent<LineRenderer>().SetPositions(points);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
