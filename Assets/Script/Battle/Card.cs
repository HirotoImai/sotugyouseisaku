//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;  // ’Ç‰Á

//public class Card : MonoBehaviour
//{
//    public TMP_Text nameText;
//    public TMP_Text costText;
//    public TMP_Text powerText;
//    public TMP_Text healthText;
//    public TMP_Text effectText;
//    public Image artworkImage;

//    private CardData data;

//    public void Init(CardData cardData)
//    {
//        data = cardData;

//        nameText.text = data.cardName;
//        costText.text = "Cost: " + data.cost;

//        if (data.kind == CardKind.Creature)
//        {
//            powerText.text = "Power: " + data.power;
//            healthText.text = "Health: " + data.health;
//        }
//        else
//        {
//            powerText.text = "";
//            healthText.text = "";
//        }

//        effectText.text = data.effectText;
//        artworkImage.sprite = data.artwork;
//    }

//    public CardData GetData() => data;
//}