using UnityEngine;
using UnityEngine.UI;
using System;

// This script will call a delegate ActivateGameplayScene
// Till then we can hold all movement objects and let the scene get initialized properly
// This will hide the initial scene spikes behind the black screen and user will see the gameplay when its smooth
// (after almost a second)

public class ActivateGameplayScene : MonoBehaviour 
{
	public static Action OnGameplaySceneActivated;

	public float ActivationDelay;
	public CanvasGroup CG;
	public float FadeSpeed;

	private bool isActive;
	private float Temp;

	void Start ()
	{
		CG.alpha = 1.0f;
	}
	
	void Update () 
	{
		if(!isActive)
		{
			Temp += Time.deltaTime;

			if(Temp >= ActivationDelay)
			{
				ActivateNow();
				isActive = true;
			}
		}
		else if(CG.alpha > 0)
		{
			CG.alpha = Mathf.MoveTowards(CG.alpha, 0, FadeSpeed * Time.deltaTime);
		}
		else
		{
			this.enabled = false;
		}
	}

	void ActivateNow()
	{
		if(OnGameplaySceneActivated != null)
		{
			OnGameplaySceneActivated();
		}
	}
}