using UnityEngine;

public class SkidTrailManager : MonoBehaviour 
{
	public SkidTrailFollow[] STF;

	private int CurrentTrail;
	private int TotalTrails;


	void OnEnable()
	{
		WheelSkidTrail.OnStartNewTrail += OnNewTrailRequested;
	}

	void OnDisable()
	{
		WheelSkidTrail.OnStartNewTrail -= OnNewTrailRequested;
	}


	void Start () 
	{
		TotalTrails = STF.Length;
	}

	SkidTrailFollow OnNewTrailRequested()
	{
		CurrentTrail++;

		if(CurrentTrail >= TotalTrails)
		{
			CurrentTrail = 0;
		}

		return STF[CurrentTrail];
	}

	void OnStopCurrentTrail(int ID)
	{
		STF[ID].EndTrail();
	}
}