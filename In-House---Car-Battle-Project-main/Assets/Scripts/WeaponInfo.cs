using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponInfo : MonoBehaviour, IPointerClickHandler
{
    public static Action<int> WeaponButtonSelected;

    [System.Serializable]
    public class Information
    {
        public int index;
        public int price;
        public bool isBought = false;
        public TMP_Text T_price;
        public GameObject alreadyBoughtImage;
        public GameObject coinImage;

        [Header("Image")]
        public Image buttonImage;
        
    }

    public Information information;
    public Sprite normalImage;
    public Sprite highlightedImage;

    private void OnEnable()
    {
        WeaponButtonSelected += GetNormalButton;
        WeaponManager.WeaponBought += UpdateUI;
    }

    void OnDisable ()
    {
        WeaponButtonSelected -= GetNormalButton;
        WeaponManager.WeaponBought -= UpdateUI;
    }

    private void Start()
    {
        UpdateUI();
    }

    void UpdateUI ()
    {
        string weaponPref = $"Weapon{information.index}";
        int boughtInt = PlayerPrefs.GetInt(weaponPref, 0);

        if (boughtInt == 0) information.isBought = false;
        else information.isBought = true;

        if (!information.isBought)
        {
            information.alreadyBoughtImage.SetActive(false);
            information.T_price.text = $"{information.price}";
            information.coinImage.SetActive(true);
        }
        else
        {
            information.alreadyBoughtImage.SetActive(true);
            information.T_price.gameObject.SetActive(false);
            information.coinImage.SetActive(false);
        }
        WeaponButtonSelected?.Invoke(information.index);
    }


    void GetNormalButton (int i)
    {
        information.buttonImage.sprite = normalImage;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        WeaponButtonSelected?.Invoke(information.index);
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
