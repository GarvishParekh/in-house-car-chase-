using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ShootingSystem : MonoBehaviour
{
    public static ShootingSystem instance;
    public static Action Shoot;

    [SerializeField] TMP_Text fireRateinfo;
    [SerializeField] TMP_Text Damageinfo;

    [Header ("Components")]
    public Transform target;
    public Transform muzzleFlash;
    [SerializeField] Transform radiusCollider;
    [SerializeField] Slider radiusSlider;
    [SerializeField] Slider damageSlider;
    [SerializeField] Slider fireRateSlider;

    [Space]
    [SerializeField] LineRenderer line;

    [Space]
    [Header("Components info")]
    [Range(1f, 20f)]
    [SerializeField] float radius = 1;
    [SerializeField] Vector3 radiusVector;
    [Space]
    [SerializeField] bool changeRadius = false;

    [Header("Gun information")]
    [Range (0.1f, 3f)]
    [SerializeField] float fireRate;
    [SerializeField] float damage;
    [SerializeField] float timer;

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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timer = fireRate;

        radiusVector.x = radius;
        radiusVector.y = radius;
        radiusVector.z = radius;
        radiusCollider.localScale = radiusVector;
    }

    private void Update()
    {
        if (changeRadius is true)
            ChangeRadius(radiusSlider.value);

        fireRate = fireRateSlider.value;
        fireRateinfo.text = fireRate.ToString("0.00");

        damage = damageSlider.value;
        Damageinfo.text = damage.ToString();

        //if (Input.GetKeyDown(KeyCode.Space))
        //_ShootFunction();

        if (target)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                _ShootFunction();
                timer = fireRate;
            }
        }
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
        if (target)
            ShowActionLines();
    }

    void ShowActionLines ()
    {
        ObjectPooler.instance.SpawnObject("Hit", target.position, Quaternion.identity);
        ObjectPooler.instance.SpawnObject("ActionLines", Vector3.zero, Quaternion.identity);
        Shoot?.Invoke();
    }
}
