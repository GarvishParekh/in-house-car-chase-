using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShootingSystem : MonoBehaviour
{
    ObjectPooler objectPool;
    public static ShootingSystem instance;
    public static Action Shoot;

    [Space]
    //[SerializeField] TMP_Text fireRateinfo;
    //[SerializeField] TMP_Text Damageinfo;

    [Header("Components")]
    [SerializeField] LayerMask enemyLayer;
    public Transform target;                    // enemy car to air
    public Vector3 hitPosition;                    // enemy car to air
    public Transform muzzleFlash;               // bullet starting point
    [SerializeField] Transform weaponPlace;
    [SerializeField] Transform radiusCollider;  // radius for finding the enemy

    /*
    [Header ("Sliders ref")]
    [SerializeField] Slider radiusSlider;       
    [SerializeField] Slider fireRateSlider;
    [SerializeField] Slider damageSlider;
    */

    [Header("Enemy Info")]
    public List<Transform> enemies = new List<Transform>();    
    
    [SerializeField] float enemyCheckRate = 20f;
    [SerializeField] float enemyCheckTimer = 20f;

    [Space]
    [Header("Config info")]
    private Vector3 radiusVector;
    [SerializeField] bool changeRadius = false;

    [Header("Weapon Config")]
    [SerializeField] bool canShoot = false;

    [Range(1f, 100f)]
    [SerializeField] float radius = 1;

    [Range (0.1f, 3f)]
    [SerializeField] float fireRate;

    [Range (1, 100)]
    [SerializeField] float damage;
    
    [Range (1f, 20f)]
    [SerializeField] float accuracy;
    [SerializeField] float timer;

    private void OnEnable()
    {
        RangeFunction.TargetLocked += OnTargerLocked;
        RangeFunction.TragetLost += OnTargerLost;
        AI_CarHealth.AIDestroy += ClearTarget;
    }

    private void OnDisable()
    {
        RangeFunction.TargetLocked -= OnTargerLocked;
        RangeFunction.TragetLost -= OnTargerLost;
        AI_CarHealth.AIDestroy -= ClearTarget;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        objectPool = ObjectPooler.instance;
        weaponPlace = GameObject.Find("WeaponPlace").transform;
        transform.parent = weaponPlace;
        transform.position = weaponPlace.position;
        timer = fireRate;

        radiusVector.x = radius;
        radiusVector.y = radius;
        radiusVector.z = radius;
        radiusCollider.localScale = radiusVector;

        GetClosestEnemy();
    }

    private void Update()
    {
        if (changeRadius == true)
            ChangeRadius(radius);

        /*
        damage = damageSlider.value;
        Damageinfo.text = damage.ToString();

        //if (Input.GetKeyDown(KeyCode.Space))
        //_ShootFunction();
        */

        GetEnemy();
        if (target)
        {
            LookATarget(target.position);
            //CheckIfLookingAtTarget();

            if (!canShoot)
                return;

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if(enemies != null)
                _ShootFunction();
                timer = fireRate;
            }
        }
    }

    void GetEnemy()
    {
        enemyCheckTimer -= Time.deltaTime;
        if (enemyCheckTimer <= 0)
        {
            enemyCheckTimer = enemyCheckRate;
            target = GetClosestEnemy();
        }
    }

    Transform GetClosestEnemy()
    {
        if (enemies.Count == 0)
            return null;

        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }

    void CheckIfLookingAtTarget ()
    {
        RaycastHit ray;
        if (Physics.Raycast(muzzleFlash.localPosition, transform.forward, out ray))
        {
            Debug.Log($"{ray.collider.gameObject.name}");
            if (ray.collider.gameObject.name == target.name)
            {
                canShoot = true;
            }else canShoot = false;
        }
    }

    void LookATarget (Vector3 targetVector)
    {
        Vector3 lookVector = targetVector - transform.position;
        Quaternion direction = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Lerp(transform.rotation, direction, accuracy * Time.deltaTime);
    }

    private void ChangeRadius (float L_radius)
    {
        radiusVector.x = L_radius;
        radiusVector.y = L_radius;
        radiusVector.z = L_radius;
        radiusCollider.localScale = radiusVector;
    }

    void OnTargerLocked(Transform l_targert)
    {
        enemies.Add(l_targert);
        target = GetClosestEnemy();
    }


    void OnTargerLost(Transform l_Traget)
    {
        enemies.Remove(l_Traget);
        if (enemies.Count == 0)
        {
            target = null;
        }
    }


    public void _ShootFunction ()
    {
        if (target)
            ShowActionLines();
    }

    void ShowActionLines ()
    {
        RaycastHit ray;
        if (Physics.Raycast(muzzleFlash.position, transform.forward, out ray, 900, enemyLayer))
        {
            Debug.Log("Name:" + ray.collider.name);
            hitPosition = ray.point;
        }
        // spawn hit effect on the enemy car
        objectPool.SpawnObject("Hit", hitPosition, Quaternion.identity, true);

        // spawn action lines from the weapon
        objectPool.SpawnObject("ActionLines", Vector3.zero, Quaternion.identity, true);

        // spawn muzzle flash from the weapon
        objectPool.SpawnObject("MuzzleFlash", muzzleFlash.position, Quaternion.identity, false);
        Shoot?.Invoke();
        target.GetComponentInParent<AI_CarHealth>().OnDamageOccur(20);
    }

    void ClearTarget() => target = null;
}
