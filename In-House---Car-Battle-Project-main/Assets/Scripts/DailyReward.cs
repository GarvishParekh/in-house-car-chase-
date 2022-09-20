using UnityEngine;
using UnityEngine.UI;

public class DailyReward : MonoBehaviour
{
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
    }

    private void Start()
    {
        if (!canCollect)
            return;

        // show daily reward panel
        rewardPanel.SetActive(true);
        // player can collect the specific date's reward 
        rewardButton[rewardCount].interactable = true;
    }
}
