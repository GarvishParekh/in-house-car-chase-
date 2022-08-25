using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShootingSystem : MonoBehaviour
{
    WaitForSeconds pointTwo = new WaitForSeconds(0.1f);
    WaitForSeconds pointFive = new WaitForSeconds(0.5f);

    [Header ("Components")]
    [SerializeField] Transform target;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject actionLines;
    [SerializeField] Transform muzzleFlash;
    [SerializeField] Transform sparkEffect;
    [SerializeField] Transform radiusCollider;
    [SerializeField] Slider radiusSlider;

    [Space]
    [SerializeField] LineRenderer line;

    [Space]
    [Header("Components info")]
    [Range(1f, 20f)]
    [SerializeField] float radius = 1;
    [SerializeField] Vector3 radiusVector;
    [Space]
    [SerializeField] bool changeRadius = false;

    private void OnEnable()
    {
        RangeFunction.TargetLocked += OnTargerLocked;
        RangeFunction.TragetLost += OnTargerLost;
    }

    private void OnDisable()
    {
        RangeFunction.TargetLocked -= OnTargerLocked;
        RangeFunction.TragetLost -= OnTargerLost;
    }

    private void Start()
    {
        radiusVector.x = radius;
        radiusVector.y = radius;
        radiusVector.z = radius;
        radiusCollider.localScale = radiusVector;
    }

    private void Update()
    {
        if (changeRadius is true)
            ChangeRadius(radiusSlider.value);

        if (Input.GetKeyDown(KeyCode.Space))
            _ShootFunction();
    }

    private void ChangeRadius (float L_radius)
    {
        radiusVector.x = L_radius;
        radiusVector.y = L_radius;
        radiusVector.z = L_radius;
        radiusCollider.localScale = radiusVector;
    }

    void OnTargerLocked(Transform l_targert) => target = l_targert;

    void OnTargerLost() => target = null;


    public void _ShootFunction ()
    {
        if (target != null)
            StartCoroutine(nameof(ShowActionLines));
    }

    IEnumerator ShowActionLines ()
    {
        line.SetPosition(0, muzzleFlash.position);
        line.SetPosition(1, target.position);
        sparkEffect.position = target.position;
        sparkEffect.gameObject.SetActive(false);
        sparkEffect.gameObject.SetActive(true);

        actionLines.SetActive(true);

        yield return pointTwo;
        actionLines.SetActive(false);

        yield return pointFive;
        sparkEffect.gameObject.SetActive(false);
    }
}
