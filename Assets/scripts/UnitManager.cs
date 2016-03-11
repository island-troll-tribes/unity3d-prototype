using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour
{
	private static UnitManager instance;

	public static UnitManager Instance {
		get{ return instance;}
	}
	
	public Color selectionColor = Color.green;
	public Texture selectionTexture;
	private List<GameObject> allUnits = new List<GameObject> ();
	private List<GameObject> selectedUnits = new List<GameObject> ();
	private Vector3 bottomRight;
	private Vector3 bottomLeft;
	private Vector3 topRight;
	private Vector3 topLeft;
	private Vector2 mouseButton1DownPoint;
	private Vector2 mouseButton1UpPoint;
	private bool mouseDrag;
	
	private void Awake ()
	{
		instance = this;
	}
	
	public void SelectUnit (GameObject unit)
	{
		foreach (GameObject go in allUnits) {
			if (go.GetInstanceID () == unit.GetInstanceID ()) {
				DeselectUnits ();
				if (!selectedUnits.Contains (go)) {
					selectedUnits.Add (go);
				}
			}
		}
	}
	
	public void DeselectUnits ()
	{
		foreach (GameObject go in selectedUnits) {
			SetColor (go.transform, Color.white);
		}
		selectedUnits.Clear ();
	}
	
	private void SetColor (Transform go, Color color)
	{
		foreach (Transform tr in go.GetComponentsInChildren<Transform>()) {
			if (tr.GetComponent<Renderer> () != null) {
				tr.GetComponent<Renderer>().material.color = color;
			}
		}
	}
	
	public void AddUnitToAllUnits (GameObject go)
	{
		if (!allUnits.Contains (go)) {
			allUnits.Add (go);
		}
	}
	
	private void OnGUI ()
	{
		if (mouseDrag) {
			float width = mouseButton1UpPoint.x - mouseButton1DownPoint.x;
			float height = (Screen.height - mouseButton1UpPoint.y) - (Screen.height - mouseButton1DownPoint.y);
			Rect rect = new Rect (mouseButton1DownPoint.x, Screen.height - mouseButton1DownPoint.y, width, height);
			GUI.DrawTexture (rect, selectionTexture, ScaleMode.StretchToFill, true);
		}
	}

	private void Update ()
	{
		if (Input.GetButtonDown ("Fire1")) {
			RTSCamera.Instance.killInput = true;
			MouseDown (Input.mousePosition);
		}
	
		if (Input.GetButtonUp ("Fire1")) {
			RTSCamera.Instance.killInput = false;
			MouseUp (Input.mousePosition);
			
		}
	
		if (Input.GetButton ("Fire1")) {
			MouseDrag (Input.mousePosition);	
		}
		
		if (Input.GetMouseButtonDown (1)) {
//			GameObject centerUnit = GetNearestUnitToCenter ();
			int cnt = selectedUnits.Count;
			// Lets define some variables next 
			float Separation = 0;                // How far should the units be from the central units 
			float AngleStep = 360f / cnt;
			
			for (int i = 0; i < cnt; i++) { 
				//if(selectedUnits[i] != centerUnit)
				selectedUnits [i].GetComponent<Unit> ().Move(GetScreenRaycastPoint (Input.mousePosition) + new Vector3 (Mathf.Sin (Mathf.Deg2Rad * (AngleStep * i)) * Separation, 0, Mathf.Cos (Mathf.Deg2Rad * (AngleStep * i)) * Separation));   
				//selectedUnits[i].GetComponent<Unit>().Move(GetScreenRaycastPoint(Input.mousePosition)) ;
				
			} 
			
			/*int numColumns = Mathf.FloorToInt (Mathf.Sqrt (cnt));  
			if (numColumns * numColumns < cnt) { 
				numColumns++;  
			}
			
			List<Vector3> spots= new List<Vector3>();
			for(int x=0; x< numColumns; x++){
				for(int y=0; y< numColumns;y++ ){
				   Vector3 offset =new Vector3( ( x*numColumns)  ,0 , ( Mathf.Floor( y / numColumns)*2));
					spots.Add(offset);
				}
			}
			
			for(int i=0; i<spots.Count; i++){
				foreach(GameObject go in selectedUnits){
					Unit u= go.GetComponent<Unit>();
					go.GetComponent<NavMeshAgent> ().destination = GetScreenRaycastPoint (Input.mousePosition)+spots[i];
				}
			}*/
		}
	}
	
	private void MouseDrag (Vector2 screenPosition)
	{
		if (screenPosition != mouseButton1DownPoint) {
			mouseDrag = true;
			mouseButton1UpPoint = screenPosition;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (screenPosition); 		
			if (Physics.Raycast (ray, out hit)) { 
				bottomRight = hit.point;
				bottomLeft = GetScreenRaycastPoint (new Vector2 (mouseButton1UpPoint.x + (mouseButton1DownPoint.x - mouseButton1UpPoint.x), mouseButton1UpPoint.y));
				topRight = GetScreenRaycastPoint (new Vector2 (mouseButton1UpPoint.x, mouseButton1UpPoint.y - ((Screen.height - mouseButton1DownPoint.y) - (Screen.height - mouseButton1UpPoint.y))));
				SelectUnitsInArea ();
			}	
		}
	}

	private void MouseDown (Vector2 screenPosition)
	{
		mouseButton1DownPoint = screenPosition;
		RaycastHit hit;
		DeselectUnits ();
		Ray ray = Camera.main.ScreenPointToRay (mouseButton1DownPoint); 
		if (Physics.Raycast (ray, out hit)) { 
			topLeft = hit.point;
		}
	}
		
	private void MouseUp (Vector2 screenPosition)
	{
		mouseButton1UpPoint = screenPosition;
		mouseDrag = false;
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (mouseButton1DownPoint);
		if (Physics.Raycast (ray, out hit)) { 
			SelectUnit (hit.transform.gameObject);	
		}
	}
	
	private Vector3 GetScreenRaycastPoint (Vector2 screenPosition)
	{
		RaycastHit hit;
		Physics.Raycast (Camera.main.ScreenPointToRay (screenPosition), out hit, 1000);

		return hit.point;

	}

	private void SelectUnitsInArea ()
	{
		Vector3[] poly = new Vector3[4];

		poly [0] = topLeft;

		poly [1] = topRight;

		poly [2] = bottomRight;

		poly [3] = bottomLeft;

		foreach (GameObject go in allUnits) {
			if (IsPointInPoly (poly, go.transform.position)) {
				if (!selectedUnits.Contains (go)) {
					SetColor (go.transform, selectionColor);
					selectedUnits.Add (go);
					
				}
			} else {
				SetColor (go.transform, Color.white);
				selectedUnits.Remove (go);
			}

		}

	}

	private bool IsPointInPoly (Vector3[] poly, Vector3 pt)
	{
		bool c = false;
		int l = 4;
		int j = l - 1;

		for (int i = -1; ++i < l; j = i) {

			if (((poly [i].z <= pt.z && pt.z < poly [j].z) || (poly [j].z <= pt.z && pt.z < poly [i].z))

            && (pt.x < (poly [j].x - poly [i].x) * (pt.z - poly [i].z) / (poly [j].z - poly [i].z) + poly [i].x))

				c = !c;
		}
		return c;
	}
	
	public float GetAverageDistanceToCenterOfSelectedUnits ()
	{
		Vector3 center = GetCenterOfSelectedUnits ();
		float dist = 0;
		foreach (GameObject go in selectedUnits) {
			dist += Vector3.Distance (go.transform.position, center);
		}
		dist /= selectedUnits.Count;
		return dist;
	}
	
	public Vector3 GetCenterOfSelectedUnits ()
	{
		Vector3 center = Vector3.zero;
		foreach (GameObject go in selectedUnits) {
			center += go.transform.position;
		}
		center /= selectedUnits.Count;
		return center;
	}
	
	public GameObject GetNearestUnitToCenter ()
	{
		Vector3 center = GetCenterOfSelectedUnits ();
		
		GameObject nearestToCenter = null;
		float nearestDistanceToCenter = Mathf.Infinity;
		
		foreach (GameObject go in selectedUnits) {
			float curDist = Vector3.Distance (go.transform.position, center);
			if (curDist < nearestDistanceToCenter) {
				nearestDistanceToCenter = curDist;
				nearestToCenter = go;
			}
		}
		return nearestToCenter;
		
	}
}
