using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour {
private static RTSCamera instance;
	
	public static RTSCamera Instance{
		get{return instance;}
	}
	
	public RTSCameraSettings settings;
    private float xDeg = 0.0f; 
    private float yDeg = 0.0f; 
	private RaycastHit hit;
	[HideInInspector]
	public bool canRotate=true;
	[HideInInspector]
	public bool killInput=false;
	
	private void Awake(){
		instance=this;
	}
	
	void Start(){
		Vector3 angles = transform.eulerAngles; 
        xDeg = angles.y; 
        yDeg = angles.x; 
		yDeg = ClampAngle (yDeg, settings.rotation.yMinLimit, settings.rotation.yMaxLimit); 

		Quaternion rotation = Quaternion.Euler (yDeg, xDeg, 0); 
		transform.rotation=rotation;
	}
	
	
    void Update()
    {
		if(killInput){
			return;
		}
		
		if(settings.zoom.enabled) {
			Vector3 translation = Vector3.zero;
        	float zoomDelta = Input.GetAxis("Mouse ScrollWheel")*settings.zoom.speed*Time.deltaTime;
        	
			if (zoomDelta != 0){
            	translation -= Vector3.up * settings.zoom.speed * zoomDelta;
        	}

			if(Physics.Raycast(transform.position,Vector3.down,out hit)){
				transform.position += translation;	
				if(zoomDelta == 0){
					if(hit.distance<settings.zoom.min){
						transform.Translate(Vector3.up*settings.zoom.speed*Time.deltaTime,Space.World);	
					}
					
					if(hit.distance>settings.zoom.max){
						transform.Translate(-Vector3.up*settings.zoom.speed*Time.deltaTime,Space.World);	
					}
				}else{
					float restoreY=transform.position.y;
					restoreY=Mathf.Clamp(restoreY,hit.point.y+settings.zoom.min,hit.point.y+settings.zoom.max);
   		   			transform.position = new Vector3( transform.position.x,restoreY, transform.position.z);
				}
			}else{
				transform.Translate(Vector3.up*settings.zoom.speed*Time.deltaTime,Space.World);
			}
		}
		
		if(Input.GetMouseButton((int)settings.rotation.mouseButton) && canRotate) {	
			if(settings.rotation.horizontal){
				xDeg += Input.GetAxis ("Mouse X") * settings.rotation.acceleration.x * 0.02f;  
			}
			
			if(settings.rotation.vertical){
				yDeg -= Input.GetAxis ("Mouse Y") * settings.rotation.acceleration.y * 0.02f; 
				yDeg = ClampAngle (yDeg, settings.rotation.yMinLimit, settings.rotation.yMaxLimit); 
			}
			Quaternion rotation = Quaternion.Euler (yDeg, xDeg, 0); 
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation,Time.deltaTime*settings.rotation.speed); 
			return;
		}
		

    	float mPosX = Input.mousePosition.x;
    	float mPosY = Input.mousePosition.y;
		float y=transform.position.y;
		
    	if (Input.GetKey("left") || Input.GetKey(settings.translation.left) || mPosX < settings.translation.edgeArea) {
			transform.Translate(Vector3.right * -settings.translation.speed* Time.deltaTime, Space.Self);
		}
    	if (Input.GetKey("right") || Input.GetKey(settings.translation.right) || mPosX >= Screen.width-settings.translation.edgeArea){
			transform.Translate(Vector3.right * settings.translation.speed * Time.deltaTime, Space.Self);
		}
   		if (Input.GetKey("down") || Input.GetKey(settings.translation.down) || mPosY < settings.translation.edgeArea){
			transform.Translate(Vector3.forward * -settings.translation.speed*(transform.eulerAngles.x/20) * Time.deltaTime, Space.Self);
		}
   	 	if (Input.GetKey("up") || Input.GetKey(settings.translation.up) || mPosY >= Screen.height-settings.translation.edgeArea){
			transform.Translate(Vector3.forward * settings.translation.speed *(transform.eulerAngles.x/20)* Time.deltaTime, Space.Self);
		}	
		
		transform.position = new Vector3( transform.position.x,y, transform.position.z);
		
    }

	private static float ClampAngle (float angle, float min, float max) 
    { 
        if (angle < -360) 
            angle += 360; 
        if (angle > 360) 
            angle -= 360; 
        return Mathf.Clamp (angle, min, max); 
    }
}
