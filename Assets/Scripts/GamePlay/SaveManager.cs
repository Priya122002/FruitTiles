using UnityEngine;

public static class SaveManager
{
    private const string HIGH_SCORE_KEY = "HIGH_SCORE";
    private const string LAST_ROWS_KEY = "LAST_ROWS";
    private const string LAST_COLUMNS_KEY = "LAST_COLUMNS";

    public static void SaveHighScore(int score)
    {
        int currentHigh = GetHighScore();

        if (score > currentHigh)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
            PlayerPrefs.Save();
        }
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    public static void SaveLastLayout(int rows, int columns)
    {
        PlayerPrefs.SetInt(LAST_ROWS_KEY, rows);
        PlayerPrefs.SetInt(LAST_COLUMNS_KEY, columns);
        PlayerPrefs.Save();
    }

    public static bool HasSavedLayout()
    {
        return PlayerPrefs.HasKey(LAST_ROWS_KEY) &&
               PlayerPrefs.HasKey(LAST_COLUMNS_KEY);
    }

    public static void LoadLastLayout(out int rows, out int columns)
    {
        rows = PlayerPrefs.GetInt(LAST_ROWS_KEY, 2);
        columns = PlayerPrefs.GetInt(LAST_COLUMNS_KEY, 2);
    }
}
