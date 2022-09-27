using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public static MainMenuUIManager instance;

    [Header ("Main Menu")]
    [SerializeField] GameObject settingPanel;
    [SerializeField] GameObject achivementPanel;
    [SerializeField] GameObject infoCanvas;
    [SerializeField] GameObject dailyReward;
    [Space]
    [SerializeField] GameObject currentCanvas;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        settingPanel.SetActive(false);
        infoCanvas.SetActive(false);
        achivementPanel.SetActive(false);
    }

    public void _ChangeCanvas (GameObject activeCanvas)
    {
        if (currentCanvas != null)
        {
            currentCanvas.SetActive(false);
        }

        activeCanvas.SetActive(true);
        currentCanvas = activeCanvas;
    }

    public void B_BackButton ()
    {
        if (currentCanvas != null)
            currentCanvas.SetActive(false);
    }

    [Space]
    [Header("Modes")]
    [SerializeField] GameObject[] VehicleCanvas;
    [SerializeField] GameObject[] weaponCanvas;

    public void B_Weapon ()
    {
        for (int i = 0; i < VehicleCanvas.Length; i++)
        {
            VehicleCanvas[i].SetActive(false);
        }

        for (int i = 0; i < weaponCanvas.Length; i++)
        {
            weaponCanvas[i].SetActive(true);
        }
    }

    public void B_Vehicle ()
    {
        for (int i = 0; i < weaponCanvas.Length; i++)
        {
            weaponCanvas[i].SetActive(false);
        }

        for (int i = 0; i < VehicleCanvas.Length; i++)
        {
            VehicleCanvas[i].SetActive(true);
        }
    }
}
