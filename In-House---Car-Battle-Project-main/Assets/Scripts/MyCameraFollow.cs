using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyCameraFollow : MonoBehaviour 
{
	public Transform Target;
	public bool isTargetAssigned;
	public bool isTornadoView;

	[Header("Camera Follow")]
	public float Height;
	public float Distance;
	public float RotationDamping;
	public float HeightDamping;

	private float DefaultRotSpeed;

	[Header("Camera Switch")]

	public float HeightTopDown;
	public float HeightRacing;
	public float DistanceTopDown;
	public float DistanceRacing;

	public enum Cam_Positions
	{
		Top_Down,
		Racing
	}

	public Cam_Positions CamPosition;
	public bool LockRotationTopDown;

	[Header("Booster Effect")]

	public Camera CamMain;

	public bool isBoosting;
	public float FOV_Speed;
	public float FOV_Increament;

	private float TargetFOV;
	private float DefaultFOV;
	private float BoostFOV;

	private Vector3 NewEur;
	private Vector3 TempPos;

	public static Action<Cam_Positions> OnCameraViewChanged;

	void OnEnable()
	{
		Car_Player.OnCarSelected += OnCarSelected;
		DeviceInput.OnBoostEnable += OnBoost_ON;
		DeviceInput.OnBoostDisable += OnBoost_OFF;
		Car_Player.OnBoosterOff += OnBoost_OFF;
		PlayerCarHealth.OnPlayerCarExplosion += OnPlayerCarExploaded;
		TornadoThrust.OnTornadoEntered += OnTornadoCamEnter;
		GroundTrigger.OnCarBackToGround += OnCarBackToGround;
		PlayerCarTrigger.OnPlayerGroundHit += OnCarBackToGround;
	}

	void OnDisable()
	{
		Car_Player.OnCarSelected -= OnCarSelected;
		DeviceInput.OnBoostEnable -= OnBoost_ON;
		DeviceInput.OnBoostDisable -= OnBoost_OFF;
		Car_Player.OnBoosterOff -= OnBoost_OFF;
		PlayerCarHealth.OnPlayerCarExplosion -= OnPlayerCarExploaded;
		TornadoThrust.OnTornadoEntered -= OnTornadoCamEnter;
		GroundTrigger.OnCarBackToGround -= OnCarBackToGround;
		PlayerCarTrigger.OnPlayerGroundHit -= OnCarBackToGround;
	}


	void OnCarSelected(Transform T)
	{
		Target = T;
		isTargetAssigned = true;
	}

	void Start()
	{
		CamMain = GetComponent<Camera>();

		DefaultRotSpeed = RotationDamping;
		DefaultFOV = CamMain.fieldOfView;
		BoostFOV = DefaultFOV + FOV_Increament;

		TargetFOV = DefaultFOV;
		Set_Racing();
	}

	void LateUpdate ()
	{
		if(isTargetAssigned)
		{
			if(!isTornadoView)
			{
				float wantedRotationAngle = Target.eulerAngles.y;
				float wantedHeight = Target.position.y + Height;

				float currentRotationAngle = transform.eulerAngles.y;
				float currentHeight = transform.position.y;

				// Damp the rotation around the y-axis
				currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, RotationDamping * Time.deltaTime);

				// Damp the height
				currentHeight = Mathf.Lerp (currentHeight, wantedHeight, HeightDamping * Time.deltaTime);

				// Convert the angle into a rotation
				Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);

				// Set the position of the camera on the x-z plane to:
				// distance meters behind the Target
				transform.position = Target.position;
				transform.position -= currentRotation * Vector3.forward * Distance;

				// Set the height of the camera
				TempPos = transform.position;
				TempPos.y = currentHeight;
				transform.position = TempPos;

				//transform.position = new Vector3(transform.position.x, , transform.position.z);

				// Always look at the Target
				transform.LookAt (Target);

				CamMain.fieldOfView = Mathf.Lerp(CamMain.fieldOfView, TargetFOV, FOV_Speed * Time.deltaTime);
			}
			else
			{
				transform.LookAt(Target);
			}
		}
	}



	public void SwitchCamera()
	{
		if(CamPosition == Cam_Positions.Racing)
		{
			Set_TopDown();
		}
		else
		{
			Set_Racing();
		}
	}

	void Set_TopDown()
	{
		CamPosition = Cam_Positions.Top_Down;

		Height = HeightTopDown;
		Distance = DistanceTopDown;

		if(LockRotationTopDown)
		{
			RotationDamping = 0;

			NewEur = CamMain.transform.eulerAngles;
			NewEur.y = 0;
			NewEur.z = 0;
			CamMain.transform.eulerAngles = NewEur;
		}

		if(OnCameraViewChanged != null)
		{
			OnCameraViewChanged(Cam_Positions.Top_Down);
		}
	}

	void Set_Racing()
	{
		CamPosition = Cam_Positions.Racing;

		Height = HeightRacing;
		Distance = DistanceRacing;

		RotationDamping = DefaultRotSpeed;

		if(OnCameraViewChanged != null)
		{
			OnCameraViewChanged(Cam_Positions.Racing);
		}
	}



	void OnBoost_ON()
	{
		isBoosting = true;
		TargetFOV = BoostFOV;
	}

	void OnBoost_OFF()
	{
		isBoosting = false;
		TargetFOV = DefaultFOV;
	}


	void OnPlayerCarExploaded() // Dont let camera aligned with cars rotation as car will rotate fast randomly in multiple direction.
	{
		RotationDamping = 0;
	}

	void OnTornadoCamEnter()
	{
		isTornadoView = true;
	}

	void OnCarBackToGround()
	{
		isTornadoView = false;
	}
}