using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public CardInstance[] loadedCards;

    public CardData[] GetCardDataList()
    {
        if (loadedCards == null || loadedCards.Length == 0) return new CardData[0];

        // CardInstance ‚©‚ç CardData ‚ðŽæ‚èo‚·
        return loadedCards.Select(ci => ci.template).ToArray();
    }
}