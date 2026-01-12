using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Game/Card")]

public enum CardElement
{
    Red,
    Green,
    Blue,
    Black,
    White
}
public class CardData : ScriptableObject
{
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