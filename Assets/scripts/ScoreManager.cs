using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int CurrentScore { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI scoreText;

    [Header("Points")]
    public int correctOrderPoints = 100;
    public int wrongOrderPenalty = 50;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        UpdateUI();
    }

    public void AddScore(int points)
    {
        CurrentScore += points;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + CurrentScore.ToString();
    }
}
