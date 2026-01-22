using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleDeck
{
    public List<int> drawPile = new List<int>();
    public int loopCount = 0;

    private List<int> original;

    public BattleDeck(DeckData source)
    {
        original = new List<int>(source.cardIDs);
        ResetDeck();
    }

    public void ResetDeck()
    {
        drawPile = new List<int>(original);
        drawPile = drawPile.OrderBy(_ => Random.value).ToList();
    }

    public int Draw()
    {
        if (drawPile.Count == 0)
        {
            loopCount++;
            ResetDeck();
        }

        int id = drawPile[0];
        drawPile.RemoveAt(0);
        return id;
    }

    public int GetModifiedCost(CardData card)
    {
        return card.cost + loopCount;
    }
}

