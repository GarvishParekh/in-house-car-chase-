using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TornadoThrust : MonoBehaviour 
{
	public float ForceUp;
	public float Torque;

	public static Action OnTornadoEntered;

	public Transform[] CamPositions;
	public int CurrentPos;

	private int TotalPos;
	public Transform Cam;

	public Rigidbody PlayerRB;

	void Start()
	{
		TotalPos = CamPositions.Length;

		CurrentPos = UnityEngine.Random.Range(0, TotalPos);
	}

	void OnTriggerEnter(Collider C)
	{
		if(C.gameObject.CompareTag("Player"))
		{
			//GameObject.FindObjectOfType<PlayerCarHealth>().OnDamageOccur(50);

			if(OnTornadoEntered != null)
			{
				OnTornadoEntered();

				CurrentPos++;

				if(CurrentPos >= TotalPos)
				{
					CurrentPos = 0;
				}

				Cam.transform.position = CamPositions[CurrentPos].position;
				Cam.transform.eulerAngles = CamPositions[CurrentPos].eulerAngles;
			}
		}
	}

	void OnTriggerStay(Collider C)
	{
		if(C.gameObject.CompareTag("Player"))
		{
			if(C.transform.parent.GetComponent<Rigidbody>())
			{
				PlayerRB = C.transform.parent.GetComponent<Rigidbody>();
				PlayerRB.AddForce(Vector3.up * ForceUp, ForceMode.Impulse);
				PlayerRB.AddTorque(Vector3.up * Torque, ForceMode.Impulse);
			}
		}
	}
}