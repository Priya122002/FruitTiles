using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverPanel;

    private int totalCards;
    private int removedCards;
    [SerializeField] private string menuSceneName = "Menu";
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
        gameOverPanel.SetActive(true);
    }
    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}
