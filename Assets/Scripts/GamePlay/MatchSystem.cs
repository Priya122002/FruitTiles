using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSystem : MonoBehaviour
{
    public static MatchSystem Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private float mismatchDelay = 0.6f;

    private readonly List<Card> flippedCards = new List<Card>();
    private bool isComparing;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterFlip(Card card)
    {
        if (card == null || flippedCards.Contains(card))
            return;

        flippedCards.Add(card);

        if (!isComparing && flippedCards.Count >= 2)
        {
            StartCoroutine(CompareRoutine());
        }
    }

    private IEnumerator CompareRoutine()
    {
        isComparing = true;

        while (flippedCards.Count >= 2)
        {
            Card first = flippedCards[0];
            Card second = flippedCards[1];

            if (first.CardId == second.CardId)
            {
                first.Disable();
                second.Disable();


                flippedCards.RemoveRange(0, 2);
            }
            else
            {

                yield return new WaitForSeconds(mismatchDelay);

                first.ShowBack();
                second.ShowBack();

                flippedCards.RemoveRange(0, 2);
            }

            yield return null; // allows continuous input
        }

        isComparing = false;
    }
}
