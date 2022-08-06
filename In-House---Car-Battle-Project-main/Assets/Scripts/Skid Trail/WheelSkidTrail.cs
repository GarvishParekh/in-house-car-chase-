using UnityEngine;
using System;

public class WheelSkidTrail : MonoBehaviour 
{
	public enum Wheel_Status
	{
		Grounded,
		InAir
	}

	[Header("Skid Trail Assignment")]

	public Wheel_Status WheelStatus;
	public WheelCollider Wc;

	public bool isPlayerCar; // If its player car then stop skid on player car explosion
	private bool isExploaded;
	private float TrailTime = 5.0f;

	[Header("Skid Trail Height")]

	private RaycastHit RH;
	private Vector3 TempPos;

	[Header("Skid Smoke")]

	public ParticleSystem PS;
	private ParticleSystem.EmissionModule Smoke;
	private ParticleSystem.MinMaxCurve SmokeDensity = new ParticleSystem.MinMaxCurve(); // Just to avoid warning about null value

	public float Interval; // Sets the interval for how frequently it should be checked.
	private float TempInterval;
	public Car_Player PlayerCar;
	public int SmokeDensity1;
	public int SmokeDensity2;
	public int SmokeDensity3;

	public delegate SkidTrailFollow DelegateStartNewSkid();
	public static event DelegateStartNewSkid OnStartNewTrail;

	public SkidTrailFollow MyTrail;

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
		transform.SetParent(null); // Remove them from car parent so that they dont get affectd by car's transform behavior

		WheelStatus = Wheel_Status.InAir;

		if(!Wc)
		{
			return;
		}

		if(PS)
		{
			Smoke = PS.emission;
		}
	}

	void Update()
	{
		// Check if the wheel is touching the ground, if not, stop the skid.

		if(Wc.isGrounded)
		{
			if(Physics.Raycast(Wc.transform.position, Vector3.down, out RH, 5))
			{
				if(RH.collider.gameObject.CompareTag("Ground"))
				{
					// Set the transform Y position to 0.01 meter above the ground

					TempPos = Wc.transform.position;
					TempPos.y = RH.point.y + 0.01f;
					transform.position = TempPos;
				}
			}

			if(WheelStatus == Wheel_Status.InAir)
			{
				OnWheelTouchedGround();
			}
		}
		else
		{
			if(WheelStatus == Wheel_Status.Grounded)
			{
				OnWheelLiftedFromGround();
			}
		}

		// Keep checking the AdjustSmokeDensity() over intervel.

		TempInterval += Time.deltaTime;

		if(TempInterval >= Interval)
		{
			TempInterval = 0;
			AdjustSmokeDensity();
		}
	}

	void OnWheelTouchedGround()
	{
		WheelStatus = Wheel_Status.Grounded;

		if(PS)
		{
			PS.Play();
		}

		// Ensure if there is already a skid trail assigned, unassign it & make it free.

		if(MyTrail && MyTrail.TrailStatus == SkidTrailFollow.Trail_Status.Occupied)
		{
			MyTrail.EndTrail();
		}

		// Assign a new trail to this wheel

		if(OnStartNewTrail != null && !isExploaded)
		{
			MyTrail = OnStartNewTrail();

			if(MyTrail)
			{
				MyTrail.StartTrail(transform, TrailTime);
			}
		}
	}

	void OnWheelLiftedFromGround()
	{
		WheelStatus = Wheel_Status.InAir;

		if(PS)
		{
			PS.Stop();
		}

		if(MyTrail)
		{
			MyTrail.EndTrail();
		}
	}



	// Check the speed and set smoke particles accordingly.

	void AdjustSmokeDensity()
	{
		if(PlayerCar.Speed > 50)
		{
			SmokeDensity.constant = SmokeDensity3;
		}
		else if(PlayerCar.Speed <= 50 && PlayerCar.Speed >= 30)
		{
			SmokeDensity.constant = SmokeDensity2;
		}
		else if(PlayerCar.Speed < 30)
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
}