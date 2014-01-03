using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
	public GameObject healthBar;
	public GameObject background;
	// Use this for initialization
	void Start ()
	{
		transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, 
		                 Camera.main.transform.rotation * Vector3.up);	
	}
	
	public void SetHealthEnabled(bool enabled) {
		//print("SetHealthEnabled: " + enabled);
		/*
		gameObject.active = enabled;
		background.active = enabled;
		healthBar.active = enabled;
		*/
		transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, 
		                 Camera.main.transform.rotation * Vector3.up);
	}
}

