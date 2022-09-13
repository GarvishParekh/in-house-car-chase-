using UnityEngine;
using System.Collections;

public class ActionLinesFunction : MonoBehaviour
{
    ShootingSystem shootingInstance;

    [SerializeField] LineRenderer line;
    WaitForSeconds pointOne = new WaitForSeconds(0.1f);

    private void Awake() => shootingInstance = ShootingSystem.instance;

    #region Catch Events
    private void OnEnable()
    {
        ShootingSystem.Shoot += OnShoot;
    }

    private void OnDisable()
    {
        ShootingSystem.Shoot -= OnShoot;
    }
    #endregion

    private void OnShoot(Vector3 hitPositon)
    {
        // set the lines position
        line.SetPosition(0, shootingInstance.muzzleFlash.position);
        line.SetPosition(1, hitPositon);

        // disable the lines
        StartCoroutine(nameof(OnShootFunction));
    }

    IEnumerator OnShootFunction ()
    {
        yield return pointOne;
        gameObject.SetActive(false);
    }
}
