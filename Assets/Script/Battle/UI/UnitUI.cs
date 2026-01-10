using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUI : MonoBehaviour
{
    public Image unitImage;
    public TMP_Text nameText;
    public TMP_Text hpText;
    public TMP_Text attackText;

    [SerializeField] private Image imageColor;

    private Unit_RT unit;   // ★ 追加

    public void Setup(CardData card, PlayerManager_RT owner)
    {
        unit = GetComponent<Unit_RT>();   // ★ 取得
        unit.Setup(card, owner);          // ★ Unit_RT を初期化

        nameText.text = card.cardName;
        unitImage.sprite = card.image;
        attackText.text = card.attack.ToString();
        hpText.text = card.hp.ToString();
        imageColor.color = card.mainColor;
    }

    // ★ HP表示更新用
    public void UpdateHP(int currentHP)
    {
        hpText.text = currentHP.ToString();
    }
}
