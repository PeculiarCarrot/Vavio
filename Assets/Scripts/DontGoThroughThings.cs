using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontGoThroughThings : MonoBehaviour
{
	// Careful when setting this to true - it might cause double
	// events to be fired - but it won't pass through the trigger
	public bool sendTriggerMessage = false; 	

	public LayerMask layerMask = -1; //make sure we aren't in this layer 
	public float skinWidth = 0.1f; //probably doesn't need to be changed 

	private float minimumExtent; 
	private float partialExtent; 
	private float sqrMinimumExtent; 
	private Vector3 previousPosition; 
	private Rigidbody myRigidbody;
	private Collider myCollider;
	public PlayerCore core;

	//initialize values 
	void Start() 
	{ 
		myRigidbody = GetComponent<Rigidbody>();
		myCollider = GetComponent<Collider>();
		previousPosition = myRigidbody.position; 
		minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z); 
		partialExtent = minimumExtent * (1.0f - skinWidth); 
		sqrMinimumExtent = minimumExtent * minimumExtent; 
	} 

	void FixedUpdate() 
	{ 
		//have we moved more than our minimum extent? 
		Vector3 movementThisStep = transform.position - previousPosition; 
		float movementSqrMagnitude = movementThisStep.sqrMagnitude;

		if (Vector3.Distance(transform.position, previousPosition) > .5f) 
		{ 
			//Debug.Log(movementThisStep);
			float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
			RaycastHit hitInfo; 
			Debug.DrawRay(previousPosition, movementThisStep, Color.red, .3f);

			//check for obstructions we might have missed 
			if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, Vector3.Distance(transform.position, previousPosition), layerMask.value))
			{
				//Debug.Log("Hit something");
				if (!hitInfo.collider)
					return;

				if (hitInfo.collider.isTrigger)
				{
					core.OnTriggerEnter(hitInfo.collider);
				}

			}
		} 

		previousPosition = transform.position; 
	}
}