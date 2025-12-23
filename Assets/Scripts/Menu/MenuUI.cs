using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject levelPanel;

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Game";

    private const int DEFAULT_ROWS = 2;
    private const int DEFAULT_COLUMNS = 2;

    private void Awake()
    {
        ShowMenu();
    }

    public void OnSettingsButtonClicked()
    {
        ShowSettings();
        SoundManager.Instance.Play("click");
    }

    public void OnBackButtonClicked()
    {
        ShowMenu();
        SoundManager.Instance.Play("click");
    }

    private void ShowMenu()
    {
        menuPanel.SetActive(true);
        levelPanel.SetActive(false);
    }

    private void ShowSettings()
    {
        menuPanel.SetActive(false);
        levelPanel.SetActive(true);
    }

    public void StartEasy()
    {
        LayoutConfig.SetLayout(2, 2);
        LoadGame();
    }

    public void StartMedium()
    {
        LayoutConfig.SetLayout(4, 4);
        LoadGame();
    }

    public void StartHard()
    {
        LayoutConfig.SetLayout(5, 6);
        LoadGame();
    }

    private void LoadGame()
    {
        SoundManager.Instance.Play("click");
        SceneManager.LoadScene(gameSceneName);
    }
}
