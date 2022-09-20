using TMPro;
using System;
using UnityEngine;

public class ShowCoinsUI : MonoBehaviour 
{
	public static ShowCoinsUI instance;
	[SerializeField]
	private TextMeshProUGUI TCoins;

	[SerializeField]
	public bool isPauseUI;

	private int TotalCoins;
	private float LastShown;
	private int AnimationSpeed;

    private void Awake()
    {
		instance = this;
    }

    void OnEnable()
	{
		PlayerPrefs.SetInt("TotalCoins", 300);
		PlayerPrefs.Save();
		TotalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
		LastShown = TotalCoins;

		if(isPauseUI)
		{
			TCoins.text = TotalCoins.ToString();
		}
		else
		{
			TCoins.text = LastShown.ToString();
		}

		VehicleSelection_UI.UpdateBuyUI += UpdateCoinUI;
	}

    private void OnDisable()
    {
		VehicleSelection_UI.UpdateBuyUI -= UpdateCoinUI;
    }

    void Update () 
	{
		if(!isPauseUI)
		{
			if(LastShown != TotalCoins)
			{
				LastShown = Mathf.MoveTowards (LastShown, TotalCoins, AnimationSpeed * Time.deltaTime);

				//TCoins.text = ((int)LastShown).ToString();
			}
		}
		//AddCoins();
	}

	void AddCoins()
    {
		if (Input.GetKeyDown (KeyCode.Space))
        {
			// add coins
			TotalCoins += 1000;
			PlayerPrefs.SetInt("TotalCoins", TotalCoins);

			// update in UI
			TCoins.text = TotalCoins.ToString();
		}
    }

	public void OnCoinsValueUpdated(int Val)
	{
		if(Val > 0)
		{
			AnimationSpeed = (int)(Val * 0.5f);
		}
		else
		{
			AnimationSpeed = (int)(-Val * 2);
		}

		if(AnimationSpeed > 15000)
		{
			AnimationSpeed = 15000;
		}

		TotalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
	}

	void UpdateCoinUI ()
    {
		int totalCoint = PlayerPrefs.GetInt("TotalCoins", 0);
		TCoins.text = totalCoint.ToString();
	}

	public bool CheckCoins(int _itemAmount)
    {
		if (_itemAmount > TotalCoins)
			return false;

		else
        {
			TotalCoins -= _itemAmount;
			PlayerPrefs.SetInt("TotalCoins", _itemAmount);

			// update in UI
			TCoins.text = TotalCoins.ToString();
			return true;
        }
    }
}