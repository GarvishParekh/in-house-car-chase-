using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLinesFunctionEnemies : MonoBehaviour
{
    AI_Car_Shooting shootingInstance;

    [SerializeField] LineRenderer line;
    WaitForSeconds pointOne = new WaitForSeconds(0.1f);

    private void Awake() => shootingInstance = AI_Car_Shooting.instance;

    #region Catch Events
    private void OnEnable()
    {
        AI_Car_Shooting.AI_Shoot += OnShoot;
    }

    private void OnDisable()
    {
        AI_Car_Shooting.AI_Shoot -= OnShoot;
    }
    #endregion

    private void OnShoot()
    {
        Debug.LogWarning("Enemy shoot");
        // set the lines position
        line.SetPosition(0, shootingInstance.muzzleFlash.position);
        line.SetPosition(1, shootingInstance.target.position);

        // disable the lines
        StartCoroutine(nameof(OnShootFunction));
    }

    IEnumerator OnShootFunction()
    {
        yield return pointOne;
        gameObject.SetActive(false);
    }
}
