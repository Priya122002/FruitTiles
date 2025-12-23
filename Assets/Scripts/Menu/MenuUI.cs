using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject continuePanel;


    [Header("Scene")]
    [SerializeField] private string gameSceneName = "Game";

    private const int DEFAULT_ROWS = 2;
    private const int DEFAULT_COLUMNS = 2;
    [SerializeField] private TextMeshProUGUI continueInfoText;

    private void Awake()
    {
        if (SaveManager.HasSavedGame())
            ShowContinue();
        else
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
        HideAllPanels();
        menuPanel.SetActive(true);
    }
    private void ShowSettings()
    {
        HideAllPanels();
        levelPanel.SetActive(true);
    }


    private string GetLevelName(int rows, int columns)
    {
        if (rows == 2 && columns == 2) return "Easy";
        if (rows == 4 && columns == 4) return "Medium";
        if (rows == 5 && columns == 6) return "Hard";
        return "Custom";
    }

    private void ShowContinue()
    {
        HideAllPanels();
        continuePanel.SetActive(true);

        GameSaveData data = SaveManager.LoadGame();
        string levelName = GetLevelName(data.rows, data.columns);

        continueInfoText.text =
            $"Level : {levelName} ({data.rows} x {data.columns})\n" +
            $"Score : {data.score}\n" +
            $"Matches : {data.matches}\n" +
            $"Turns : {data.turns}";
    }


    public void ContinueGame()
    {
        if (SaveManager.HasSavedGame())
        {
            SoundManager.Instance.Play("click");
            SceneManager.LoadScene(gameSceneName);
        }
    }
    public void StartNewGame()
    {
        SaveManager.ClearGame();
        ShowMenu();
    }

    public void StartEasy()
    {
        SaveManager.ClearGame();      
        LayoutConfig.SetLayout(2, 2);
        LoadGame();
    }

    public void StartMedium()
    {
        SaveManager.ClearGame();       
        LayoutConfig.SetLayout(4, 4);
        LoadGame();
    }

    public void StartHard()
    {
        SaveManager.ClearGame();      
        LayoutConfig.SetLayout(5, 6);
        LoadGame();
    }


    private void LoadGame()
    {
        SoundManager.Instance.Play("click");
        SceneManager.LoadScene(gameSceneName);
    }
    private void HideAllPanels()
    {
        menuPanel.SetActive(false);
        levelPanel.SetActive(false);
        continuePanel.SetActive(false);
    }

}
