using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RewardCardInfo : MonoBehaviour
{
    public static Action RewardCollected;

    [System.Serializable]
    class CardInfo
    {
        // 0 = not collected ; 1 = collected
        public string collectedPref;
        public string dayInfo;
        public string rewardName;
        public bool isCollected = false;
        [Space]
        public int index;
        public int amount;
        public Image cardImage;
        public GameObject B_collect;
        public GameObject tickMark;
    }
    [SerializeField] CardInfo cardInfo;

    [Header("User interface")]
    [SerializeField] Sprite rewardImage;
    [SerializeField] TMP_Text T_dayInfo;
    [SerializeField] TMP_Text T_rewardName;
    [SerializeField] TMP_Text T_amountInfo;

    private void Awake()
    {
        // give UI the info
        T_dayInfo.text = cardInfo.dayInfo;
        T_rewardName.text = cardInfo.rewardName;
        T_amountInfo.text = cardInfo.amount.ToString();
        cardInfo.cardImage.sprite = rewardImage;

        string completePref = cardInfo.collectedPref + cardInfo.index.ToString();
        // 0 = not collected ; 1 = collected
        int pref = PlayerPrefs.GetInt(completePref, 0);
        if (pref == 1)
            cardInfo.isCollected = true;
        else if (pref == 0)
            cardInfo.isCollected = false;
    }

    public void B_RewardCollected() => RewardCollected?.Invoke();
}
