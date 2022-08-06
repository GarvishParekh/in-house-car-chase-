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

    public void OnPointerClick(PointerEventData D)
	{
		if(OnVehicleSelectionButtonClicked != null)
		{
			OnVehicleSelectionButtonClicked(ButtonIndex, VehicleListIndex);
		}
	}

	void GetVehicalInfo ()
	{
		Im_Vehicle.sprite = VehicleImageSystem.instance.vehicleIcons[ButtonIndex];
		T_VehicleName.text = VehicleImageSystem.instance.vehicleName[ButtonIndex];
	}		
}