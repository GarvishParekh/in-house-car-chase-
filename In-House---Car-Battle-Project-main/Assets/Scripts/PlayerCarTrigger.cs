using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCarTrigger : MonoBehaviour 
{
	public bool isExploded;
	private int CollisionImpact;

	public static Action<int> OnPlayerCarDamage;
	public static Action<Vector3> OnPlayerCarCollision;

	private float DurationGround = 1.0f;
	private float DurationAI = 0.4f;
	private float DurationEnvironment = 1.0f;

	private bool isGroundEnabled;
	private bool isAIEnabled;
	private bool isEnvironmentEnabled;

	private float TempGround;
	private float TempAI;
	private float TempEnvironment;

	public static Action OnPlayerGroundHit = delegate{};

	void OnEnable()
	{
		PlayerCarHealth.OnPlayerCarExplosion += OnPlayerCarExploaded;
	}

	void OnDisable()
	{
		PlayerCarHealth.OnPlayerCarExplosion -= OnPlayerCarExploaded;
	}


	void Start()
	{
		isGroundEnabled = true;
		isAIEnabled = true;
		isEnvironmentEnabled = true;
	}

	void Update()
	{
		if(!isAIEnabled)
		{
			TempAI += Time.deltaTime;

			if(TempAI > DurationAI)
			{
				TempAI = 0;
				isAIEnabled = true;
			}
		}

		if(!isEnvironmentEnabled)
		{
			TempEnvironment += Time.deltaTime;

			if(TempEnvironment > DurationEnvironment)
			{
				TempEnvironment = 0;
				isEnvironmentEnabled = true;
			}
		}

		if(!isGroundEnabled)
		{
			TempGround += Time.deltaTime;

			if(TempGround > DurationGround)
			{
				TempGround = 0;
				isGroundEnabled = true;
			}
		}
	}


	void OnCollisionEnter(Collision C)
	{
		if(!isExploded)
		{
			if(C.collider.CompareTag("Car_AI") && isAIEnabled)
			{
				if(!C.gameObject.GetComponent<Car_AI>().isExploded) // Only damage if AI Car is not exploaded
				{
					CollisionImpact = (int)C.relativeVelocity.magnitude;

					if(C.contacts.Length > 0) // Raise event so that the CollisionParticle script can show sparks at this point
					{
						ShowSpartsAt(C.contacts[0].point);
					}

					if(CollisionImpact > 2) // Neglact very low collision hits
					{
						GettingDamage(CollisionImpact);
					}

					isAIEnabled = false;
				}
			}
			else if(C.collider.CompareTag("Environment") && isEnvironmentEnabled)
			{
				CollisionImpact = (int)C.relativeVelocity.magnitude;

				if(C.contacts.Length > 0) // Raise event so that the CollisionParticle script can show sparks at this point
				{
					ShowSpartsAt(C.contacts[0].point);
				}

				if(CollisionImpact > 2) // Neglact very low collision hits
				{
					GettingDamage(CollisionImpact);
				}

				isEnvironmentEnabled = false;
			}
			else if(C.collider.CompareTag("Ground") && isGroundEnabled)
			{
				CollisionImpact = (int) (C.relativeVelocity.magnitude * 0.5f);

				if(CollisionImpact > 2) // Neglact very low collision hits
				{
					GettingDamage(CollisionImpact);
				}

				OnPlayerGroundHit();

				isGroundEnabled = false;
			}
		}
	}

	void ShowSpartsAt(Vector3 P)
	{
		if(OnPlayerCarCollision != null)
		{
			OnPlayerCarCollision(P);
		}
	}

	void GettingDamage(int Points)
	{
		if(OnPlayerCarDamage != null)
		{
			OnPlayerCarDamage(CollisionImpact);
		}
	}

	void OnPlayerCarExploaded()
	{
		isExploded = true;
	}
}