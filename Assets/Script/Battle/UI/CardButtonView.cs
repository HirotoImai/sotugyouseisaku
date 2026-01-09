using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardButtonView : MonoBehaviour
{
    [Header("カード表示")]
    public Image backgroundImage;
    public Image cardImage;
    public TMP_Text cardNameText;
    public TMP_Text costText;
    public TMP_Text atkText;
    public TMP_Text hpText;

    public CardInstance cardInstance; // private のまま保持
    private PlayerManager_RT owner;

    public void Setup(CardInstance card)
    {
        cardInstance = card;

        if (cardImage != null)
        {
            cardImage.sprite = card.data.image;
            cardImage.color = card.data.mainColor;
        }

        if (cardNameText != null)
            cardNameText.text = card.data.cardName;

        if (costText != null)
            costText.text = $"Cost:{card.data.cost + 1}";

        if (atkText != null)
            atkText.text = $"Atk:{card.data.attack}";

        if (hpText != null)
            hpText.text = $"HP:{card.data.hp}";
    }

    public void SetOwner(PlayerManager_RT player)
    {
        owner = player;
    }

    //public CardData GetCardData()
    //{
    //    return card.data;
    //}

    public void OnClick()
    {
        if (owner == null || cardInstance == null)
        {
            return;
        }
            owner.TryPlayCard(cardInstance);
        Debug.Log("true");
    }

    public void Highlight(bool enable)
    {
        GetComponent<Image>().color = enable ? Color.yellow : Color.white;
    }

    public CardInstance GetCardInstance() { return cardInstance; }
}
