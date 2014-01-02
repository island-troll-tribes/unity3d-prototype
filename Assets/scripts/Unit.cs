using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
	bool						moveToTarget;
	Vector3						targetPosition;
	private bool				isSelected;
	private float				rotationSpeed = 6.0f;
	private float				movementSpeed = 15.0f;
	private float				targetReachedRadius = 1.5f;
	private MovementState 		UnitState = MovementState.STOP;
	private GameObject 			healthBar;
	public GameObject 			healthBarPrefab;
	
	enum MovementState {
		STOP = 1,
		MOVE = 2
	};
	// Use this for initialization
	void Start ()
	{
		GameObject Unit = GameObject.Find("UnitManager");
		Unit.SendMessage("AddUnit", gameObject);
		
		healthBar = Instantiate( healthBarPrefab, transform.position, Quaternion.identity ) as GameObject;
		healthBar.transform.parent = gameObject.transform;
		healthBar.transform.position = new Vector3(transform.position.x, transform.position.y+5, transform.position.z);
		
		SetUnitSelected(false);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (moveToTarget) {
			
			Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
			float str = Mathf.Min (rotationSpeed * Time.deltaTime, 1);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, str);
			
			// Check direction angle. If greater than 60° then first turn without moving, otherwise full throttle ahead.
			Vector3 targetDir = targetPosition - transform.position;
			Vector3 move = transform.forward;
			float angle = Vector3.Angle(targetDir, move);
			if (angle > 35.0) {
				UnitState = MovementState.STOP;
			} else {
				UnitState = MovementState.MOVE;
			}
			// check the distance
			float distanceToDestination = Vector3.Distance(targetPosition, transform.position);
			//print ("Distance to other: " + dist);
			if (distanceToDestination < targetReachedRadius) {
				UnitState = MovementState.STOP;
				moveToTarget = false;
				distanceToDestination = 0.0f;
			}
		}
	}
	void FixedUpdate() {
		
		switch (UnitState)  {
		case MovementState.STOP :
			movementSpeed = 0.0f;
			break;
		case MovementState.MOVE:
			movementSpeed = 20.0f;
			break;
		}
		transform.Translate(Vector3.forward * Time.deltaTime * (movementSpeed));
	}
	
	public void MoveToPoint (Vector3 newTarget) {
		moveToTarget = true;
		UnitState = MovementState.MOVE;
		targetPosition = newTarget;
		//print ("Tank moving to: " + targetPosition);
	}
	public void SetUnitSelected(bool selected)
	{
		isSelected = selected;
		healthBarPrefab.GetComponent<HealthBar>().SetHealthEnabled(isSelected);
	}
	public void SetSelected() {
		print("I got selected... " + name);
		GameObject Unit = GameObject.Find("UnitManager");
		Unit.SendMessage("AddSelectedUnit", gameObject);
	}
}

