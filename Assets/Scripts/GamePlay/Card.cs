using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform cardSection;
    [SerializeField] private Image frontBG;
    [SerializeField] private Image frontFruit;
    [SerializeField] private Image backImage;
    [SerializeField] private Button cardButton;

    [Header("Flip Settings")]
    [SerializeField] private float flipDuration = 0.25f;

    private bool isFaceUp;
    private bool isFlipping;

    // 🔹 Expose sprite for matching
    public Sprite FruitSprite => frontFruit.sprite;

    // -------------------------
    // INITIALIZE (RESET STATE)
    // -------------------------
    public void Initialize(int id, Sprite fruitSprite)
    {
        frontFruit.sprite = fruitSprite;

        cardSection.gameObject.SetActive(true);
        cardSection.localScale = Vector3.one;
        cardSection.localRotation = Quaternion.Euler(0f, 180f, 0f);

        frontBG.enabled = true;
        frontFruit.enabled = true;
        backImage.enabled = false;

        isFaceUp = true;
        isFlipping = false;

        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(OnClick);

        SetInteractable(false); // preview controls enabling
    }

    // -------------------------
    // INPUT
    // -------------------------
    private void OnClick()
    {
        if (isFlipping || isFaceUp)
            return;

        StartCoroutine(FlipToFront());
    }

    // -------------------------
    // FLIP TO FRONT
    // -------------------------
    private IEnumerator FlipToFront()
    {
        isFlipping = true;
        SetInteractable(false);

        yield return Rotate(0f, 180f, true);

        isFaceUp = true;
        isFlipping = false;
        SetInteractable(true);

        // ✅ Register AFTER flip completes
        MatchSystem.Instance.RegisterFlip(this);
    }

    // -------------------------
    // FLIP TO BACK (MISMATCH)
    // -------------------------
    public IEnumerator FlipToBack()
    {
        isFlipping = true;
        SetInteractable(false);

        yield return Rotate(180f, 0f, false);

        isFaceUp = false;
        isFlipping = false;
        SetInteractable(true);
    }

    // -------------------------
    // ROTATION CORE
    // -------------------------
    private IEnumerator Rotate(float from, float to, bool showFront)
    {
        float t = 0f;

        while (t < flipDuration)
        {
            float angle = Mathf.Lerp(from, to, t / flipDuration);
            cardSection.localRotation = Quaternion.Euler(0f, angle, 0f);

            if (angle >= 90f)
            {
                frontBG.enabled = showFront;
                frontFruit.enabled = showFront;
                backImage.enabled = !showFront;
            }

            t += Time.deltaTime;
            yield return null;
        }

        cardSection.localRotation = Quaternion.Euler(0f, to, 0f);
        frontBG.enabled = showFront;
        frontFruit.enabled = showFront;
        backImage.enabled = !showFront;
    }

    // -------------------------
    // MATCHED → BURST
    // -------------------------
    public void OnMatched()
    {
        Disable();
        StartCoroutine(BurstAndHide());
    }

    private IEnumerator BurstAndHide()
    {
        float duration = 0.25f;

        Vector3 startScale = cardSection.localScale;
        Vector3 burstScale = startScale * 1.3f;

        float t = 0f;
        while (t < duration)
        {
            cardSection.localScale = Vector3.Lerp(startScale, burstScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        t = 0f;
        while (t < duration)
        {
            cardSection.localScale = Vector3.Lerp(burstScale, Vector3.zero, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        // IMPORTANT: hide visual only (keep grid slot)
        cardSection.gameObject.SetActive(false);

        GameManager.Instance.NotifyCardRemoved();
    }

    // -------------------------
    // PREVIEW RESET
    // -------------------------
    public void ShowBackImmediate()
    {
        cardSection.gameObject.SetActive(true);
        cardSection.localScale = Vector3.one;
        cardSection.localRotation = Quaternion.Euler(0f, 0f, 0f);

        frontBG.enabled = false;
        frontFruit.enabled = false;
        backImage.enabled = true;

        isFaceUp = false;
    }

    // -------------------------
    // UTIL
    // -------------------------
    public void Disable()
    {
        SetInteractable(false);
    }

    public void SetInteractable(bool value)
    {
        cardButton.interactable = value;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
