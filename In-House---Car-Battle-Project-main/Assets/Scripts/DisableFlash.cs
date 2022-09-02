using UnityEngine;
using System.Collections;

public class DisableFlash : MonoBehaviour
{
    WaitForSeconds pointOne = new WaitForSeconds(0.1f);

    private void OnEnable()
    {
        StartCoroutine(nameof(DisableObject), gameObject);
    }

    IEnumerator DisableObject (GameObject objToDisable)
    {
        yield return pointOne;
        objToDisable.SetActive(false);
    }
}
