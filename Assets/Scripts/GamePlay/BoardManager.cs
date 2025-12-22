using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private RectTransform boardRect;

    [Header("Layout")]
    [SerializeField] private float spacing = 10f;

    private void Start()
    {
        GenerateBoard(LayoutConfig.Rows, LayoutConfig.Columns);
    }

    public void GenerateBoard(int rows, int columns)
    {
        int totalCards = rows * columns;

        if (totalCards % 2 != 0)
        {
            Debug.LogWarning("Odd card count detected. Adjusting layout.");
            columns += 1;
            totalCards = rows * columns;
        }

        ConfigureGrid(rows, columns);
        SpawnCards(rows, columns);
    }

    private void ConfigureGrid(int rows, int columns)
    {
        float width = boardRect.rect.width;
        float height = boardRect.rect.height;

        float cellWidth = (width - (columns - 1) * spacing) / columns;
        float cellHeight = (height - (rows - 1) * spacing) / rows;

        float cellSize = Mathf.Min(cellWidth, cellHeight);

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.cellSize = new Vector2(cellSize, cellSize);
        grid.spacing = Vector2.one * spacing;
    }

    private void SpawnCards(int rows, int columns)
    {
        int pairCount = (rows * columns) / 2;
        List<int> ids = new List<int>();

        for (int i = 0; i < pairCount; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        Shuffle(ids);

        foreach (int id in ids)
        {
            Card card = Instantiate(cardPrefab, grid.transform);
            card.Initialize(id);
        }
    }

    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
