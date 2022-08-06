using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class PlayerCarExplosion : MonoBehaviour 
{
	[Header("Explosion Configuration")]

	public float ExplosionPower;
	public float ExplosionRange;
	public Transform ExplosionPos;
	public ForceMode FM;

	[Header("Objects Deteachment")]

	public WheelCollider WC_FL;
	public WheelCollider WC_FR;
	public WheelCollider WC_RL;
	public WheelCollider WC_RR;

	public Rigidbody W_Mesh_FL;
	public Rigidbody W_Mesh_FR;
	public Rigidbody W_Mesh_RL;
	public Rigidbody W_Mesh_RR;

	public Rigidbody W_Mesh_Extra1;
	public Rigidbody W_Mesh_Extra2;

	public Transform COM_Explosion;

	public float WheelForceMultiplier;

	private Rigidbody _RB;
	private Car_Player PlayerCar;

	[Header("Explosion Effect")]

	public ParticleSystem ParticleFire;
	public Renderer[] BodyParts;
	public Color ColDark;

	private bool SetTimeScale;
	private bool SetDragValue;

	private float DelayDrag = 0.3f;
	private float DelayTimeScale = 0.6f;

	void OnEnable()
	{
		PlayerCarHealth.OnPlayerCarExplosion += OnExplosion;
	}

	void OnDisable()
	{
		PlayerCarHealth.OnPlayerCarExplosion -= OnExplosion;
	}

	void Start()
	{
		_RB = GetComponent<Rigidbody>();
		PlayerCar = GetComponent<Car_Player>();

		if(W_Mesh_FL.GetComponent<MeshCollider>())
		{
			W_Mesh_FL.GetComponent<MeshCollider> ().enabled = false;
		}

		if(W_Mesh_FR.GetComponent<MeshCollider>())
		{
			W_Mesh_FR.GetComponent<MeshCollider> ().enabled = false;
		}

		if(W_Mesh_RL.GetComponent<MeshCollider>())
		{
			W_Mesh_RL.GetComponent<MeshCollider> ().enabled = false;
		}

		if(W_Mesh_RR.GetComponent<MeshCollider>())
		{
			W_Mesh_RR.GetComponent<MeshCollider> ().enabled = false;
		}
	}

	void Update()
	{
		if(SetTimeScale)
		{
			DelayTimeScale -= Time.deltaTime;

			if(DelayTimeScale < 0)
			{
				SetTimeScale = false;
				NormalTImeNOw();
			}
		}

		if(SetDragValue)
		{
			DelayDrag -= Time.deltaTime;

			if(DelayDrag < 0)
			{
				SetDragValue = false;
				NormalDragNow();
			}
		}
	}

	public void OnExplosion () 
	{
		if(!PlayerCar.isExploded)
		{
			PlayerCar.isExploded = true;
			_RB.drag = 1.2f;
			_RB.centerOfMass = COM_Explosion.localPosition;

			_RB.AddExplosionForce(ExplosionPower * 1.3f, ExplosionPos.position, ExplosionRange, 1.0f, FM);

			DeteachWheel(WC_FL, W_Mesh_FL);
			DeteachWheel(WC_FR, W_Mesh_FR);
			DeteachWheel(WC_RL, W_Mesh_RL);
			DeteachWheel(WC_RR, W_Mesh_RR);

			if(W_Mesh_Extra1)
			{
				DeteachWheel(W_Mesh_Extra1);
			}

			if(W_Mesh_Extra2)
			{
				DeteachWheel(W_Mesh_Extra2);
			}

			ParticleFire.Play();

			BodyParts[0].material.color = ColDark;

			Time.timeScale = 0.2f;

			SetTimeScale = true; // start timer for setting normal timescale again
			SetDragValue = true; // start timer for setting normal drag value

			if(W_Mesh_FL.GetComponent<MeshCollider>())
			{
				W_Mesh_FL.GetComponent<MeshCollider> ().enabled = true;
			}

			if(W_Mesh_FR.GetComponent<MeshCollider>())
			{
				W_Mesh_FR.GetComponent<MeshCollider> ().enabled = true;
			}

			if(W_Mesh_RL.GetComponent<MeshCollider>())
			{
				W_Mesh_RL.GetComponent<MeshCollider> ().enabled = true;
			}

			if(W_Mesh_RR.GetComponent<MeshCollider>())
			{
				W_Mesh_RR.GetComponent<MeshCollider> ().enabled = true;
			}
		}
	}

	void DeteachWheel(WheelCollider WC, Rigidbody R)
	{
		WC.gameObject.SetActive(false);

		R.transform.SetParent(null);
		R.interpolation = RigidbodyInterpolation.Interpolate;
		R.isKinematic = false;

		R.AddExplosionForce(ExplosionPower * WheelForceMultiplier, ExplosionPos.position, ExplosionRange, 1.0f, FM);
	}

	void DeteachWheel(Rigidbody R)
	{
		R.transform.SetParent(null);
		R.interpolation = RigidbodyInterpolation.Interpolate;
		R.isKinematic = false;

		R.AddExplosionForce(ExplosionPower * WheelForceMultiplier, ExplosionPos.position, ExplosionRange, 1.0f, FM);
	}

	void NormalDragNow()
	{
		_RB.drag = 0.02f;
	}

	void NormalTImeNOw()
	{
		Time.timeScale = 1.0f;
	}
}