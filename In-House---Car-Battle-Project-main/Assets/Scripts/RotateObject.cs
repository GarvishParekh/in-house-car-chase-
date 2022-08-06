using UnityEngine;

public class RotateObject : MonoBehaviour 
{
	public float RotateSpeed;
	private Vector3 TempRot;
	public Transform ObjTransform;

	void Update () 
	{
		TempRot = ObjTransform.eulerAngles;
		TempRot.y -= RotateSpeed * Time.deltaTime;
		ObjTransform.eulerAngles = TempRot;
	}
}
