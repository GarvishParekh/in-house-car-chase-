using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]

public class Car_Player : MonoBehaviour 
{
	[SerializeField] AnimationCurve turnInputCurve;

	[Header("Wheels")]
	[SerializeField] WheelCollider[] driveWheel;
	[SerializeField] WheelCollider[] turnWheel;

	[Header("Behaviour")]
	// Car
	[SerializeField] AnimationCurve motorTorque;
	[SerializeField] float brakeForce = 1500.0f;
	[Range(0f, 50.0f)]
	[SerializeField] float steerAngle = 30.0f;
	[Range(0.001f, 10.0f)]
	[SerializeField] float steerSpeed = 0.2f;

	[SerializeField] Transform centerOfMass;

	// External inputs
	[SerializeField] float steering;
	public float Steering { get{ return steering; } set{ steering = value; } } 

	[SerializeField] float speed = 0.0f;
	public float Speed { get{ return speed; } }

	Rigidbody _rb;
	WheelCollider[] wheels;
	public float TorqueMultiplier = 1;

	[Range(-1.0f, 1.0f)]
	public float InputForward;

	[Range(-1.0f, 1.0f)]
	public float InputSides;

	public enum Input_Type
	{
		Keyboard,
		Screen
	}

	[Header("Classified Input")]

	public Input_Type InputType;

	public bool isLeft;
	public bool isRight;

	public bool isExploded;

	[Header("Turbo Booster")]
	public float BoostForce;
	public int MaxSpeed;
	public bool isBoosting;
	public int BoostMaxSpeed;
	private int CurrentMaxSpeed;
	public float BoostCharage;
	public float BoostDischarge;
	public float BoosterValue;

	public Image BoosterBar;

	public ParticleSystem Booster1;
	public ParticleSystem Booster2;

	[Header ("Breaking Extra Force")]

	public float ExtraDrag;
	private float DefaultDrag;
	public bool isBreaking;

	public static Action OnBoosterOff;
	public static Action<Transform> OnCarSelected;
	private bool isTornadoView;

	void OnEnable()
	{
		DeviceInput.OnRightDown += OnRight_Down;
		DeviceInput.OnRightUp += OnRight_Up;
		DeviceInput.OnLeftDown += OnLeft_Down;
		DeviceInput.OnLeftUp += OnLeft_Up;

		DeviceInput.OnBoostEnable += OnBooster_Enabled;
		DeviceInput.OnBoostDisable += OnBooster_Disabled;

		ActivateGameplayScene.OnGameplaySceneActivated += ActivateCar;
		TornadoThrust.OnTornadoEntered += OnEnteringTornado;
		GroundTrigger.OnCarBackToGround += OnBackToGround;
	}

	void OnDisable()
	{
		DeviceInput.OnRightDown -= OnRight_Down;
		DeviceInput.OnRightUp -= OnRight_Up;
		DeviceInput.OnLeftDown -= OnLeft_Down;
		DeviceInput.OnLeftUp -= OnLeft_Up;

		DeviceInput.OnBoostEnable -= OnBooster_Enabled;
		DeviceInput.OnBoostDisable -= OnBooster_Disabled;

		ActivateGameplayScene.OnGameplaySceneActivated -= ActivateCar;
		TornadoThrust.OnTornadoEntered -= OnEnteringTornado;
		GroundTrigger.OnCarBackToGround -= OnBackToGround;
	}

	void Awake() 
	{
		_rb = GetComponent<Rigidbody>();
		_rb.centerOfMass = centerOfMass.localPosition;
		wheels = GetComponentsInChildren<WheelCollider>();

		if(Application.platform == RuntimePlatform.Android)
		{
			InputType = Input_Type.Screen;
		}

		CurrentMaxSpeed = MaxSpeed;
	}

	void Start()
	{
		_rb.isKinematic = true; // Hold the car physis until the black overlay is active

		if(OnCarSelected != null)
		{
			OnCarSelected(transform);
		}

		if(!BoosterBar)
		{
			BoosterBar = GameObject.Find("BoosterBar").GetComponent<Image>();
		}

		DefaultDrag = _rb.drag;

		OnBooster_Disabled();
	}

	void ActivateCar() // This will be called when the scene will be activated manually
	{
		_rb.isKinematic = false;
	}

	void FixedUpdate () 
	{
		if(!isExploded)
		{
			GetInputs();

			speed = transform.InverseTransformDirection(_rb.velocity).z * 3.6f;
			steering = turnInputCurve.Evaluate(InputSides) * steerAngle;

			foreach (WheelCollider wheel in turnWheel)
			{
				wheel.steerAngle = Mathf.Lerp(wheel.steerAngle, steering, steerSpeed);
			}

			foreach (WheelCollider wheel in wheels)
			{
				wheel.brakeTorque = 0;
			}

			if(speed < 0.1f && speed > -0.1f)
			{
				speed = 0;
			}

			if (Mathf.Abs(speed) < 4 || Mathf.Sign(speed) == Mathf.Sign(InputForward))
			{
				if(speed <= CurrentMaxSpeed)
				{
					foreach (WheelCollider wheel in driveWheel)
					{
						wheel.brakeTorque = 0;
						wheel.motorTorque = InputForward * motorTorque.Evaluate(speed) * 4 * TorqueMultiplier;
					}
				}
				else
				{
					foreach (WheelCollider wheel in driveWheel)
					{
						wheel.brakeTorque = brakeForce * 0.4f;
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

			if(isBoosting)
			{
				if(speed <= BoostMaxSpeed)
				{
					_rb.AddForce(_rb.transform.forward * BoostForce * Time.deltaTime, ForceMode.Impulse);
				}

				BoosterValue -= BoostDischarge * Time.deltaTime;

				if(BoosterValue <= 0)
				{
					OnBooster_Disabled();

					if(OnBoosterOff != null)
					{
						OnBoosterOff();
					}
				}
			}
			else
			{
				if(BoosterValue < 100)
				{
					BoosterValue += BoostCharage * Time.deltaTime;
				}
			}

			if(BoosterBar)
			{
				BoosterBar.fillAmount = BoosterValue * 0.01f;
			}

			if(Speed > 0 && isBreaking && !isTornadoView)
			{
				_rb.drag = ExtraDrag;
			}
			else
			{
				_rb.drag = DefaultDrag;
			}
		}
	}


	void GetInputs()
	{
		if(InputType == Input_Type.Keyboard)
		{
			if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
			{
				isLeft = true;
				isRight = true;

				CheckForBrakes_ApplyExtraResistance();
			}
			else if(Input.GetKey(KeyCode.A))
			{
				isLeft = true;
				isRight = false;

				CheckForBrakes_ApplyExtraResistance();
			}
			else if(Input.GetKey(KeyCode.D))
			{
				isLeft = false;
				isRight = true;

				CheckForBrakes_ApplyExtraResistance();
			}
			else
			{
				isLeft = false;
				isRight = false;

				CheckForBrakes_ApplyExtraResistance();
			}
		}

		if(isLeft && isRight)
		{
			InputForward = Mathf.MoveTowards(InputForward, -1.0f, Time.deltaTime * 8.0f);
		}
		else if(!isLeft && isRight)
		{
			InputForward = Mathf.MoveTowards(InputForward, 1.0f, Time.deltaTime * 8.0f);
			InputSides = Mathf.MoveTowards(InputSides, 1.0f, Time.deltaTime * 8.0f);
		}
		else if(isLeft && !isRight)
		{
			InputForward = Mathf.MoveTowards(InputForward, 1.0f, Time.deltaTime * 8.0f);
			InputSides = Mathf.MoveTowards(InputSides, -1.0f, Time.deltaTime * 8.0f);
		}
		else
		{
			InputForward = Mathf.MoveTowards(InputForward, 1, Time.deltaTime * 10.0f);
			InputSides = Mathf.MoveTowards(InputSides, 0, Time.deltaTime * 10.0f);
		}
	}




	void OnBooster_Enabled()
	{
		if(BoosterValue > 0)
		{
			isBoosting = true;
			CurrentMaxSpeed = BoostMaxSpeed;

			if(Booster1)
			{
				Booster1.Play();
			}

			if(Booster2)
			{
				Booster2.Play();
			}
		}
	}

	void OnBooster_Disabled()
	{
		isBoosting = false;
		CurrentMaxSpeed = MaxSpeed;

		if(Booster1)
		{
			Booster1.Stop();
		}

		if(Booster2)
		{
			Booster2.Stop();
		}
	}

	void OnLeft_Down()
	{
		isLeft = true;
		CheckForBrakes_ApplyExtraResistance();
	}

	void OnLeft_Up()
	{
		isLeft = false;
		CheckForBrakes_ApplyExtraResistance();
	}

	void OnRight_Down()
	{
		isRight = true;
		CheckForBrakes_ApplyExtraResistance();
	}

	void OnRight_Up()
	{
		isRight = false;
		CheckForBrakes_ApplyExtraResistance();
	}


	void CheckForBrakes_ApplyExtraResistance()
	{
		if(isLeft && isRight)
		{
			isBreaking = true;
		}
		else
		{
			isBreaking = false;
		}
	}


	public void Freeze_Me() // Not being used ATM
	{
		foreach (WheelCollider wheel in wheels)
		{
			wheel.motorTorque = 0;
			wheel.brakeTorque = Mathf.Abs(InputForward) * brakeForce;
		}
	}

	void OnEnteringTornado()
	{
		isTornadoView = true;
	}

	void OnBackToGround()
	{
		isTornadoView = false;
	}
}
