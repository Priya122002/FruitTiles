using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }
    [Header("References")]
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private RectTransform boardRect;
    [SerializeField] private Card cardPrefab;

    [Header("Layout")]
    [SerializeField] private float spacing = 10f;

    [Header("Card Sprites")]
    [SerializeField] private List<Sprite> cardIcons;

    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float countdownScaleFrom = 1.5f;
    [SerializeField] private float countdownAnimDuration = 0.4f;


    private readonly List<Card> spawnedCards = new List<Card>();
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (SaveManager.HasSavedGame())
        {
            LoadSavedBoard();
        }
        else
        {
            CreateNewBoard();
        }
    }

    private void CreateNewBoard()
    {
        int rows = LayoutConfig.Rows;
        int columns = LayoutConfig.Columns;

        GenerateBoard(rows, columns);
    }
    public void SaveBoardState()
    {
        GameSaveData data = new GameSaveData
        {
            rows = LayoutConfig.Rows,
            columns = LayoutConfig.Columns,
            score = ScoreManager.Instance.GetScore(),
            turns = GameStatsManager.Instance.GetTurns(),
            matches = GameStatsManager.Instance.GetMatches(),
            cardIds = new List<int>(),
            cardRemoved = new List<bool>()
        };

        foreach (Card card in spawnedCards)
        {
            data.cardIds.Add(card.CardId);
            data.cardRemoved.Add(card.IsRemoved);
        }

        SaveManager.SaveGame(data);
    }
    private void LoadSavedBoard()
    {
        GameSaveData data = SaveManager.LoadGame();

        LayoutConfig.SetLayout(data.rows, data.columns);
        ConfigureGrid(data.rows, data.columns);

        spawnedCards.Clear();

        for (int i = 0; i < data.cardIds.Count; i++)
        {
            int id = data.cardIds[i];
            Sprite icon = cardIcons[id % cardIcons.Count];

            Card card = Instantiate(cardPrefab, grid.transform);
            card.Initialize(id, icon);

            if (data.cardRemoved[i])
            {
                card.RestoreAsMatched();
            }
            else
            {
                card.ShowBackImmediate();
                card.SetInteractable(true);  
            }


            spawnedCards.Add(card);
        }

        ScoreManager.Instance.SetScore(data.score);
        GameStatsManager.Instance.SetStats(data.turns, data.matches);

        GameManager.Instance.SetTotalCards(spawnedCards.Count);

        int removedCount = 0;
        foreach (bool removed in data.cardRemoved)
        {
            if (removed) removedCount++;
        }

        for (int i = 0; i < removedCount; i++)
        {
            GameManager.Instance.NotifyCardRemoved();
        }
    }


    public void GenerateBoard(int rows, int columns)
    {
        int totalCards = rows * columns;

        if (totalCards % 2 != 0)
            columns++;

        ConfigureGrid(rows, columns);
        SpawnCards(rows, columns);
    }

    private void ConfigureGrid(int rows, int columns)
    {
        float width = boardRect.rect.width;
        float height = boardRect.rect.height;

        float cellW = (width - (columns - 1) * spacing) / columns;
        float cellH = (height - (rows - 1) * spacing) / rows;
        float size = Mathf.Min(cellW, cellH);

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.cellSize = new Vector2(size, size);
        grid.spacing = Vector2.one * spacing;
    }

    private void SpawnCards(int rows, int columns)
    {
        spawnedCards.Clear();

        int pairCount = (rows * columns) / 2;
        List<CardData> dataList = new List<CardData>();

        for (int i = 0; i < pairCount; i++)
        {
            Sprite icon = cardIcons[i % cardIcons.Count];

            CardData data = new CardData
            {
                id = i,
                icon = icon
            };

            dataList.Add(data);
            dataList.Add(data);
        }

        Shuffle(dataList);

        foreach (CardData data in dataList)
        {
            Card card = Instantiate(cardPrefab, grid.transform);
            card.Initialize(data.id, data.icon);
            spawnedCards.Add(card);
        }

        GameManager.Instance.SetTotalCards(spawnedCards.Count);

        StartCoroutine(PreviewRoutine());
    }

    private IEnumerator PreviewRoutine()
    {
        foreach (Card card in spawnedCards)
            card.SetInteractable(false);

        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            SoundManager.Instance.Play("time_tick");
            yield return AnimateCountdown();
        }

        countdownText.gameObject.SetActive(false);

        foreach (Card card in spawnedCards)
            card.ShowBackImmediate();

        foreach (Card card in spawnedCards)
            card.SetInteractable(true);
    }
    private IEnumerator AnimateCountdown()
    {
        float t = 0f;

        Vector3 startScale = Vector3.one * countdownScaleFrom;
        Vector3 endScale = Vector3.one;

        countdownText.transform.localScale = startScale;

        while (t < countdownAnimDuration)
        {
            t += Time.deltaTime;
            float lerp = t / countdownAnimDuration;

            lerp = Mathf.SmoothStep(0f, 1f, lerp);

            countdownText.transform.localScale = Vector3.Lerp(startScale, endScale, lerp);
            yield return null;
        }

        countdownText.transform.localScale = endScale;

        yield return new WaitForSeconds(0.6f);
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            (list[i], list[r]) = (list[r], list[i]);
        }
    }
}
