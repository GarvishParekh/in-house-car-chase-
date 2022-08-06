using UnityEngine;

public class VehicleData : MonoBehaviour 
{
	[Space]
	[Header("Storage & Display")]

	public string DisplayName;
	public int StorageIndex;
	public Sprite S_Silhouette;

	[Space]
	[Header("Configurations")]
	[Space]
	[Space]

	public int[] EngineValue;
	public int[] ControlsValue;
	public int[] HealthValue;
	public float[] BoosterValue;

	[Space]
	public int[] AccelerationKick;
	public int[] BrakesKick;
	public int[] BoosterCharge;
	public int[] BoosterDischarge;

	[Space]
	[Header("Progress Bar Fill Value")]
	[Space]
	[Space]

	public float[] PBarEngine;
	public float[] PBarControls;
	public float[] PBarHealth;
	public float[] PBarBooster;

	[Space]
	[Header("Costs of Upgrade")]
	[Space]
	[Space]

	public int VehicleCost;
	public int[] EngineCost;
	public int[] HealthCost;
	public int[] ControlsCost;
	public int[] BoosterCost;
	public int MaxUpgrades;

	[Space]
	[Header("Skins")]
	[Space]
	[Space]

	public Texture[] VehicleSkins;
	public Sprite[] SkinSprites;
	public int[] SkinStorageID;
	public int SkinCost;
	public int TotalSkins;

	[Space]
	[Header("Current Status")]
	[Space]
	[Space]

	public bool isPurchased;

	public int EngineLevel;
	public int HealthLevel;
	public int ControlLevel;
	public int BoosterLevel;

	public int[] SkinsPurchaseOrder;

	private int i;
	private string S;

	public Renderer[] RenderObjs;

	public void GetData_Configurations()
	{
		if(PlayerPrefs.GetInt(string.Concat("VehiclePurchased_", StorageIndex.ToString()), 0) == 1)
		{
			isPurchased = true;
		}

		EngineLevel = PlayerPrefs.GetInt(string.Concat("Vehicle_", StorageIndex.ToString(), "_EngineLevel"), 0);
		HealthLevel = PlayerPrefs.GetInt(string.Concat("Vehicle_", StorageIndex.ToString(), "_HealthLevel"), 0);
		ControlLevel = PlayerPrefs.GetInt(string.Concat("Vehicle_", StorageIndex.ToString(), "_ControlLevel"), 0);
		BoosterLevel = PlayerPrefs.GetInt(string.Concat("Vehicle_", StorageIndex.ToString(), "_BoosterLevel"), 0);
	}
}