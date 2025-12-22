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

    private int cardId;
    private bool isFaceUp;
    private bool isFlipping;

    public int CardId => cardId;

    public void Initialize(int id, Sprite fruitSprite)
    {
        cardId = id;
        frontFruit.sprite = fruitSprite;

        cardButton.onClick.RemoveAllListeners();
        cardButton.onClick.AddListener(OnClick);

        ShowFrontImmediate();   
        SetInteractable(false);
    }

    private void OnClick()
    {
        if (isFlipping || isFaceUp)
            return;

        StartCoroutine(FlipToFront());
        MatchSystem.Instance.RegisterFlip(this);
    }

    private IEnumerator FlipToFront()
    {
        isFlipping = true;
        SetInteractable(false);

        yield return Rotate(0f, 180f, true);

        isFaceUp = true;
        isFlipping = false;
        SetInteractable(true);
    }

    public IEnumerator FlipToBack()
    {
        isFlipping = true;
        SetInteractable(false);

        yield return Rotate(180f, 0f, false);

        isFaceUp = false;
        isFlipping = false;
        SetInteractable(true);
    }

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

    public void ShowFrontImmediate()
    {
        cardSection.localRotation = Quaternion.Euler(0f, 180f, 0f);
        frontBG.enabled = true;
        frontFruit.enabled = true;
        backImage.enabled = false;
        isFaceUp = true;
    }

    public void ShowBackImmediate()
    {
        cardSection.localRotation = Quaternion.Euler(0f, 0f, 0f);
        frontBG.enabled = false;
        frontFruit.enabled = false;
        backImage.enabled = true;
        isFaceUp = false;
    }

    public void Disable()
    {
        SetInteractable(false);
    }

    public void SetInteractable(bool value)
    {
        cardButton.interactable = value;
    }
}
