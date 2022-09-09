using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// This script manages
// - AI car's health
// - Visuals of health
// AI Car's arrow indicator for player car

[RequireComponent(typeof(Car_AI))]
[RequireComponent(typeof(AICarExplosion))]

public class AI_CarHealth : MonoBehaviour 
{
	WaitForSeconds tenSeconds = new WaitForSeconds(10);
	public static Action CarAdded;
	public static Action<Transform> AIDestroy;

	[Header("Display Healthbar")]
	private Camera Cam;
	public Transform HealthObj;
	public Image HealthBar;
	public CanvasGroup CG;

	private Transform TargetCam;
	private Vector3 Temp1;
	private RaycastHit RH;

	[Header("Car Health")]

	public int HP;
	public AnimationCurve AC;

	private Car_AI CA;
	private float UnitHealth;
	private int CurrentDmg;
	private float CamDistance;

	[Header("Car Arrow")]

	public Transform ArrowTransform;
	public Transform Arrow;
	private Vector3 TempEur;
	public Transform LookArrow;

	public float CheckRate;
	private float TempRate;

	private Transform ArrowHolder;
	public bool PlayerCarExploaded;
	public bool ThisCarExploaded;
	private float FarClipPlane;
	private AICarExplosion ACE;

	public enum Car_State
	{
		Exploaded,
		InsideFrustum,
		OutOfFrustum
	}

	public Car_State CarState;
	public GameObject shadowParent;


	void OnEnable()
	{
		Car_Player.OnCarSelected += SetTargetCar;
		PlayerCarHealth.OnPlayerCarExplosion += OnPlayerCarExploaded;
	}

	void OnDisable()
	{
		Car_Player.OnCarSelected -= SetTargetCar;
		PlayerCarHealth.OnPlayerCarExplosion -= OnPlayerCarExploaded;
	}

	void Start ()
	{
		CarAdded?.Invoke();
		ArrowHolder = GameObject.FindWithTag("ArrowsHolder").transform;
		Cam = Camera.main;
		ACE = GetComponent<AICarExplosion>();
		CA = GetComponent<Car_AI>();

		Arrow.SetParent(ArrowHolder);
		Arrow.localPosition = Vector3.zero;
		Arrow.localEulerAngles = Vector3.zero;

		LookArrow.SetParent(ArrowHolder);
		LookArrow.localPosition = Vector3.zero;
		LookArrow.localEulerAngles = Vector3.zero;

		TargetCam = Cam.transform;
		UnitHealth = HP * 0.01f;
		FarClipPlane = Cam.farClipPlane;
	}
	
	void Update () 
	{
		if(!PlayerCarExploaded && !ThisCarExploaded)
		{
			TempRate += Time.deltaTime;

			if(TempRate > CheckRate)
			{
				TempRate = 0;

				// Check inFtructrum or not every fixed rate
				CheckForArrow();
			}

			// Manage arrow angle, health bar scale & health bar always face to camera.

			if(CarState == Car_State.OutOfFrustum)
			{
				CG.alpha = 0;
				LookArrow.LookAt(transform);
				Arrow.eulerAngles = LookArrow.eulerAngles;

				TempEur = Arrow.localEulerAngles;
				TempEur.x = 0;
				TempEur.z = 0;
				Arrow.localEulerAngles = TempEur;
			}
			else
			{
				HealthObj.eulerAngles = TargetCam.eulerAngles;
				CamDistance = (transform.position - TargetCam.position).magnitude;
				HealthObj.localScale = Vector3.one * AC.Evaluate(CamDistance);
			}
		}
	}

	void CheckForArrow()
	{
		Temp1 = Cam.WorldToViewportPoint(transform.position);

		if(PlayerCarExploaded || Temp1.x > 0 && Temp1.x < 1 && Temp1.y > 0 && Temp1.y < 1 && Temp1.z > 0 && Temp1.z < FarClipPlane)
		{
			if(CarState != Car_State.InsideFrustum) // So that it executes inside code only when frustrum is changed.
			{
				CarState = Car_State.InsideFrustum;
				CG.alpha = 1;
				ArrowTransform.gameObject.SetActive(false);
			}
		}
		else
		{
			if(CarState != Car_State.OutOfFrustum) // So that it executes inside code only when frustrum is changed.
			{
				CarState = Car_State.OutOfFrustum;

				ArrowTransform.gameObject.SetActive(true);
			}
		}
	}

	void OnCollisionEnter(Collision C)
	{
		if(C.collider.CompareTag("Player") || C.collider.CompareTag("Car_AI") || C.collider.CompareTag("Environment"))
		{
			CurrentDmg = (int) C.relativeVelocity.magnitude;

			if(CurrentDmg > 4)
			{
				OnDamageOccur(CurrentDmg);
			}
		}
	}

	void SetTargetCar(Transform T)
	{
		LookArrow.SetParent(T);
		LookArrow.localPosition = Vector3.zero;
		LookArrow.localEulerAngles = Vector3.zero;
	}

	IEnumerator destroyCar()
	{
		yield return tenSeconds;
		Destroy(gameObject);
	}

	bool isDestroyed = false;

	[Header("After Losing")]
	[SerializeField] Transform objectToDequeue;
	public void OnDamageOccur(int DmgPoints)
	{
		if (isDestroyed)
			return;
		HP -= DmgPoints;

		if(HP < 0)
		{
			Destroy(shadowParent);
			isDestroyed = true;
			AIDestroy?.Invoke(objectToDequeue);
			StartCoroutine(nameof(destroyCar));		// destroy the car after 10 seconds
			HP = 0;
			CA.Stop_AI_Behavior();
			ACE.OnExplosion();
			OnThisCarExploaded();
			objectToDequeue.tag = "Player";

			// if the car gets destroy remove it from the list of target cars
		}

		HealthBar.fillAmount = (HP / UnitHealth) * 0.01f;
	}

	void OnThisCarExploaded()
	{
		CarState = Car_State.Exploaded;

		ThisCarExploaded = true;
		ArrowTransform.gameObject.SetActive(false);
		CG.alpha = 0;
	}

	void OnPlayerCarExploaded()
	{
		PlayerCarExploaded = true;
		ArrowTransform.gameObject.SetActive(false);
		CG.alpha = 0;
	}
}