using System;
using UnityEngine;

public class RangeFunction : MonoBehaviour
{
    public static Action<Transform> TargetLocked;
    public static Action TragetLost;

    [SerializeField] string AiCarString;


    private void OnTriggerEnter(Collider Info)
    {
        Debug.Log("Target locked");
        if (Info.CompareTag(AiCarString))
        {
            TargetLocked?.Invoke(Info.transform);
        }
    }

    private void OnTriggerExit(Collider Info)
    {
        if (Info.CompareTag(AiCarString))
            TragetLost?.Invoke();
    }
}
