using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardButtonView : MonoBehaviour
{
    public Image cardImage;
    public Image colorPanel;   // 色をつけたい UI
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI atkText;
    public TextMeshProUGUI hpText;
    private CardInstance owner;

    public void SetOwner(CardInstance card)
    {
        owner = card;
        // UI 更新など必要に応じて追加
    }
    public void Setup(CardInstance card)
    {
        // カード名・能力
        nameText.text = card.cardName;
        costText.text = card.cost.ToString();
        atkText.text = card.attack.ToString();
        hpText.text = card.hp.ToString();

        // **画像反映**
        if (card.image != null)
            cardImage.sprite = card.image;

        // **色反映**
        colorPanel.color = card.mainColor;

        Debug.Log($"カード:{card.cardName} の色: {card.mainColor}");
    }
}
