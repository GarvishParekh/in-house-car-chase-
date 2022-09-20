using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUiManager : MonoBehaviour
{
    [Header ("Canvas")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject inputPanel;
    [SerializeField] GameObject nullPanel;
    [SerializeField] GameObject settingsPanel;

    private GameObject currentPanel;

    [SerializeField] string HomeScene;
    [SerializeField] string CurrentScene;

    private void Start()
    {
        // close panel
        pauseMenu.SetActive(false);
        nullPanel.SetActive(false);

        // change to input panel
        ChangePanel(inputPanel);
    }

    void ChangePanel (GameObject activePanel)
    {
        if (currentPanel != null)
        {
            currentPanel.SetActive(false);
            Debug.Log($"Panel available");
        }
        else Debug.Log($"No panel available");

        activePanel.SetActive(true);
        currentPanel = activePanel;
    }

    #region Setting panel
    public void B_OpenSettingPanel() => ChangePanel(settingsPanel);

    public void B_CloseSettingPanel() => settingsPanel.SetActive(false);
    #endregion

    void ChangeTime(int timeValue = 0) => Time.timeScale = timeValue;

    void ChangeScene(string sceneName) => SceneManager.LoadScene(sceneName);

    public void B_PauseMenu()
    {
        ChangeTime(0);
        ChangePanel(pauseMenu);
    }

    public void B_Resume ()
    {
        ChangeTime(1);
        ChangePanel(inputPanel);
    }

    public void B_Home ()
    {
        ChangeTime(1);
        ChangeScene(HomeScene);
    }

    public void B_Restart ()
    {
        ChangeTime(1);
        ChangeScene(CurrentScene);
    }
}
