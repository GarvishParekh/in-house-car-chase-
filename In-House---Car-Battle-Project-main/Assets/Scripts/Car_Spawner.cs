using UnityEngine;

public class Car_Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] cars;

    private void Awake()
    {
        int selectedCar = PlayerPrefs.GetInt("SelectedVehicle", 0);
        Instantiate(cars[selectedCar], Vector3.zero, Quaternion.identity);
    }
}
