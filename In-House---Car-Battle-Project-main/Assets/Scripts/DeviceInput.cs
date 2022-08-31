using UnityEngine;
using System;

public class DeviceInput : MonoBehaviour 
{
	public static Action OnRightDown;

	public static Action OnRightUp;

	public static Action OnLeftDown;

	public static Action OnLeftUp;

	public static Action OnBoostEnable;

	public static Action OnBoostDisable;

	public bool LockFPS;
	public int TargetFPS;
	public bool isExploaded;


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
		Time.timeScale = 1.0f;

		if(LockFPS || Application.platform == RuntimePlatform.Android)
		{
			Application.targetFrameRate = TargetFPS;
		}

		isExploaded = false;
	}




	public void OnLeft_Down()
	{
		if(OnLeftDown != null && !isExploaded)
		{
			OnLeftDown();
		}
	}

	public void OnLeft_Up()
	{
		if(OnLeftUp != null && !isExploaded)
		{
			OnLeftUp();
		}
	}

	public void OnRight_Down()
	{
		if(OnRightDown != null && !isExploaded)
		{
			Debug.Log("Event called");
			OnRightDown();
		}
	}

	public void OnRight_Up()
	{
		if(OnRightUp != null && !isExploaded)
		{
			OnRightUp();
		}
	}

	public void OnBoost_Enable()
	{
		if(OnBoostEnable != null && !isExploaded)
		{
			OnBoostEnable();
		}
	}

	public void OnBoost_Disable()
	{
		if(OnBoostDisable != null && !isExploaded)
		{
			OnBoostDisable();
		}
	}



	public void Restart()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}



	void OnPlayerCarExploaded()
	{
		isExploaded = true;

		if(OnBoostDisable != null)
		{
			OnBoostDisable();
		}

		if(OnRightUp != null)
		{
			OnRightUp();
		}

		if(OnLeftUp != null)
		{
			OnLeftUp();
		}
	}
}