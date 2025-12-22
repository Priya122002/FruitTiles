using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image frontImage;
    [SerializeField] private Image backImage;

    private int cardId;
    private bool isFaceUp;

    public int CardId => cardId;
    public bool IsFaceUp => isFaceUp;

    public void Initialize(int id)
    {
        cardId = id;
        button.onClick.AddListener(Flip);
        ShowBack();
    }

    private void Flip()
    {
        if (isFaceUp) return;

        isFaceUp = true;
        ShowFront();

        MatchSystem.Instance.RegisterFlip(this);
    }

    public void ShowFront()
    {
        frontImage.enabled = true;
        backImage.enabled = false;
    }

    public void ShowBack()
    {
        frontImage.enabled = false;
        backImage.enabled = true;
        isFaceUp = false;
    }

    public void Disable()
    {
        button.interactable = false;
    }
}
