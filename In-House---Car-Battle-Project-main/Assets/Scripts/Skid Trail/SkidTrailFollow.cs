using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]

public class SkidTrailFollow : MonoBehaviour 
{
	public bool Follow;
	public Transform TargetPos;
	private TrailRenderer TR;

	private bool StartNew;
	private float TrailTime;
	private float Temp;

	private float Width = 0.25f;

	public enum Trail_Status
	{
		Free,
		Occupied
	}

	public Trail_Status TrailStatus;

	void Start()
	{
		TR = GetComponent<TrailRenderer>();

		if(!TR)
		{
			return;
		}
	}

	void Update () 
	{
		if(Follow && TargetPos)
		{
			transform.position = TargetPos.position;
		}

		if(StartNew)
		{
			Temp+= Time.deltaTime;

			if(Temp >= 0.1f)
			{
				StartNew = false;
				StartNow();
			}
			else
			{
				TR.time = 0;
			}
		}
	}

	public void StartTrail(Transform Pos, float T)
	{
		TR.time = 0;
		TrailTime = T;
		TargetPos = Pos;

		TR.widthMultiplier = 0;
		Temp = 0;
		StartNew = true;

		transform.position = Pos.position;

		TrailStatus = Trail_Status.Occupied;
	}

	public void EndTrail()
	{
		Follow = false;
		TargetPos = null;
		TrailStatus = Trail_Status.Free;
	}

	void StartNow()
	{
		if(TR.positionCount < 2)
		{
			TR.widthMultiplier = Width;
			Follow = true;
			TR.time = TrailTime;
		}
	}
}