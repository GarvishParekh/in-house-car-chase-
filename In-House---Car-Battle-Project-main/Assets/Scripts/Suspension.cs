using UnityEngine;

[RequireComponent(typeof(WheelCollider))]

public class Suspension : MonoBehaviour 
{
	Vector3 pos;
	Quaternion quat;
	public GameObject _wheelModel;
	private WheelCollider _wheelCollider;

	void Start()
	{
		_wheelCollider = GetComponent<WheelCollider>();
	}

	void FixedUpdate()
	{
		_wheelCollider.GetWorldPose(out pos, out quat);

		_wheelModel.transform.rotation = quat;
		_wheelModel.transform.position = pos;
	}
}