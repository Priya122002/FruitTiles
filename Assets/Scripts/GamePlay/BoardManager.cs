using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private RectTransform boardRect;
    [SerializeField] private Card cardPrefab;

    [Header("Layout")]
    [SerializeField] private float spacing = 10f;

    [Header("Card Sprites")]
    [SerializeField] private List<Sprite> cardIcons;

    private readonly List<Card> spawnedCards = new List<Card>();

    private void Start()
    {
        int rows = LayoutConfig.Rows;
        int columns = LayoutConfig.Columns;

        if (rows <= 0 || columns <= 0)
        {
            rows = 2;
            columns = 2;
            LayoutConfig.SetLayout(rows, columns);
        }

        GenerateBoard(rows, columns);
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
        yield return new WaitForSeconds(2f);

        foreach (Card card in spawnedCards)
        {
            card.ShowBackImmediate();
            card.SetInteractable(true);
        }
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
