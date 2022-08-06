using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VehicleUpgrade_Manager : MonoBehaviour 
{
	public Canvas C_VehicleStatsWindow;

	[SerializeField]
	private VehicleData[] Vehicles;

	[Space]

	[Header("Configuration UI")]

	[SerializeField]
	private Image PBar_Engine_Current;
	[SerializeField]
	private Image PBar_Engine_Max;

	[SerializeField]
	private Image PBar_Control_Current;
	[SerializeField]
	private Image PBar_Control_Max;

	[SerializeField]
	private Image PBar_Health_Current;
	[SerializeField]
	private Image PBar_Health_Max;

	[SerializeField]
	private Image PBar_Booster_Current;
	[SerializeField]
	private Image PBar_Booster_Max;

	[Space]

	[SerializeField]
	private TextMeshProUGUI T_EngineValue;
	[SerializeField]
	private TextMeshProUGUI T_ControlValue;
	[SerializeField]
	private TextMeshProUGUI T_HealthValue;
	[SerializeField]
	private TextMeshProUGUI T_BoosterValue;

	[Space]

	[Header("Upgrade Section")]

	[Space]
	[Space]

	[SerializeField]
	private TextMeshProUGUI T_EngineDiff;
	[SerializeField]
	private TextMeshProUGUI T_ControlDiff;
	[SerializeField]
	private TextMeshProUGUI T_HealthDiff;
	[SerializeField]
	private TextMeshProUGUI T_BoosterDiff;

	[Space]

	[SerializeField]
	private TextMeshProUGUI T_EngineCost;
	[SerializeField]
	private TextMeshProUGUI T_ControlCost;
	[SerializeField]
	private TextMeshProUGUI T_HealthCost;
	[SerializeField]
	private TextMeshProUGUI T_BoosterCost;

	[Space]

	[SerializeField]
	private TextMeshProUGUI T_EngineTitle;
	[SerializeField]
	private TextMeshProUGUI T_ControlTitle;
	[SerializeField]
	private TextMeshProUGUI T_HealthTitle;
	[SerializeField]
	private TextMeshProUGUI T_BoosterTitle;

	[Space]

	[SerializeField]
	private GameObject ButtonEngine;
	[SerializeField]
	private GameObject ButtonControl;
	[SerializeField]
	private GameObject ButtonHealth;
	[SerializeField]
	private GameObject ButtonBooster;

	[Space]

	[SerializeField]
	private GameObject ToolTipEngine;
	[SerializeField]
	private GameObject ToolTipControl;
	[SerializeField]
	private GameObject ToolTipHealth;
	[SerializeField]
	private GameObject ToolTipBooster;

	[SerializeField]
	private float PBarSpeed;

	private VehicleData SelectedVehicle;

	private float Fill_Engine_Current;
	private float Fill_Engine_Max;

	private float Fill_Control_Current;
	private float Fill_Control_Max;

	private float Fill_Health_Current;
	private float Fill_Health_Max;

	private float Fill_Booster_Current;
	private float Fill_Booster_Max;

	void OnEnable()
	{
		VehicleSelectionButton.OnVehicleSelectionButtonClicked += OnVehicleSelected;
		MainScreen_UI.OnBackButtonClicked_CustomizeWindow += OnClickBack_CustomizeWindow;
		MainScreen_UI.OnCustomizeButtonClicked += OnClick_CustomizeButton;
		VehicleSelection_UI.OnDefaultVehicleSeelcted += OnDefaultVehicleSelected;
	}

	void OnDisable()
	{
		VehicleSelectionButton.OnVehicleSelectionButtonClicked -= OnVehicleSelected;
		MainScreen_UI.OnBackButtonClicked_CustomizeWindow -= OnClickBack_CustomizeWindow;
		MainScreen_UI.OnCustomizeButtonClicked -= OnClick_CustomizeButton;
		VehicleSelection_UI.OnDefaultVehicleSeelcted -= OnDefaultVehicleSelected;
	}

	void Update () 
	{
		PBar_Engine_Current.fillAmount = Mathf.MoveTowards(PBar_Engine_Current.fillAmount, Fill_Engine_Current, PBarSpeed * Time.deltaTime);
		PBar_Engine_Max.fillAmount = Mathf.MoveTowards(PBar_Engine_Max.fillAmount, Fill_Engine_Max, PBarSpeed * Time.deltaTime);

		PBar_Control_Current.fillAmount = Mathf.MoveTowards(PBar_Control_Current.fillAmount, Fill_Control_Current, PBarSpeed * Time.deltaTime);
		PBar_Control_Max.fillAmount = Mathf.MoveTowards(PBar_Control_Max.fillAmount, Fill_Control_Max, PBarSpeed * Time.deltaTime);

		PBar_Health_Current.fillAmount = Mathf.MoveTowards(PBar_Health_Current.fillAmount, Fill_Health_Current, PBarSpeed * Time.deltaTime);
		PBar_Health_Max.fillAmount = Mathf.MoveTowards(PBar_Health_Max.fillAmount, Fill_Health_Max, PBarSpeed * Time.deltaTime);

		PBar_Booster_Current.fillAmount = Mathf.MoveTowards(PBar_Booster_Current.fillAmount, Fill_Booster_Current, PBarSpeed * Time.deltaTime);
		PBar_Booster_Max.fillAmount = Mathf.MoveTowards(PBar_Booster_Max.fillAmount, Fill_Booster_Max, PBarSpeed * Time.deltaTime);
	}

	void OnDefaultVehicleSelected(int VehicleIndex)
	{
		OnVehicleSelected(0, VehicleIndex);
	}

	void OnVehicleSelected(int ListButtonIndex, int VehicleIndex) // List button index not useful here.
	{
		C_VehicleStatsWindow.enabled = true;

		SelectedVehicle = Vehicles[VehicleIndex];

		Fill_Engine_Current = SelectedVehicle.PBarEngine[SelectedVehicle.EngineLevel];
		Fill_Engine_Max = SelectedVehicle.PBarEngine[SelectedVehicle.MaxUpgrades - 1];

		Fill_Control_Current = SelectedVehicle.PBarControls[SelectedVehicle.ControlLevel];
		Fill_Control_Max = SelectedVehicle.PBarControls[SelectedVehicle.MaxUpgrades - 1];

		Fill_Health_Current = SelectedVehicle.PBarHealth[SelectedVehicle.HealthLevel];
		Fill_Health_Max = SelectedVehicle.PBarHealth[SelectedVehicle.MaxUpgrades - 1];

		Fill_Booster_Current = SelectedVehicle.PBarBooster[SelectedVehicle.BoosterLevel];
		Fill_Booster_Max = SelectedVehicle.PBarBooster[SelectedVehicle.MaxUpgrades - 1];

		T_EngineValue.text = SelectedVehicle.EngineValue[SelectedVehicle.EngineLevel].ToString();
		T_ControlValue.text = SelectedVehicle.ControlsValue[SelectedVehicle.ControlLevel].ToString();
		T_HealthValue.text = SelectedVehicle.HealthValue[SelectedVehicle.HealthLevel].ToString();
		T_BoosterValue.text = (SelectedVehicle.BoosterValue[SelectedVehicle.BoosterLevel] * 10).ToString();

		T_EngineTitle.text = string.Concat("ENGINE - LVL ", (SelectedVehicle.EngineLevel + 1).ToString());
		T_ControlTitle.text = string.Concat("CONTROL - LVL ", (SelectedVehicle.ControlLevel + 1).ToString());
		T_HealthTitle.text = string.Concat("HEALTH - LVL ", (SelectedVehicle.HealthLevel + 1).ToString());
		T_BoosterTitle.text = string.Concat("TURBO - LVL ", (SelectedVehicle.BoosterLevel + 1).ToString());

		if(SelectedVehicle.EngineLevel < SelectedVehicle.MaxUpgrades)
		{
			ButtonEngine.SetActive(true);
			T_EngineDiff.text = string.Concat("+", (SelectedVehicle.EngineValue[SelectedVehicle.EngineLevel + 1] - SelectedVehicle.EngineValue[SelectedVehicle.EngineLevel]).ToString() );
			T_EngineCost.text = SelectedVehicle.EngineCost[SelectedVehicle.EngineLevel + 1].ToString();
		}
		else
		{
			ButtonEngine.SetActive(false);
			T_EngineDiff.text = "MAX";
		}

		if(SelectedVehicle.ControlLevel < SelectedVehicle.MaxUpgrades)
		{
			ButtonControl.SetActive(true);
			T_ControlDiff.text = string.Concat("+", (SelectedVehicle.ControlsValue[SelectedVehicle.ControlLevel + 1] - SelectedVehicle.ControlsValue[SelectedVehicle.ControlLevel]).ToString() );
			T_ControlCost.text = SelectedVehicle.ControlsCost[SelectedVehicle.ControlLevel + 1].ToString();
		}
		else
		{
			ButtonControl.SetActive(false);
			T_ControlDiff.text = "MAX";
		}

		if(SelectedVehicle.HealthLevel < SelectedVehicle.MaxUpgrades)
		{
			ButtonHealth.SetActive(true);
			T_HealthDiff.text = string.Concat("+", (SelectedVehicle.HealthValue[SelectedVehicle.HealthLevel + 1] - SelectedVehicle.HealthValue[SelectedVehicle.HealthLevel]).ToString() );
			T_HealthCost.text = SelectedVehicle.HealthCost[SelectedVehicle.HealthLevel + 1].ToString();
		}
		else
		{
			ButtonHealth.SetActive(false);
			T_HealthDiff.text = "MAX";
		}

		if(SelectedVehicle.BoosterLevel < SelectedVehicle.MaxUpgrades)
		{
			ButtonBooster.SetActive(true);
			T_BoosterDiff.text = string.Concat("+", ( (SelectedVehicle.BoosterValue[SelectedVehicle.BoosterLevel + 1] - SelectedVehicle.BoosterValue[SelectedVehicle.BoosterLevel]) * 10).ToString("f0") );
			T_BoosterCost.text = SelectedVehicle.BoosterCost[SelectedVehicle.BoosterLevel + 1].ToString();
		}
		else
		{
			ButtonBooster.SetActive(false);
			T_BoosterDiff.text = "MAX";
		}
	}

	void OnClickBack_CustomizeWindow()
	{
		C_VehicleStatsWindow.enabled = true;
	}

	void OnClick_CustomizeButton()
	{
		C_VehicleStatsWindow.enabled = false;
	}
}