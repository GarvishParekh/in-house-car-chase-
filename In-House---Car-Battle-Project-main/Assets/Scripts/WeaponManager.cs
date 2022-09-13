using TMPro;
using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    public static Action<bool> WeaponBought;

    [SerializeField] int selectedIndex = 0;
    [SerializeField] int selecterPrice = 0;
    [SerializeField] string selectedName;

    [Header("Buttons")]
    [SerializeField] GameObject B_buy;
    [SerializeField] GameObject B_equip;
    [SerializeField] TMP_Text T_price; 

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

    void OnWeaponSelceted(int _weaponIndex, int _weaponPrice)
    {
        selectedIndex = _weaponIndex;
        Debug.Log(_weaponPrice);
        T_price.text = $"{_weaponPrice}";
        selecterPrice = _weaponPrice;

        // name for storing the prefab
        string weaponPref = $"Weapon{_weaponIndex}";        
        selectedName = weaponPref;
        int boughtInt = PlayerPrefs.GetInt(weaponPref, 0);
        bool _isBought = false;         // check if the weapon is bought or not 

        if (boughtInt == 0) _isBought = false;
        else if (boughtInt == 1) _isBought = true;
        else if (boughtInt == 2)
        {
            B_equip.SetActive(false);
            B_buy.SetActive(false);
        }

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
        WeaponBought?.Invoke(true);
        B_Equip();
        OnWeaponSelceted(selectedIndex, selecterPrice);
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
        PlayerPrefs.SetInt(selectedName, 2);
    }
    #endregion

    public void B_OnOpenPanel ()
    {
        B_buy.SetActive(false);
        B_equip.SetActive(false);
    }
}
