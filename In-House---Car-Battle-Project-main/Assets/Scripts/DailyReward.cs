using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
    public static Action<int> CollectCoint;
    bool canCollect = false;

    [Header("Reward information")]
    [SerializeField] GameObject rewardPanel;
    [SerializeField] string rewardPref;
    [SerializeField] int rewardCount;
    [SerializeField] Button[] rewardButton;

    [Space]
    [Header("Date information")]
    [SerializeField] string datePref;
    [SerializeField] int currenDate;
    [SerializeField] int lastRewardDate = 0;

    private void Awake()
    {
        // get info about last date 
        lastRewardDate = PlayerPrefs.GetInt(datePref, 0);
        rewardPanel.SetActive(false);

        // get into for the current date 
        currenDate = int.Parse(System.DateTime.Now.Date.ToString("dd"));
        // if the last date is not equal to the current date
        if (lastRewardDate != currenDate)
            canCollect = true;
        else
            canCollect = false;

        // disable all the buttons at the start
        DisableButtons();
    }

    private void OnEnable()
    {
        RewardCardInfo.RewardCollected += RewardCollected;
    }

    private void OnDisable()
    {
        RewardCardInfo.RewardCollected -= RewardCollected;
    }

    private void Start()
    {
        rewardCount = PlayerPrefs.GetInt(rewardPref, 0);
        if (rewardCount >= rewardButton.Length)
            return;
        if (!canCollect)
            return;

        MainMenuUIManager.instance._ChangeCanvas(rewardPanel);
        MainMenuUIManager.instance.currentCanvas = rewardPanel;
        // show daily reward panel
        // player can collect the specific date's reward
        rewardButton[rewardCount].interactable = true;
    }

    void DisableButtons ()
    {
        for (int i = 0; i < rewardButton.Length; i++)
        {
            rewardButton[i].interactable = false;
        }
    }

    void RewardCollected ()
    {
        rewardCount++;
        PlayerPrefs.SetInt(rewardPref, rewardCount);
        DisableButtons();
        PlayerPrefs.SetInt(datePref, currenDate);
    }

    public void B_CoinsReward (int _Coins)
    {
        // update in coint UI
        CollectCoint?.Invoke(_Coins);  // add 500 coins
    }




    //----------------------------------------------------------
    private void Update()
    {
        if (Input.GetKeyDown (KeyCode.Space))
        {
            PlayerPrefs.DeleteAll();
            Debug.LogAssertion("Player pref deleted");
        }
    }
}
