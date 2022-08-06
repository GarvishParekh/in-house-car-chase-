using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFollower : MonoBehaviour 
{
	public Transform PlayerCar;

	void Start () 
	{
		transform.SetParent (null);
	}

	void Update()
	{
		if(PlayerCar)
		{
			transform.position = PlayerCar.position;
		}
	}
}