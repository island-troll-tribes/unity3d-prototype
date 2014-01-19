using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
	byte						team;
	bool						moveToTarget;
	Vector3						targetPosition;
	private bool				isSelected;
	private Vector3				targetDir;
	private float				angle;
	private float				movementSpeed = 15.0f;
	private float				targetReachedRadius = 1.5f;
	private MovementState 		UnitState = MovementState.STOP;
	public 	float 				health = 150.0f;
	public 	float 				maxHealth = 150.0f;
	public  int					level = 1;
	public  Texture2D 			backgroundTexture;
	public 	Texture2D 			healthTexture;
	public  bool				isHero = true;
	public  bool 				isBuilding;
	public  float 				xp = 0;
	
	enum MovementState {
		STOP = 1,
		MOVE = 2
	};
	// Use this for initialization
	void Start ()
	{
		GameObject Unit = GameObject.Find("UnitManager");
		Unit.SendMessage("AddUnit", gameObject);
		
		SetUnitSelected(false);
	}

	// Update is called once per frame
	void Update ()
	{
		if (moveToTarget) {
			targetDir = targetPosition - transform.position;
			transform.rotation = Quaternion.LookRotation( targetDir );
			angle = Vector3.Angle( targetDir, transform.forward);
			if (angle > 35.0) {
				UnitState = MovementState.STOP;
			} else {
				UnitState = MovementState.MOVE;
			}
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
	}
	public void SetUnitSelected(bool selected)
	{
		isSelected = selected;
		// healthBarPrefab.GetComponent<HealthBar>().SetHealthEnabled(isSelected);
	}
	public void SetSelected() {
		print("I got selected... " + name);
		GameObject Unit = GameObject.Find("UnitManager");
		Unit.SendMessage("AddSelectedUnit", gameObject);
	}
	void OnGUI ()
	{
		
		Vector2 backgroundBarSize = new Vector2 (Screen.width * 0.2f, Screen.height * 0.06f);
		
		Vector3 viewPos = Camera.main.WorldToScreenPoint (this.transform.position + new Vector3 (0, 3, 0));
		
		float valueZ = viewPos.z;
		if (valueZ < 1) {
			valueZ = 1;
		} else if (valueZ > 4) {
			valueZ = 4;
		}
		float valueToNormalize = Mathf.Abs (1 / (valueZ - 0.5f));
		
		int backgroundBarWidth = (int)(backgroundBarSize.x * valueToNormalize);
		if (backgroundBarWidth % 2 != 0) {
			backgroundBarWidth++;
		}
		float backgroundBarHeight = (int)(backgroundBarSize.y * valueToNormalize);
		if (backgroundBarHeight % 2 != 0) {
			backgroundBarHeight++;
		}
		
		float innerBarWidth = backgroundBarWidth - 2 * 2;
		float innerBarHeight = backgroundBarHeight - 2 * 2;
		
		
		float posYHealthBar = Screen.height - viewPos.y - backgroundBarHeight;
		
		GUI.BeginGroup (new Rect (viewPos.x - backgroundBarWidth / 2, posYHealthBar, backgroundBarWidth, backgroundBarHeight));
		GUI.DrawTexture (new Rect (0, 0, backgroundBarWidth, backgroundBarHeight), backgroundTexture, ScaleMode.StretchToFill);
		
		float healthPercent = (health / maxHealth);
		GUI.DrawTexture (new Rect (2, 2, innerBarWidth * healthPercent, innerBarHeight), healthTexture, ScaleMode.StretchToFill);
		
		GUI.EndGroup ();
		
		if (isHero) {
			GUI.Label(new Rect (viewPos.x - 50, posYHealthBar - 23, 100, 25), "Level "+level);
		}
		
	}
}

