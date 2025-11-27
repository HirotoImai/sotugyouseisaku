using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDeck", menuName = "CardGame/DeckData")]
public class DeckData : ScriptableObject
{
    public List<CardData> cards = new List<CardData>();
}