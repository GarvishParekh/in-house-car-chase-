using System;
using UnityEngine;

public class AI_Car_Shooting : MonoBehaviour
{
    ObjectPooler objectPool;

    //[SerializeField] ObjectPooler ActionLinesPool;
    public static AI_Car_Shooting instance;
    public static Action AI_Shoot;

    [Space]
    //[SerializeField] TMP_Text fireRateinfo;
    //[SerializeField] TMP_Text Damageinfo;

    [Header("Components")]
    [SerializeField] LayerMask enemyLayer;
    public Transform target;                    // enemy car to air
    public Vector3 hitPosition;                 // enemy car to air
    public Transform muzzleFlash;               // bullet starting point
    [SerializeField] Vector3 hitPoint;
    [SerializeField] Transform actionLines;     // action lines for indicating enemy is shooting

    /*
    [Header ("Sliders ref")]
    [SerializeField] Slider radiusSlider;       
    [SerializeField] Slider fireRateSlider;
    [SerializeField] Slider damageSlider;
    */

    [Header("Enemy Info")]
    [SerializeField] float enemyCheckRate = 20f;
    [SerializeField] float enemyCheckTimer = 20f;

    [Space]
    [Header("Weapon Config")]
    [SerializeField] bool canShoot = false;

    [Range(1f, 100f)]
    [SerializeField] float radius = 1;
    [Range(1f, 1000f)]
    [SerializeField] float range;

    [Range(0.1f, 3f)]
    [SerializeField] float fireRate;

    [Range(1, 100)]
    [SerializeField] int damage;

    [Range(1f, 20f)]
    [SerializeField] float accuracy;
    [SerializeField] float timer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        objectPool = ObjectPooler.instance;
        timer = fireRate;
    }

    void ChangeWeaponSettings(int _weaponSelected)
    {
        if (_weaponSelected == 0)
        {
            fireRate = 0.85f;
            damage = 5;
        }
        else if (_weaponSelected == 1)
        {
            fireRate = 0.3f;
            damage = 3;
        }
        else if (_weaponSelected == 2)
        {
            fireRate = 0.66f;
            damage = 12;
        }
        else if (_weaponSelected == 3)
        {
            fireRate = 1.2f;
            damage = 17;
        }
    }

    private void Update()
    {
        /*
        damage = damageSlider.value;
        Damageinfo.text = damage.ToString();

        //if (Input.GetKeyDown(KeyCode.Space))
        //_ShootFunction();
        */

        if (target)
        {
            LookATarget(target.position);
            //CheckIfLookingAtTarget();

            CheckIfLookingAtTarget();
            timer -= Time.deltaTime;
            if (!canShoot)
                return;

            if (timer <= 0)
            {
                _ShootFunction();
                timer = fireRate;
            }
        }

        else
        {
            lerpSpeed = Mathf.MoveTowards(0, 1, 3 * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, 0, 0), lerpSpeed);
        }
    }
    float lerpSpeed = 0;

    void CheckIfLookingAtTarget()
    {
        RaycastHit ray = new RaycastHit();
        if (Physics.Raycast(muzzleFlash.position, transform.forward, out ray, Mathf.Infinity))
        {
            if (ray.collider.gameObject.CompareTag("Player"))
            {
                canShoot = true;
                hitPoint = ray.point;
            }
            else
            {
                canShoot = false;
            }
        }
    }

    void LookATarget(Vector3 targetVector)
    {
        Vector3 lookVector = targetVector - transform.position;
        Quaternion direction = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Lerp(transform.rotation, direction, accuracy * Time.deltaTime);
    }

    public void _ShootFunction()
    {
        if (target)
            ShowActionLines();
    }

    void ShowActionLines()
    {
        // spawn hit effect on the enemy car
        objectPool.SpawnObject("Hit", hitPoint, Quaternion.identity, true);

        // spawn action lines from the weapon
        LineRenderer _actionLines = Instantiate(actionLines, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();

        if (_actionLines)
        {
            _actionLines.SetPosition(0, muzzleFlash.position);
            _actionLines.SetPosition(1, hitPoint);
        }
        

        // spawn muzzle flash from the weapon
        objectPool.SpawnObject("MuzzleFlash", muzzleFlash.position, Quaternion.identity, false);
        AI_Shoot?.Invoke();
    }
}
