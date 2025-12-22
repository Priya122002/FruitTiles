using UnityEngine;
using TMPro;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI matchesText;
    [SerializeField] private TextMeshProUGUI turnsText;

    private int matches;
    private int turns;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ResetStats();
    }

    public void ResetStats()
    {
        matches = 0;
        turns = 0;
        UpdateUI();
    }

    public void AddMatch()
    {
        matches++;
        UpdateUI();
    }

    public void AddTurn()
    {
        turns++;
        UpdateUI();
    }

    public int GetMatches() => matches;
    public int GetTurns() => turns;

    private void UpdateUI()
    {
        if (matchesText != null)
            matchesText.text = $"Matches\n{matches}";

        if (turnsText != null)
            turnsText.text = $"Turns\n{turns}";
    }
}
