using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUI : MonoBehaviour
{
    public Image unitImage;
    public TMP_Text nameText;
    public TMP_Text hpText;
    public TMP_Text attackText;
    private PlayerManager_RT owner;
    public void Setup(CardData card, PlayerManager_RT owner)
    {
        this.owner = owner;
        nameText.text = card.cardName;
        unitImage.sprite = card.image;
        attackText.text = card.attack.ToString();
        hpText.text = card.hp.ToString();
    }
}
