using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject settingsPanel;

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
    }

    public void OnBackButtonClicked()
    {
        ShowMenu();
    }

    private void ShowMenu()
    {
        menuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    private void ShowSettings()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
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

    public void LoadGameDefault()
    {
        if (SaveManager.HasSavedLayout())
        {
            SaveManager.LoadLastLayout(out int rows, out int columns);
            LayoutConfig.SetLayout(rows, columns);
        }
        else
        {
            LayoutConfig.SetLayout(2, 2);
        }

        SceneManager.LoadScene(gameSceneName);
    }


    private void LoadGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
