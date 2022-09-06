using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    public static Action WeaponBought;

    [SerializeField] int selectedIndex = 0;
    [SerializeField] string selectedName;

    [Header("Buttons")]
    [SerializeField] GameObject B_buy;
    [SerializeField] GameObject B_equip;

    [Header("Weapons")]
    [SerializeField] GameObject[] weapons;
    [SerializeField] GameObject selectedWeapon = null;

    private void OnEnable()
    {
        WeaponInfo.WeaponButtonSelected += OnWeaponSelceted;
    }

    private void OnDisable()
    {
        WeaponInfo.WeaponButtonSelected -= OnWeaponSelceted;
    }

    private void Start()
    {
        // -1 if no weapon is selected at the time
        int _SelectedWeapons = PlayerPrefs.GetInt("Selected Weapon", -1);
        if (_SelectedWeapons != -1)
        {
            weapons[_SelectedWeapons].SetActive(true);
            selectedWeapon = weapons[_SelectedWeapons];
        }
    }

    void OnWeaponSelceted(int _weaponIndex)
    {
        selectedIndex = _weaponIndex;

        // name for storing the prefab
        string weaponPref = $"Weapon{_weaponIndex}";        
        selectedName = weaponPref;
        int boughtInt = PlayerPrefs.GetInt(weaponPref, 0);
        bool _isBought = false;         // check if the weapon is bought or not 

        if (boughtInt == 0) _isBought = false;
        else _isBought = true;

        if (_isBought)
        {
            B_equip.SetActive(true);
            B_buy.SetActive(false);
        }
        else if (!_isBought)
        {
            B_equip.SetActive(false);
            B_buy.SetActive(true);
        }
    }

    #region Button Inputs
    public void B_BuyButton ()
    {
        PlayerPrefs.SetInt(selectedName, 1);
        WeaponBought?.Invoke();
    }

    public void B_Equip()
    {
        if (selectedWeapon != null)
        {
            selectedWeapon.SetActive(false);
        }
        weapons[selectedIndex].SetActive(true);
        selectedWeapon = weapons[selectedIndex];
        PlayerPrefs.SetInt("Selected Weapon", selectedIndex);
    }
    #endregion

    public void B_OnOpenPanel ()
    {
        B_buy.SetActive(false);
        B_equip.SetActive(false);
    }
}
