using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen_UI : MonoBehaviour
{
	public static Action OnBuyButtonClicked;
	public static Action OnCustomizeButtonClicked;
	public static Action OnBackButtonClicked_CustomizeWindow;

	[SerializeField] string gameplayScene;

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

	public void B_StartFunction ()
    {
		SceneManager.LoadScene(gameplayScene);
    }
}