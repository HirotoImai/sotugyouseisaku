using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardDataListWrapper
{
    public List<CardDataSerializable> cards;

    public CardDataListWrapper(List<CardDataSerializable> cards)
    {
        this.cards = cards;
    }
}

[System.Serializable]
public class CardDataSerializable
{
    public int cardID;
    public string cardName;
    public int cost;
    public int attack;
    public int hp;
    public string color;
    public string imagePath;
    public CardElement element;
}
