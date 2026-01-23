using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DeckCardView : MonoBehaviour
{
    [Header("UI")]
    public Image backgroundImage;
    public Image cardImage;
    public TMP_Text cardNameText;
    public TMP_Text costText;
    public TMP_Text atkText;
    public TMP_Text hpText;
    private CardData cardData;
    private Action<CardData> onClick;

    /// <summary>
    /// 表示とクリック処理を設定
    /// </summary>
    public void Setup(CardData data, Action<CardData> onClickAction)
    {
        cardData = data;
        onClick = onClickAction;

        if (cardImage != null)
            cardImage.sprite = data.image;
        if(backgroundImage != null)
            backgroundImage.color = data.mainColor;
        if (cardNameText != null)
            cardNameText.text = data.cardName;
        if (costText != null)
            costText.text = $"{data.cost + 1}";

        if (atkText != null)
            atkText.text = $"{data.attack}";

        if (hpText != null)
            hpText.text = $"{data.hp}";
    }

    /// <summary>
    /// Button から呼ばれる
    /// </summary>
    public void OnClick()
    {
        if (cardData == null)
            return;

        onClick?.Invoke(cardData);
    }
}
