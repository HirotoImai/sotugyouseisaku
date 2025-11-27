using System.Collections.Generic;

public class Player
{
    public int life = 20;
    public int currentCost = 0;
    public int maxCost = 0;

    public List<CardData> deck = new List<CardData>();
    public List<CardData> hand = new List<CardData>();
    public List<CardData> field = new List<CardData>();
}