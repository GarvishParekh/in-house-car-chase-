using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

// IMPORTANT : The order of vehicle will be shown based on the list Vehicle Details.
// Please maintain the same order in VehicleDetails, Vehicles & CamTrans list.

public class VehicleSelection_UI : MonoBehaviour 
{
	[Space]

	[Header("Vehicle Selection Display UI")]

	[Space]
	[Space]

	[SerializeField]
	private VehicleData[] VehicleDetails;
	[SerializeField]
	private VehicleSelectionButton[] ButtonList;
	[SerializeField]
	private Sprite S_Selected;
	[SerializeField]
	private Sprite S_NotSelected;

	[Space]

	[Header("Vehicle Selection Camera Movement")]

	[Space]
	[Space]

	[SerializeField]
	private GameObject[] Vehicles;
	[SerializeField]
	private Transform[] CamTrans;

	[SerializeField]
	private Transform Cam;

	[SerializeField]
	private float CamMoveSpeed;
	[SerializeField]
	private float CamRotSpeed;

	private int SelectedVehicle_ListIndex;
	private int SelectedButtonIndex;
	private int LastButtonIndex;
	private int LastVehicleIndex;

	private int i;
	private int TotalVehicles;
	private Transform TargetTransform;

	[Space]
	[SerializeField] GameObject buyButton;
	[SerializeField] GameObject startbutton;
	[SerializeField] TMP_Text vehicleCost;
	[SerializeField] GameObject customizeButton;

	public static Action<int> OnDefaultVehicleSeelcted;
	public static Action UpdateBuyUI;


	void OnEnable()
	{
		VehicleSelectionButton.OnVehicleSelectionButtonClicked += OnVehicleList_ButtonClicked;
	}

	void OnDisable()
	{
		VehicleSelectionButton.OnVehicleSelectionButtonClicked -= OnVehicleList_ButtonClicked;
	}

	void Awake()
	{
		PlayerPrefs.SetInt("VehiclePurchased_0", 1);
		PlayerPrefs.SetInt("VehiclePurchaseIndex_0", 1);
	}

	void Start () 
	{
		TotalVehicles = VehicleDetails.Length;

		// Load all vehicle's purchase & upgrade data. Disable all vehicles by default
		for(i = 0; i < TotalVehicles; i++)
		{
			VehicleDetails[i].GetData_Configurations();
			Vehicles[i].SetActive(false);
		}

		// Show the vehicle which is selected.
		ShowCurrentVehicle(true);

		// Call this Action so that other scripts also get notified about which vehicle is selected by default
		if(OnDefaultVehicleSeelcted != null)
		{
			OnDefaultVehicleSeelcted(SelectedVehicle_ListIndex);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space))
        {
			DeleteAllSaveFiles();
        }

		Cam.position = Vector3.Lerp (Cam.position, TargetTransform.position, CamMoveSpeed * Time.deltaTime);
		Cam.eulerAngles = Vector3.Lerp (Cam.eulerAngles, TargetTransform.eulerAngles, CamRotSpeed * Time.deltaTime);
	}

	void OnVehicleList_ButtonClicked(int ButtonIndex, int V_ListIndex)
	{
		SelectedButtonIndex = ButtonIndex;
		SelectedVehicle_ListIndex = V_ListIndex;

		ShowCurrentVehicle();
		PlayerPrefs.SetInt("SelectedVehicle", V_ListIndex);
	}

	void ShowCurrentVehicle(bool ShowDirect = false)
	{
		Vehicles [LastVehicleIndex].SetActive (false);
		Vehicles [SelectedVehicle_ListIndex].SetActive (true);

		bool isBought = false;
		if (PlayerPrefs.GetInt(string.Concat("VehiclePurchased_", SelectedVehicle_ListIndex.ToString()), 0) == 1)
        {
			isBought = true;
		}
        else
        {
			isBought = false;
        }
		int vehiclePrice = VehicleDetails[SelectedVehicle_ListIndex].VehicleCost;

		if (!isBought)
        {
			buyButton.SetActive(true);
			customizeButton.SetActive(false);
			startbutton.SetActive(false);
			vehicleCost.text = vehiclePrice.ToString();
        }
        else
        {
			buyButton.SetActive(false);
			customizeButton.SetActive(true);
			startbutton.SetActive(true);
		}
		LastVehicleIndex = SelectedVehicle_ListIndex;

		ButtonList[LastButtonIndex].Im_Button.sprite = S_NotSelected;
		ButtonList[SelectedButtonIndex].Im_Button.sprite = S_Selected;
		LastButtonIndex = SelectedButtonIndex;

		// Show camera at its target position by default when this scene loads.
		if(ShowDirect)
		{
			Cam.position = CamTrans [SelectedVehicle_ListIndex].position;
			Cam.eulerAngles = CamTrans [SelectedVehicle_ListIndex].eulerAngles;
		}

		TargetTransform = CamTrans [SelectedVehicle_ListIndex];
	}

	void OnClick_Select()
	{
		PlayerPrefs.SetInt("SelectedVehicle_StorageIndex", VehicleDetails[SelectedVehicle_ListIndex].StorageIndex);
	}

	// buy button function
	public void B_BuyButton ()
    {
		int vehicleCost = VehicleDetails[SelectedVehicle_ListIndex].VehicleCost;
		int totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

		if (totalCoins >= vehicleCost)
        {
			totalCoins -= vehicleCost;
			PlayerPrefs.SetInt("TotalCoins", totalCoins);
			PlayerPrefs.SetInt(string.Concat("VehiclePurchased_", SelectedVehicle_ListIndex.ToString()), 1);

			ShowCurrentVehicle();
			UpdateBuyUI?.Invoke();
		}
	}

	void DeleteAllSaveFiles ()
    {
		PlayerPrefs.DeleteAll();
    }
}