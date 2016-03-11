using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CameraTranslation{
	public float edgeArea=25;
	public float speed=30;
	public KeyCode left=KeyCode.A;
	public KeyCode right=KeyCode.D;
	public KeyCode up=KeyCode.W;
	public KeyCode down=KeyCode.S;
}
