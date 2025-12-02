using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardButtonView : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private Text cardNameText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    private CardInstance owner;

    public void SetOwner(CardInstance card)
    {
        owner = card;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (owner == null) return;

        cardNameText.text = owner.cardName;
        cardImage.sprite = owner.image;
        cardImage.color = owner.mainColor;
    }
}
