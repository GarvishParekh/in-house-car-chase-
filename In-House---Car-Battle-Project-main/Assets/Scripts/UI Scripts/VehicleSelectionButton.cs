using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VehicleSelectionButton : MonoBehaviour, IPointerClickHandler
{
	public int ButtonIndex;
	public int VehicleListIndex;

	public Image Im_Button;
	public Image Im_Vehicle;
	public Image Im_Lock;
	public TextMeshProUGUI T_VehicleName;


	public static Action<int, int> OnVehicleSelectionButtonClicked;


    private void Start()
    {
		GetVehicalInfo();
    }

    private void OnEnable()
    {
		VehicleSelection_UI.UpdateBuyUI += GetVehicalInfo;
    }

    private void OnDisable()
    {
		VehicleSelection_UI.UpdateBuyUI += GetVehicalInfo;
    }

    public void OnPointerClick(PointerEventData D)
	{
		if(OnVehicleSelectionButtonClicked != null)
		{
			OnVehicleSelectionButtonClicked(ButtonIndex, VehicleListIndex);
		}
	}

	void GetVehicalInfo ()
	{
		Im_Vehicle.sprite = VehicleImageSystem.instance.vehicleIcons[VehicleListIndex];
		T_VehicleName.text = VehicleImageSystem.instance.vehicleName[VehicleListIndex];

		if (PlayerPrefs.GetInt(string.Concat("VehiclePurchased_", VehicleListIndex.ToString()), 0) == 1)
        {
			Im_Lock.gameObject.SetActive(false);
		}
	}		
}