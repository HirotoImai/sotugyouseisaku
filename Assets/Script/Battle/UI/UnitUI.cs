using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUI : MonoBehaviour
{
    public Image unitImage;
    public TMP_Text nameText;
    public TMP_Text hpText;
    public TMP_Text attackText;

    public void Setup(CardData card)
    {
        nameText.text = card.cardName;
        unitImage.sprite = card.image;
        attackText.text = card.attack.ToString();
        hpText.text = card.hp.ToString();
    }
}
