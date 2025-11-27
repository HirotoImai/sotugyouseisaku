using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public Image cardImage;
    public Text nameText;
    public Text statText;

    public void Setup(CardData cardData, bool isPlayer)
    {
        if (cardImage != null) cardImage.sprite = cardData.image;
        if (nameText != null) nameText.text = cardData.cardName;
        if (statText != null) statText.text = $"ATK:{cardData.attack} DEF:{cardData.hp}";
    }
}