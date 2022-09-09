using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Car_AI : MonoBehaviour 
{
	[Header("Wheels")]
	[SerializeField] WheelCollider[] driveWheel;
	[SerializeField] WheelCollider[] turnWheel;

	[Header("Behaviour")]
	// Car
	[SerializeField] AnimationCurve motorTorque;
	[SerializeField] float brakeForce = 1500.0f;
	[Range(5f, 50.0f)]
	[SerializeField] float steerAngle = 30.0f;
	[Range(0.04f, 1.0f)]
	[SerializeField] float steerSpeed = 0.2f;

	[SerializeField] Transform centerOfMass;

	// External inputs
	float steering;
	private float Steering { get{ return steering; } set{ steering = value; } } 

	public float Speed;

	Rigidbody _rb;
	public WheelCollider[] wheels;
	public float TorqueMultiplier = 1;

	[Range(-1.0f, 1.0f)]
	public float InputForward;

	public bool isExploded;

	public Transform LookVector;
	private Transform Target;

	public float CalculatedAngle;

	public float Distance;

	public int MaxSpeed;

	[SerializeField]
	bool isPrefab = true;

	void OnEnable()
	{
		PlayerCarHealth.OnPlayerCarExplosion += Stop_AI_Behavior;
		Car_Player.OnCarSelected += SetTargetPlayerCar;

		ActivateGameplayScene.OnGameplaySceneActivated += ActivateRigidbody;
	}

	void OnDisable()
	{
		Car_Player.OnCarSelected -= SetTargetPlayerCar;
		PlayerCarHealth.OnPlayerCarExplosion -= Stop_AI_Behavior;

		ActivateGameplayScene.OnGameplaySceneActivated -= ActivateRigidbody;
	}

	void Awake() 
	{
		_rb = GetComponent<Rigidbody>();
		_rb.centerOfMass = centerOfMass.localPosition;
	}

	void Start()
	{
		if (!isPrefab)
		{
			_rb.isKinematic = true;
		}
		if (Target == null)
			Target = GameObject.FindGameObjectWithTag("Player").transform;
	}

	
	void ActivateRigidbody()
	{
		_rb.isKinematic = false;
	}

	void FixedUpdate () 
	{
		if(!isExploded)
		{
			Speed = transform.InverseTransformDirection(_rb.velocity).z * 3.6f;

			foreach (WheelCollider wheel in turnWheel)
			{
				wheel.steerAngle = Mathf.LerpAngle(wheel.steerAngle, steering, steerSpeed);
			}

			foreach (WheelCollider wheel in wheels)
			{
				wheel.brakeTorque = 0;
			}

			if(Speed < 0.1f && Speed > -0.1f)
			{
				Speed = 0;
			}

			if (Mathf.Abs(Speed) < 4 || Mathf.Sign(Speed) == Mathf.Sign(InputForward))
			{
				if(Speed <= MaxSpeed)
				{
					foreach (WheelCollider wheel in driveWheel)
					{
						wheel.brakeTorque = 0;
						wheel.motorTorque = InputForward * motorTorque.Evaluate(Speed) * 4 * TorqueMultiplier;
					}
				}
				else
				{
					foreach (WheelCollider wheel in driveWheel)
					{
						wheel.brakeTorque = 0;
						wheel.motorTorque = 0;
					}
				}
			}
			else
			{
				foreach (WheelCollider wheel in wheels)
				{
					wheel.motorTorque = 0;
					wheel.brakeTorque = Mathf.Abs(InputForward) * brakeForce;
				}
			}
		}
	}

	void Update()
	{
		if(Target && !isExploded)
		{
			LookVector.LookAt(Target.position);

			if(LookVector.localEulerAngles.y < 180)
			{
				CalculatedAngle = LookVector.localEulerAngles.y;
			}
			else
			{
				CalculatedAngle = (360 - LookVector.localEulerAngles.y) * -1;
			}

			if(CalculatedAngle > 0)
			{
				if(CalculatedAngle <= steerAngle)
				{
					Steering = CalculatedAngle;
				}
				else
				{
					Steering = steerAngle;
				}
			}
			else
			{
				if(CalculatedAngle >= -steerAngle)
				{
					Steering = CalculatedAngle; 
				}
				else
				{
					Steering = - steerAngle;
				}
			}

			Distance = Vector3.Distance(transform.position, Target.position);

			if(Distance <= 5)
			{
				//steering = steerAngle;
				InputForward = -0.5f;
			}
			else
			{
				InputForward = 1;
			}
		}
	}

	void SetTargetPlayerCar(Transform T)
	{
		Target = T;
	}

	public void Stop_AI_Behavior()
	{
		foreach (WheelCollider wheel in wheels)
		{
			wheel.motorTorque = 0;
			this.enabled = false;

			wheel.brakeTorque = Mathf.Abs(InputForward) * brakeForce * 0.2f;
		}
	}
}
