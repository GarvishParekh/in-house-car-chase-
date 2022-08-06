using UnityEngine;
using System;

public class GroundTrigger : MonoBehaviour 
{
	public Rigidbody RB_Player;
	public static Action<int> OnGroundHit = delegate{};
	public static Action OnCarBackToGround = delegate{};
	private int DamagePoint;
	public bool isInAir;

	void OnEnable()
	{
		TornadoThrust.OnTornadoEntered += OnTornadoEnter;
	}

	void OnDisable()
	{
		TornadoThrust.OnTornadoEntered -= OnTornadoEnter;
	}

	void OnTriggerEnter(Collider C)
	{
		if(C.CompareTag("Ground"))
		{
			if(isInAir && RB_Player && OnGroundHit != null)
			{
				isInAir = false;
				DamagePoint = (int) RB_Player.velocity.y;

				if(RB_Player.velocity.y < 0)
				{
					DamagePoint *= -1;
				}

				if(DamagePoint < 2)
				{
					DamagePoint = 2;
				}

				OnGroundHit(DamagePoint * 3);
				OnCarBackToGround();
			}
		}
	}

	void OnTornadoEnter()
	{
		isInAir = true;
	}
}