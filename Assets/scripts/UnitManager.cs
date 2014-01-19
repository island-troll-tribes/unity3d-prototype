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
	
	public void SelectUnitsInArea(Vector3 point1, Vector3 point2)
	{
		if (point2.x < point1.x) {
			// swap x positions. Selection rectangle is beeing drawn from right to left
			var x1 = point1.x;
			var x2 = point2.x;
			point1.x = x2;
			point2.x = x1;
		}
		
		if (point2.z > point1.z) {
			// swap z positions. Selection rectangle is beeing drawn from bottom to top
			var z1 = point1.z;
			var z2 = point2.z;
			point1.z = z2;
			point2.z = z1;
		}
		foreach( GameObject Unit in allUnitList ) {
			Vector3 UnitPos = Unit.transform.position;
			if (UnitPos.x > point1.x && UnitPos.x < point2.x && UnitPos.z < point1.z && UnitPos.z > point2.z) {
				selectedUnitList.Add(Unit);
				Unit.SendMessage("SetUnitSelected", true);
			}
		}
	}
	private void Test() {
		print("UnitManager: Test!");
	}
	
	private void OnApplicationQuit() {
		instance = null;
	}
}

