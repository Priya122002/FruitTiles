using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    private int totalCards;
    private int removedCards;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        gameOverPanel.SetActive(false);
    }

    public void SetTotalCards(int count)
    {
        totalCards = count;
        removedCards = 0;
    }

    public void NotifyCardRemoved()
    {
        removedCards++;

        if (removedCards >= totalCards)
        {
            ShowGameOver();
        }
    }

    private void ShowGameOver()
    {
        int finalScore = ScoreManager.Instance.GetScore();

        if (finalScoreText != null)
            finalScoreText.text = $"Score : {finalScore}";

        gameOverPanel.SetActive(true);
    }
}
