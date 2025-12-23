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
        SoundManager.Instance.Play("game_over");
        SaveManager.ClearGame();

        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SoundManager.Instance.Play("click");

        SceneManager.LoadScene(gameSceneName);

    }

  
    public void GoToMenu()
    {
        SoundManager.Instance.Play("click");
        SceneManager.LoadScene(menuSceneName);
    }
}
