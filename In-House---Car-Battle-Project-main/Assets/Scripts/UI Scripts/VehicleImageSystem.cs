using TMPro;
using UnityEngine;

public class VehicleImageSystem : MonoBehaviour
{
    public static VehicleImageSystem instance;

    public Sprite[] vehicleIcons;
    public string[] vehicleName;

    private void Awake()
    {
        instance = this;
    }
}
