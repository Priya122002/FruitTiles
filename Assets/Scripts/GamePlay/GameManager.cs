using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI finalScoreText;

    [Header("Scenes")]
    [SerializeField] private string menuSceneName = "Menu";
    [SerializeField] private string gameSceneName = "Game";

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

        SaveManager.SaveHighScore(finalScore);
        SaveManager.SaveLastLayout(LayoutConfig.Rows, LayoutConfig.Columns);

        if (finalScoreText != null)
            finalScoreText.text = $"Score : {finalScore}";

        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

  
    public void GoToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
