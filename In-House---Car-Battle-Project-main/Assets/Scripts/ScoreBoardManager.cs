using TMPro;
using UnityEngine;
using System.Collections;

public class ScoreBoardManager : MonoBehaviour
{

    ScoreManager scoreInstance;

    [Header("Panels")]
    [SerializeField] GameObject scoreBoardCanvas;
    [SerializeField] GameObject ingameCanvas;
    [SerializeField] GameObject inputCanvas;

    [Header("Text fields")]
    [SerializeField] TMP_Text T_timeSurvived;
    [SerializeField] TMP_Text T_vehicleDestroyed;
    [SerializeField] TMP_Text T_XPGained;

    [Header("Values")]
    [SerializeField] int timeSurvived;
    [SerializeField] int vehicleDestoryed;
    [SerializeField] int xpGained;

    private void Start() => scoreInstance = ScoreManager.instance;

    // listening for events
    private void OnEnable() => PlayerCarHealth.GameOver += OnGameOver;

    private void OnDisable() => PlayerCarHealth.GameOver -= OnGameOver;

    // game over function
    void OnGameOver() => StartCoroutine(nameof(GameOverFunction));

    IEnumerator GameOverFunction ()
    {
        // load all the data from the game
        vehicleDestoryed = scoreInstance.vehicleDestroyedCount;
        xpGained = vehicleDestoryed * 50;

        // update the user interface
        T_timeSurvived.text = timeSurvived.ToString();
        T_vehicleDestroyed.text = vehicleDestoryed.ToString();
        T_XPGained.text = xpGained.ToString();

        yield return new WaitForSeconds(2);
        // set the panel active on game over
        scoreBoardCanvas.SetActive(true);
        ingameCanvas.SetActive(false);
    }
}
