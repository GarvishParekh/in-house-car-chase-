using TMPro;
using System;
using UnityEngine;

public class RangeFunction : MonoBehaviour
{
    public static Action<Transform> TargetLocked;
    public static Action TragetLost;

    [SerializeField] string AiCarString;

    [Header ("Info")]
    [SerializeField] TMP_Text T_targetInfo;



    private void OnTriggerEnter(Collider Info)
    {
        if (Info.CompareTag(AiCarString))
        {
            TargetLocked?.Invoke(Info.transform);
            T_targetInfo.text = ($"Target Locked");
        }
    }

    private void OnTriggerExit(Collider Info)
    {
        if (Info.CompareTag(AiCarString))
        {
            TragetLost?.Invoke();
            T_targetInfo.text = ($"Target Lost");
        }
    }
}
