using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
	private 		List<GameObject> 	allUnitList 		= new List<GameObject>();
	private 		List<GameObject> 	selectedUnitList 	= new List<GameObject>();
	private static	UnitManager			instance;
	
	public static UnitManager GetInstance() 
	{
		if( instance == null )
		{
			instance = FindObjectOfType<UnitManager>();
		}
		return instance;
	}
	public int GetSelectedUnitCount()
	{
		return selectedUnitList.Count;
	}
	public void AddUnit(GameObject Unit)
	{
		allUnitList.Add(Unit);
	}
	void AddSelectedUnit(GameObject Unit)
	{
		selectedUnitList.Add(Unit);
		Unit.SendMessage("SetUnitSelected", true);
	}
	
	public void ClearSelectedUnitList()
	{
		foreach( GameObject Unit in allUnitList ) {
			Unit.SendMessage("SetUnitSelected", false);
		}
		selectedUnitList.Clear();
	}
	public void MoveSelectedUnitToPoint(Vector3 destinationPoint) 
	{
		foreach (GameObject Unit in selectedUnitList) {
			Unit.SendMessage("MoveToPoint", destinationPoint);
		}
	}
	
	public void SelectUnitsInArea(Vector3 StartPoint, Vector3 EndPoint)
	{
		if( EndPoint.x < StartPoint.x )
			swap<float>( ref EndPoint.x, ref StartPoint.x );
		if( EndPoint.y > StartPoint.y )
			swap<float>( ref EndPoint.y, ref StartPoint.y );
		
		foreach( GameObject Unit in allUnitList ) {
			Vector3 UnitPos = Unit.transform.position;
			if( UnitPos.x > StartPoint.x && UnitPos.x < EndPoint.x && UnitPos.y < StartPoint.y && UnitPos.y > EndPoint.y ) {
				selectedUnitList.Add(Unit);
				Unit.SendMessage("SetUnitSelected", true);
			}
		}
	}
	private void swap<T>(ref T lhs, ref T rhs)
	{
		T temp;
		temp = lhs;
		lhs = rhs;
		rhs = temp;
	}
}

