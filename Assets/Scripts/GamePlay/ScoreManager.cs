using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Score Settings")]
    [SerializeField] private int matchScore = 10;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int currentScore;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ResetScore();
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateUI();
    }

    public void AddMatchScore()
    {
        currentScore += matchScore;
        UpdateUI();
    }

    public int GetScore()
    {
        return currentScore;
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score : {currentScore}";
    }
}
