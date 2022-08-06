using UnityEngine;

public class MotionBlurControl : MonoBehaviour 
{
	public ParticleSystem BlurParticle;
	private ParticleSystem.MainModule Main;

	public int MinParticles;
	public int MaxParticles;
	public float IncrementInterval;
	public int IncrementRate;

	private int CurrentParticles;
	private bool isStarted;
	private float Temp;
	private bool isRacingCam;

	void OnEnable()
	{
		MyCameraFollow.OnCameraViewChanged += OnCameraViewChanged;
		DeviceInput.OnBoostEnable += StartMotionBlur;
		DeviceInput.OnBoostDisable += StopMotionBlur;
		Car_Player.OnBoosterOff += StopMotionBlur;
	}

	void OnDisable()
	{
		MyCameraFollow.OnCameraViewChanged -= OnCameraViewChanged;
		DeviceInput.OnBoostEnable -= StartMotionBlur;
		DeviceInput.OnBoostDisable -= StopMotionBlur;
		Car_Player.OnBoosterOff -= StopMotionBlur;
	}



	void FixedUpdate()
	{
		if(isStarted)
		{
			Main = BlurParticle.main;
			Main.maxParticles = CurrentParticles;

			Temp += Time.deltaTime;

			if(Temp >= IncrementInterval)
			{
				Temp = 0;

				if(CurrentParticles < MaxParticles)
				{
					CurrentParticles += IncrementRate;
				}
				else
				{
					isStarted = false;
				}
			}
		}
	}



	void StartMotionBlur()
	{
		if(BlurParticle && isRacingCam)
		{
			isStarted = true;
			CurrentParticles = MinParticles;
			BlurParticle.Play();
		}
	}

	void StopMotionBlur()
	{
		if(BlurParticle)
		{
			isStarted = false;
			BlurParticle.Stop();
		}
	}



	void OnCameraViewChanged(MyCameraFollow.Cam_Positions P)
	{
		if(P == MyCameraFollow.Cam_Positions.Racing)
		{
			isRacingCam = true;
		}
		else
		{
			isRacingCam = false;
		}
	}
}