using UnityEngine;
using System;

public class WheelSkidTrail_AI : MonoBehaviour 
{
	public enum SkidTrail_Status
	{
		On,
		Off
	}

	[Header("Skid Trail Assignment")]

	public SkidTrail_Status Status;
	public WheelCollider Wc;

	public bool isPlayerCar; // If its player car then stop skid on player car explosion
	public int MySkidID;
	private bool isExploaded;

	[Header("Skid Trail Height")]

	public bool GotSafeHeight;
	private RaycastHit RH;
	public float TrailHeight;
	private Vector3 TempPos;

	[Header("Skid Smoke")]

	public ParticleSystem PS;
	private ParticleSystem.EmissionModule Smoke;
	private ParticleSystem.MinMaxCurve SmokeDensity = new ParticleSystem.MinMaxCurve(); // Just to avoid warning about null value

	public float Interval; // Sets the interval for how frequently it should be checked.
	private float TempInterval;
	public Car_AI CarAI;
	public int SmokeDensity1;
	public int SmokeDensity2;
	public int SmokeDensity3;

	public delegate int DelegateStartNewSkid(Transform SkiPos);
	public static event DelegateStartNewSkid OnStartNewTrail;

	public static Action<int> OnStopCurrentTrail;

	void OnEnable()
	{
		if(isPlayerCar)
		{
			PlayerCarHealth.OnPlayerCarExplosion += OnCarExploded;
		}
	}

	void OnDisable()
	{
		if(isPlayerCar)
		{
			PlayerCarHealth.OnPlayerCarExplosion -= OnCarExploded;
		}
	}

	void Start () 
	{
		Status = SkidTrail_Status.Off;

		if(!Wc)
		{
			return;
		}

		if(PS)
		{
			Smoke = PS.emission;
		}

		// Get ground height

		if(Physics.Raycast(transform.position, Vector3.down, out RH, 30))
		{
			if(RH.collider.gameObject.CompareTag("Ground"))
			{
				GotSafeHeight = true;
				TrailHeight = RH.point.y + 0.02f;
			}
		}
	}

	void Update()
	{
		// Check if the wheel is touching the ground, if not, stop the skid.

		if(Wc.isGrounded)
		{
			if(Status == SkidTrail_Status.Off)
			{
				Status = SkidTrail_Status.On;
				OnWheelTouchedGround();
			}
		}
		else
		{
			if(Status == SkidTrail_Status.On)
			{
				Status = SkidTrail_Status.Off;
				OnWheelLiftedFromGround();
			}
		}

		// Manage the height of this transform little bit above the ground so that skid stays at this height.

		if(!GotSafeHeight) // If the ground height pos is not yet defined somehow then keep detecting it.
		{
			if(Physics.Raycast(transform.position, Vector3.down, out RH, 30))
			{
				if(RH.collider.gameObject.CompareTag("Ground"))
				{
					GotSafeHeight = true;
					TrailHeight = RH.point.y + 0.02f;
				}
			}
		}
		else // Keep the transform above 0.02m to the ground once the ground height is defined.
		{
			TempPos = transform.position;
			TempPos.y = TrailHeight;
			transform.position = TempPos;
		}


		// Keep checking the AdjustSmokeDensity() over intervel.

		TempInterval += Time.deltaTime;

		if(TempInterval >= Interval)
		{
			TempInterval = 0;
			AdjustSmokeDensity();
		}
	}


	// Check the speed and set smoke particles accordingly.

	void AdjustSmokeDensity()
	{
		if(CarAI.Speed > 50)
		{
			SmokeDensity.constant = SmokeDensity3;
		}
		else if(CarAI.Speed <= 50 && CarAI.Speed >= 30)
		{
			SmokeDensity.constant = SmokeDensity2;
		}
		else if(CarAI.Speed < 30)
		{
			SmokeDensity.constant = SmokeDensity1;
		}

		Smoke.rateOverTime = SmokeDensity;
	}


	// Stop skid marks & smoke when car is exploaded.

	void OnCarExploded()
	{
		isExploaded = true;
		OnWheelLiftedFromGround();
	}



	void OnWheelTouchedGround()
	{
		if(PS)
		{
			Smoke.enabled = true;
		}

		if(OnStartNewTrail != null && !isExploaded)
		{
			MySkidID = OnStartNewTrail(transform);
		}
	}

	void OnWheelLiftedFromGround()
	{
		if(PS)
		{
			Smoke.enabled = false;
		}

		if(OnStopCurrentTrail != null)
		{
			OnStopCurrentTrail(MySkidID);
		}
	}
}