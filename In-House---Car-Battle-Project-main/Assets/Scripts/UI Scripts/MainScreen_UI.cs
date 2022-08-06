using System;
using UnityEngine;

public class MainScreen_UI : MonoBehaviour
{
	public static Action OnBuyButtonClicked;
	public static Action OnCustomizeButtonClicked;
	public static Action OnBackButtonClicked_CustomizeWindow;

	public void OnClick_Customize()
	{
		if(OnCustomizeButtonClicked != null)
		{
			OnCustomizeButtonClicked();
		}
	}

	public void OnClick_Buy ()
    {
		OnBuyButtonClicked?.Invoke(); 

	}

	public void OnClick_BackButton_CustomizeWindow()
	{
		if(OnBackButtonClicked_CustomizeWindow != null)
		{
			OnBackButtonClicked_CustomizeWindow();
		}
	}
}