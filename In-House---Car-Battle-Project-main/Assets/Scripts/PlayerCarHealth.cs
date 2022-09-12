using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerCarHealth : MonoBehaviour 
{
	public int HP;

	public Image BarMain;
	public Image BarInner;

	private bool isBarMoving;
	private bool isDelaying;

	private float UnitHealth;
	private float BarsFillAmount;

	public float BarSpeed;
	public float BarDelay;

	private float TempDelay;

	public static Action OnPlayerCarExplosion;


	void OnEnable()
	{
		PlayerCarTrigger.OnPlayerCarDamage += OnDamageOccur;
		GroundTrigger.OnGroundHit += OnDamageOccur;
		AI_Car_Shooting.AI_Shoot += ShootingDamage;
	}

	void OnDisable()
	{
		PlayerCarTrigger.OnPlayerCarDamage -= OnDamageOccur;
		GroundTrigger.OnGroundHit -= OnDamageOccur;
		AI_Car_Shooting.AI_Shoot -= ShootingDamage;
	}

	void Start () 
	{
		UnitHealth = HP * 0.01f;
	}

	void Update()
	{
		if(isBarMoving)
		{
			BarInner.fillAmount = Mathf.MoveTowards(BarInner.fillAmount, BarsFillAmount, BarSpeed * Time.deltaTime);

			if(BarInner.fillAmount == BarsFillAmount)
			{
				isBarMoving = false;
			}
		}

		if(isDelaying)
		{
			TempDelay += Time.deltaTime;

			if(TempDelay >= BarDelay)
			{
				isDelaying = false;
				isBarMoving = true;
			}
		}
	}

	public void OnDamageOccur(int DmgPoints)
	{
		HP -= DmgPoints;

		if(HP < 0)
		{
			HP = 0;
		}

		isBarMoving = false;
		isDelaying = true;
		TempDelay = 0;

		BarsFillAmount = (HP / UnitHealth) * 0.01f;
		BarMain.fillAmount = BarsFillAmount;

		if(HP == 0 && OnPlayerCarExplosion != null)
		{
			OnPlayerCarExplosion();
		}
	}

	void ShootingDamage ()
    {
		OnDamageOccur(3);
    }
}