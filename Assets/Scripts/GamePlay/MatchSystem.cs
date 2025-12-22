using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSystem : MonoBehaviour
{
    public static MatchSystem Instance { get; private set; }

    [SerializeField] private float mismatchDelay = 0.6f;

    private readonly List<Card> flipped = new List<Card>();
    private bool isComparing;

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterFlip(Card card)
    {
        if (flipped.Contains(card))
            return;

        flipped.Add(card);

        if (!isComparing && flipped.Count >= 2)
            StartCoroutine(CompareRoutine());
    }

    private IEnumerator CompareRoutine()
    {
        isComparing = true;

        while (flipped.Count >= 2)
        {
            Card a = flipped[0];
            Card b = flipped[1];

            if (a.CardId == b.CardId)
            {
                a.OnMatched();
                b.OnMatched();
            }

            else
            {
                yield return new WaitForSeconds(mismatchDelay);
                yield return a.FlipToBack();
                yield return b.FlipToBack();
            }

            flipped.RemoveRange(0, 2);
            yield return null;
        }

        isComparing = false;
    }
}
