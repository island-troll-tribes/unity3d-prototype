using UnityEngine;
using System.Collections;

public class UnitSelection : MonoBehaviour
{
	private Vector2			leftClickDownPoint;
	private Vector2 		leftClickUpPoint;
	
	private Vector2 		rightClickDownPoint;
	
	private Vector3 		rightTerrainHitPoint;
	private Vector3			leftTerrainHitPoint;
	
	private Vector3			rectStart;
	private Vector3			rectEnd;
	
	private LineRenderer 	lineRenderer;
	
	private bool 			leftDrag			= false;
	
	private float			raycastLength		= 1000.0f;
	private static int		terrainLayerMask 	= 1 << 8;
	private static int 		nonTerrainLayerMask = ~terrainLayerMask;
	
	private int				ClickReleaseRange	= 20;
	
	public Texture 			selectionTexture;
	
	void OnGUI() {
		if (leftDrag) {
			
			int width = (int)(leftClickUpPoint.x - leftClickDownPoint.x);
			int height = (int)((Screen.height - leftClickUpPoint.y) - (Screen.height - leftClickDownPoint.y));
			Rect rect = new Rect(leftClickDownPoint.x, Screen.height - leftClickDownPoint.y, width, height);
			GUI.DrawTexture (rect, selectionTexture, ScaleMode.StretchToFill, true);
		}
	}
	void Update() {
		
		if (Input.GetMouseButtonDown(0)) // Left Click Down
		{
			LeftClickDown(Input.mousePosition);
		}
		if (Input.GetMouseButtonUp(0)) // Left Click Up
		{
			LeftClickUp(Input.mousePosition);
		}
		
		if (Input.GetMouseButton(0)) // Left Click
		{
			LeftClickDrag(Input.mousePosition);
		}
		
		if (Input.GetMouseButtonDown(1)) // Right Click Down
		{
			RightClickDown(Input.mousePosition);
		}
		if (Input.GetMouseButtonUp(1)) // Right Click Up
		{
			RightClickUp(Input.mousePosition);
		}
	}
	
	void LeftClickDown(Vector2 ClickPosition)
	{
		leftClickDownPoint = ClickPosition;
		
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (ClickPosition); 
		//Debug.DrawRay (ray.origin, ray.direction * 100.0, Color.green); 
		if ( Physics.Raycast (ray, out hit, raycastLength) ) // terrainLayerMask
		{ 
			if (hit.collider.name == "Terrain")
			{
				leftTerrainHitPoint = hit.point;
				rectStart = hit.point;
			} 
			else
			{
				//print ("Mouse Down Hit something: " + hit.collider.name);
				
				// Ray hit a unit, not the terrain. Deselect all units as the fire 1 up 
				// event will then select that just recently clicked unit!
				UnitManager.GetInstance().ClearSelectedUnitList();
			}
			//Debug.DrawRay (ray.origin, ray.direction * 100.0, Color.green); 	
		}
	}
	
	void LeftClickUp(Vector2 ClickPosition)
	{
		leftClickUpPoint = ClickPosition;
		
		//print("currently selected units: " + UnitManager.GetInstance().GetSelectedUnitsCount());	
		leftDrag = false;
		
		if (IsInRange(leftClickDownPoint, leftClickUpPoint)) {
			// user just did a click, no dragging. mouse 1 down and up pos are equal.
			// if units are selected, move them. If not, select that unit.
			if (UnitManager.GetInstance().GetSelectedUnitCount() == 0) {
				// no units selected, select the one we clicked - if any.
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (leftClickDownPoint);
				if ( Physics.Raycast (ray, out hit, raycastLength, nonTerrainLayerMask) )
				{ 
					// Ray hit something. Try to select that hit target. 
					//print ("Hit something: " + hit.collider.name);
					hit.collider.gameObject.SendMessage("SetSelected");
				}
				
			} else {
				// untis are selected, move them. Unit Manager's unit count is > 0!
				UnitManager.GetInstance().MoveSelectedUnitToPoint(leftTerrainHitPoint);
			}	
		}	
	}
	
	void LeftClickDrag(Vector2 ClickPosition)
	{
		if(ClickPosition != rightClickDownPoint)
		{
			leftDrag = true;
			leftClickUpPoint = ClickPosition;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (ClickPosition); 		
			if ( Physics.Raycast (ray, out hit, raycastLength, terrainLayerMask) )
			{ 
				//print ("Hit Terrain 2 " + hit.point);
				rectEnd = hit.point;
				UnitManager.GetInstance().ClearSelectedUnitList();
				UnitManager.GetInstance().SelectUnitsInArea(rectStart, rectEnd);
			}	
			
		}
	}
	
	void RightClickDown(Vector2 ClickPosition)
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (ClickPosition);
		if ( Physics.Raycast (ray, out hit, raycastLength) ) // terrainLayerMask
		{ 
			if (hit.collider.name == "Terrain")
			{
				print ("RightMouse Down Hit Terrain " + hit.point);
				
				rightTerrainHitPoint = hit.point;
			} 
			else
			{
				print ("RightMouse Down Hit something: " + hit.collider.name);
				// Ray hit a unit
			}
		}
	}
	
	void RightClickUp(Vector2 ClickPosition)
	{
		rightClickDownPoint = Input.mousePosition;
		if( UnitManager.GetInstance().GetSelectedUnitCount() > 0 ) {
			UnitManager.GetInstance().MoveSelectedUnitToPoint(rightTerrainHitPoint);
		}
	}
	bool IsInRange(Vector2 v1, Vector2 v2)
	{
		float dist = Vector2.Distance(v1, v2);
		print("Right click release button distance: " + dist);
		if( dist < ClickReleaseRange )
			return true;
		return false;
	}
}

