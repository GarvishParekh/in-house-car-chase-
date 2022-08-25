using UnityEngine;

public class ShootingSystem : MonoBehaviour
{
    [Header ("Components")]
    [SerializeField] Transform target;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform radiusCollider;

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
            ChangeRadius(radius);
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
}
