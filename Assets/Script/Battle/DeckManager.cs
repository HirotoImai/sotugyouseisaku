using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    void Start()
    {
        // CardInstance[] Å® IEnumerable<CardData>
        var cardDataList = CardSaveManager.loadedCards.Select(ci => ci.template);

        SomeMethodExpectingCardData(cardDataList); // IEnumerable<CardData> ÇìnÇ∑
    }

    void SomeMethodExpectingCardData(System.Collections.Generic.IEnumerable<CardData> cards)
    {
        foreach (var card in cards)
            Debug.Log(card.cardName);
    }
}
