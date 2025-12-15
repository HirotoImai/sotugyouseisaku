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

    public CardData data; // private のまま保持
    private PlayerManager_RT owner;

    public void Setup(CardData cardData)
    {
        data = cardData;

        if (cardImage != null)
        {
            cardImage.sprite = data.image;
            cardImage.color = data.mainColor;
        }

        if (cardNameText != null)
            cardNameText.text = data.cardName;

        if (costText != null)
            costText.text = $"Cost:{data.cost + 1}";

        if (atkText != null)
            atkText.text = $"Atk:{data.attack}";

        if (hpText != null)
            hpText.text = $"HP:{data.hp}";
    }

    public void SetOwner(PlayerManager_RT player)
    {
        owner = player;
    }

    public CardData GetCardData()
    {
        return data;
    }

    public void OnClick()
    {
        if (data == null)
        {
            Debug.LogWarning("CardButtonView: data がセットされていません");
            return;
        }

        if (owner != null)
            owner.DeployCard(data);
        else
            Debug.Log($"カードクリック: {data.cardName}（owner未設定）");
    }

    public void Highlight(bool enable)
    {
        GetComponent<Image>().color = enable ? Color.yellow : Color.white;
    }
}
