using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    [Header ("Values")]
    [SerializeField] int currentScore;
    public int vehicleDestroyedCount = 0;
    [SerializeField] int maxScore = 100;
    public int level = 1;

    [Header ("User inteface")]
    [SerializeField] TMP_Text T_score;
    [SerializeField] TMP_Text T_level;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        AI_CarHealth.AIDestroy += AddScore;
    }

    void AddScore(Transform t)
    {
        currentScore += 50;
        vehicleDestroyedCount += 1;
        T_score.text = currentScore.ToString();
        if (CheckForScore())
        {
            maxScore += 200;
            level += 1;
            T_level.text = $"Level: {level}";
        }
    }

    private bool CheckForScore()
    {
        return currentScore >= maxScore;
    }
}
