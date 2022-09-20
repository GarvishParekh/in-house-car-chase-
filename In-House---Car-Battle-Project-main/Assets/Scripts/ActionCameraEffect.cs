using UnityEngine;

public class ActionCameraEffect : MonoBehaviour
{
    [SerializeField] Gyroscope gyro;
    Quaternion rot = new Quaternion(0, 0, 1, 0);

    private void Start()
    {
        //gyro.enabled = true;
    }

    private void Update()
    {
        //transform.rotation = gyro.attitude * rot;
    }
}
