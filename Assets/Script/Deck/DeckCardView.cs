using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DeckCardView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text cardNameText;

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

        if (cardNameText != null)
            cardNameText.text = data.cardName;
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
