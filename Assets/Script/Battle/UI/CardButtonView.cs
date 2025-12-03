using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardButtonView : MonoBehaviour
{
    public Image backgroundImage;
    public Image cardImage;
    public TMP_Text cardNameText;
    public TMP_Text costText; // Cost, Atk, HPなどを表示する用
    public TMP_Text atkText;
    public TMP_Text hpText;

    private CardData data; // このカードの情報を保持
    private PlayerManager_RT owner; // クリック時に通知する相手

    public void Setup(CardData data)
    {
        this.data = data;

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

        // ここで色を確認
        Debug.Log($"カード: {data.cardName} の色: R={data.mainColor.r}, G={data.mainColor.g}, B={data.mainColor.b}, A={data.mainColor.a}");
    }

    // PlayerManager を外部からセットするためのメソッド
    public void SetOwner(PlayerManager_RT owner)
    {
        this.owner = owner;
    }

    // ボタンの OnClick に割り当て
    public void OnClick()
    {
        if (data == null)
        {
            Debug.LogWarning("CardButtonView: data がセットされていません");
            return;
        }

        if (owner != null)
        {
            owner.DeployCard(data);
        }
        else
        {
            Debug.Log($"カードをクリック: {data.cardName}（owner未設定）");
        }
    }
    public void Highlight(bool enable)
    {
        GetComponent<Image>().color = enable ? Color.yellow : Color.white;
    }
}
