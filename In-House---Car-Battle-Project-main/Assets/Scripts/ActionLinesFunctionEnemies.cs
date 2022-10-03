using System.Collections;
using UnityEngine;

public class ActionLinesFunctionEnemies : MonoBehaviour
{
    AI_Car_Shooting shootingInstance;

    [SerializeField] LineRenderer line;
    WaitForSeconds pointOne = new WaitForSeconds(0.1f);

    private void Start()
    {
        StartCoroutine(nameof(OnShootFunction));
    }

    IEnumerator OnShootFunction()
    {
        yield return pointOne;
        Destroy(gameObject);
    }
}
