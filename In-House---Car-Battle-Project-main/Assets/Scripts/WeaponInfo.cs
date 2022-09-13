using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponInfo : MonoBehaviour, IPointerClickHandler
{
    public static Action<int, int> WeaponButtonSelected;

    [System.Serializable]
    public class Information
    {
        public string name;
        public int index;
        public int price;
        public bool isBought = false;
        public GameObject lockImage;

        [Header("Image")]
        public Image buttonImage;
        
    }

    public TMP_Text T_weaponName;
    public Information information;
    public Sprite normalImage;
    public Sprite highlightedImage;

    private void OnEnable()
    {
        WeaponButtonSelected += GetNormalButton;
        WeaponManager.WeaponBought += normalAIupdate;
    }

    void OnDisable ()
    {
        WeaponButtonSelected -= GetNormalButton;
        WeaponManager.WeaponBought -= normalAIupdate;
    }

    private void Start()
    {
        UpdateUI(false);
    }

    void UpdateUI (bool _justBought)
    {
        string weaponPref = $"Weapon{information.index}";
        int boughtInt = PlayerPrefs.GetInt(weaponPref, 0);

        if (boughtInt == 0) information.isBought = false;
        else information.isBought = true;

        if (!information.isBought)
        {
            information.lockImage.SetActive(true);
        }
        else
        {
            information.lockImage.SetActive(false);
        }
        WeaponButtonSelected?.Invoke(information.index, information.price);
    }

    void normalAIupdate(bool b)
    {
        string weaponPref = $"Weapon{information.index}";
        int boughtInt = PlayerPrefs.GetInt(weaponPref, 0);

        if (boughtInt == 0) information.isBought = false;
        else information.isBought = true;

        if (!information.isBought)
        {
            information.lockImage.SetActive(true);
        }
        else
        {
            information.lockImage.SetActive(false);
        }
    }


    void GetNormalButton (int i, int j)
    {
        information.buttonImage.sprite = normalImage;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        WeaponButtonSelected?.Invoke(information.index, information.price);
        information.buttonImage.sprite = highlightedImage;
    }

    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.P))
        {
            Debug.LogWarning($"Playerpref deleted");
            PlayerPrefs.DeleteAll();
        }
    }
}
