using UnityEngine;

public class Car_Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] cars;

    private void Start()
    {
        int selectedCar = PlayerPrefs.GetInt("SelectedVehile", 0);
        Instantiate(cars[selectedCar], Vector3.zero, Quaternion.identity);
    }
}
