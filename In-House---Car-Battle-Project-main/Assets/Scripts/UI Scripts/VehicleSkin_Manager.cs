using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VehicleSkin_Manager : MonoBehaviour 
{
	public Canvas C_SkinsWindow;

	[SerializeField]
	private VehicleData[] Vehicles;

	[SerializeField]
	private GameObject[] SkinsGO;

	[SerializeField]
	private VehicleSkinButton[] SkinsButton;

	[SerializeField]
	private int[] SkinsIndex_Purchased;

	[SerializeField]
	private int[] SkinsIndex_NotPurchased;

	private int TotalSkins_Purchased;
	private int TotalSkins_NotPurchased;
	private int BufferNumber;

	private int SelectedTotalSkins;
	private int SelectedVehicleIndex;
	private int SelectedVehicleStorageIndex;

	private int SelectedSkin_ListIndex;
	private int SelectedSkin_StorageIndex;
	private int SelectedSkin_ButtonIndex;

	private int PreviousSkin_ButtonIndex;

	private int i;
	private VehicleData SelectedVehicle;

	private string S_Temp;
	private int S_PurchaseOrder;

	[SerializeField]
	private Sprite S_NormalBG;

	[SerializeField]
	private Sprite S_SelectedBG;

	void OnEnable()
	{
		VehicleSelectionButton.OnVehicleSelectionButtonClicked += OnVehicleSelected;
		MainScreen_UI.OnCustomizeButtonClicked += OnClick_CustomizeButton;
		MainScreen_UI.OnBackButtonClicked_CustomizeWindow += OnClickBack;
		VehicleSelection_UI.OnDefaultVehicleSeelcted += OnDefaultVehicleSelected;
		VehicleSkinButton.OnSkinButtonClicked += OnSkinButtonClicked;
	}

	void OnDisable()
	{
		VehicleSelectionButton.OnVehicleSelectionButtonClicked -= OnVehicleSelected;
		MainScreen_UI.OnCustomizeButtonClicked -= OnClick_CustomizeButton;
		MainScreen_UI.OnBackButtonClicked_CustomizeWindow -= OnClickBack;
		VehicleSelection_UI.OnDefaultVehicleSeelcted -= OnDefaultVehicleSelected;
		VehicleSkinButton.OnSkinButtonClicked -= OnSkinButtonClicked;
	}


	// When selecting the default vehicle on starting of the scene
	void OnDefaultVehicleSelected(int VehicleIndex)
	{
		OnVehicleSelected(0, VehicleIndex);
	}

	// When selecting a vehicle using vehicle list UI
	void OnVehicleSelected(int ButtonIndex, int VehicleIndex)
	{
		C_SkinsWindow.enabled = false;
		SelectedVehicleIndex = VehicleIndex;

		SelectedVehicle = Vehicles[SelectedVehicleIndex];
		SelectedVehicleStorageIndex = SelectedVehicle.StorageIndex;
	}

	// When user clicks on Back button of Customize window
	void OnClickBack()
	{
		C_SkinsWindow.enabled = false;
	}

	// When user click son Customize button
	void OnClick_CustomizeButton()
	{
		C_SkinsWindow.enabled = true;

		ShowSkinsInUI();
	}

	void ShowSkinsInUI()
	{
		// Reset all buffer variables

		TotalSkins_NotPurchased = 0;
		TotalSkins_Purchased = 0;
		BufferNumber = 0;

		SelectedTotalSkins = SelectedVehicle.TotalSkins;

		// Always skin with 0 storage index is default purchased

		PlayerPrefs.SetInt(string.Concat("Vehicle_", SelectedVehicle.StorageIndex.ToString(), "_SkinPurchased_0"), 1);
		PlayerPrefs.SetInt(string.Concat("Vehicle_", SelectedVehicle.StorageIndex.ToString(), "_SkinPurchaseOrder_0"), 1);

		// Sort purchaed & not-purchased indexes

		for(i = 0; i < SelectedTotalSkins; i++)
		{
			S_Temp = string.Concat("Vehicle_", SelectedVehicle.StorageIndex.ToString(), "_SkinPurchased_", SelectedVehicle.SkinStorageID[i].ToString());

			if(PlayerPrefs.GetInt(S_Temp, 0) == 1) // Skin is purchased
			{
				S_PurchaseOrder = PlayerPrefs.GetInt(string.Concat("Vehicle_", SelectedVehicle.StorageIndex.ToString(), "_SkinPurchaseOrder_", SelectedVehicle.SkinStorageID[i].ToString()), 1);
				SkinsIndex_Purchased[S_PurchaseOrder - 1] = i;

				TotalSkins_Purchased++;
			}
			else // Skin is not purchased
			{
				SkinsIndex_NotPurchased[TotalSkins_NotPurchased] = i;
				TotalSkins_NotPurchased++;
			}
		}

		// Show first purchased skins in reverse order in UI to show last purchased first, then show not-purchased skins in normal order

		for(i = TotalSkins_Purchased - 1; i >= 0; i--)
		{
			ShowEachSkinInUI_PUrchased(SkinsIndex_Purchased[i]);
		}

		for(i = 0; i < TotalSkins_NotPurchased; i++)
		{
			ShowEachSkinInUI_NotPUrchased(SkinsIndex_NotPurchased[i]);
		}

		// Adjust last 4 skins appereance

		if(SelectedTotalSkins == 10)
		{
			SkinsGO[8].SetActive(true);
			SkinsGO[9].SetActive(true);
			SkinsGO[10].SetActive(false);
			SkinsGO[11].SetActive(false);
		}
		else if(SelectedTotalSkins == 12)
		{
			SkinsGO[8].SetActive(true);
			SkinsGO[9].SetActive(true);
			SkinsGO[10].SetActive(true);
			SkinsGO[11].SetActive(true);
		}
		else if(SelectedTotalSkins == 8)
		{
			SkinsGO[8].SetActive(false);
			SkinsGO[9].SetActive(false);
			SkinsGO[10].SetActive(false);
			SkinsGO[11].SetActive(false);
		}
	}

	void ShowEachSkinInUI_PUrchased(int SkinIndex)
	{
		SkinsButton[BufferNumber].ImSkin.sprite = SelectedVehicle.SkinSprites[SkinIndex];
		SkinsButton[BufferNumber].SkinListIndex = SkinIndex;
		SkinsButton[BufferNumber].SkinStorageID = SelectedVehicle.SkinStorageID[SkinIndex];
		SkinsButton[BufferNumber].T_Cost.text = SelectedVehicle.SkinCost.ToString();

		S_Temp = string.Concat("Vehicle_", SelectedVehicle.StorageIndex.ToString(), "_SelectedSkin");

		if(PlayerPrefs.GetInt(S_Temp, 0) == SelectedVehicle.SkinStorageID[SkinIndex]) // Skin is selected. Apply skin to vehicle and use special background for skin button
		{
			SkinsButton[BufferNumber].SkinSelected();

			SkinsButton[PreviousSkin_ButtonIndex].ImBG.sprite = S_NormalBG;
			SkinsButton[BufferNumber].ImBG.sprite = S_SelectedBG;

			foreach(Renderer R in SelectedVehicle.RenderObjs)
			{
				R.material.mainTexture = SelectedVehicle.VehicleSkins[SkinIndex];
			}

			PreviousSkin_ButtonIndex = BufferNumber;
		}
		else
		{
			SkinsButton[BufferNumber].SkinPurchased();
		}

		print("Purchased button : " + BufferNumber);

		BufferNumber++;
	}

	void ShowEachSkinInUI_NotPUrchased(int SkinIndex)
	{
		SkinsButton[BufferNumber].ImSkin.sprite = SelectedVehicle.SkinSprites[SkinIndex];
		SkinsButton[BufferNumber].SkinListIndex = SkinIndex;
		SkinsButton[BufferNumber].SkinStorageID = SelectedVehicle.SkinStorageID[SkinIndex];
		SkinsButton[BufferNumber].T_Cost.text = SelectedVehicle.SkinCost.ToString();

		SkinsButton[BufferNumber].SkinNotPurchased();

		print("Not Purchased button : " + BufferNumber);

		BufferNumber++;
	}



	// When user clicks on any Skin
	void OnSkinButtonClicked(int SkinButtonIndex, int SkinListIndex)
	{
		foreach(Renderer R in SelectedVehicle.RenderObjs)
		{
			R.material.mainTexture = SelectedVehicle.VehicleSkins[SkinListIndex];
		}

		SkinsButton[PreviousSkin_ButtonIndex].ImBG.sprite = S_NormalBG;
		SkinsButton[SkinButtonIndex].ImBG.sprite = S_SelectedBG;

		PreviousSkin_ButtonIndex = SkinButtonIndex;
	}
}