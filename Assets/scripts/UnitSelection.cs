using UnityEngine;
using System.Collections;

public class UnitSelection : MonoBehaviour
{
	private 		Vector2			leftClickDownPoint;
	private 		Vector2 		leftClickUpPoint;
	
	private 		Vector3 		rightTerrainHitPoint;
	
	private 		Vector3			rectStart;
	private 		Vector3			rectEnd;
	
	private 		LineRenderer 	lineRenderer;
	
	private 		bool 			leftDrag;
	
	private 		float			raycastLength		= 200.0f;
	private static 	LayerMask 		terrainLayerMask 	= 1 << 8;
	private static 	LayerMask 		unitLayerMask 		= ~terrainLayerMask;
	
	private 		int				ClickReleaseRange	= 20;
	
	public 			Texture 		selectionTexture;
	
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
		Ray ray = Camera.main.ScreenPointToRay (leftClickDownPoint); 
		if ( Physics.Raycast (ray, out hit, raycastLength) )
		{ 
			if (hit.collider.name == "Terrain")
			{
				rectStart = hit.point;
			} 
			else
			{
				UnitManager.GetInstance().ClearSelectedUnitList();
			}
		}
	}
	
	void LeftClickUp(Vector2 ClickPosition)
	{
		leftClickUpPoint = ClickPosition;
		leftDrag = false;
		
		if (IsInRange(leftClickDownPoint, leftClickUpPoint)) {
			if (UnitManager.GetInstance().GetSelectedUnitCount() == 0) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (leftClickDownPoint);
				if ( Physics.Raycast (ray, out hit, raycastLength, unitLayerMask) )
				{ 
					hit.collider.gameObject.SendMessage("SetSelected");
				}
			}
		}
	}
	
	void LeftClickDrag(Vector2 ClickPosition)
	{
		if(ClickPosition != leftClickDownPoint)
		{
			leftDrag = true;
			leftClickUpPoint = ClickPosition;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (leftClickUpPoint); 		
			if ( Physics.Raycast (ray, out hit, raycastLength, terrainLayerMask) )
			{ 
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
		if ( Physics.Raycast (ray, out hit, raycastLength) )
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
		if( UnitManager.GetInstance().GetSelectedUnitCount() > 0 ) {
			UnitManager.GetInstance().MoveSelectedUnitToPoint(rightTerrainHitPoint);
		}
	}
	bool IsInRange(Vector2 v1, Vector2 v2)
	{
		if( Vector2.Distance(v1, v2) < ClickReleaseRange )
			return true;
		return false;
	}
}

