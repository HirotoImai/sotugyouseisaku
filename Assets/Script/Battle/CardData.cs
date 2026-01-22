using UnityEngine;
public enum CardElement
{
    Red,
    Green,
    Blue,
    Black,
    White
}
[CreateAssetMenu(fileName = "CardData", menuName = "Game/Card")]

public class CardData : ScriptableObject
{
    public int cardID;
    public string cardName;
    public Sprite image;
    public int cost;
    public int attack;
    public int hp;
    [Header("Visual")]
    public Color mainColor;

    [Header("Element")]
    public CardElement element;
}