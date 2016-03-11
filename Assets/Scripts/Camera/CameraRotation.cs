using UnityEngine;
using System.Collections;

public enum MouseButton:int{
	Left=0,
	Right=1,
	Middle=2
}

[System.Serializable]
public class CameraRotation{
	public bool horizontal=true;
	public bool vertical=true;
	public MouseButton mouseButton=MouseButton.Right;
	public float speed=5;
	public float yMinLimit = -80; 
    public float yMaxLimit = 80; 
	public Vector2 acceleration= new Vector2(200,200);
}

