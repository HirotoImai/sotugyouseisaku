using System.Collections.Generic;
using UnityEngine;
public static class DeckSaveKeys
{
    public const string PlayerDeck = "PlayerDeck";
    public const string CPUDeck = "CPUDeck";
}
[CreateAssetMenu(menuName = "Card/DeckData")]
public class DeckData : ScriptableObject
{
    public const int MaxDeckSize = 20;

    public List<int> cardIDs = new List<int>();

    // ----------------------
    // 初期化
    // ----------------------
    public void InitializeAsEmpty()
    {
        if (cardIDs == null)
            cardIDs = new List<int>();
        else
            cardIDs.Clear();
    }

    public void InitializeFromDatabase(CardDatabase db)
    {
        InitializeAsEmpty();

        if (db == null || db.allCards == null || db.allCards.Count == 0)
        {
            Debug.LogError("CardDatabase が未初期化です");
            return;
        }

        // 中身を詰めない方針：先頭から MaxDeckSize
        for (int i = 0; i < MaxDeckSize && i < db.allCards.Count; i++)
        {
            cardIDs.Add(db.allCards[i].cardID);
        }
    }

    // ----------------------
    // デッキ操作
    // ----------------------
    public bool CanAddCard(CardData card)
    {
        if (card == null) return false;
        if (cardIDs.Count >= MaxDeckSize) return false;
        return !cardIDs.Contains(card.cardID);
    }

    public bool AddCard(CardData card)
    {
        if (!CanAddCard(card))
            return false;

        cardIDs.Add(card.cardID);
        return true;
    }

    public void RemoveCard(int cardID)
    {
        cardIDs.Remove(cardID);
    }

    public bool IsSavable(int min)
    {
        return cardIDs.Count >= min;
    }
}

[System.Serializable]
public class DeckSaveData
{
    public List<int> cardIDs = new();
}
