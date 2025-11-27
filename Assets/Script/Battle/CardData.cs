using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Game/Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite image;
    public int cost;
    public int attack;
    public int hp;
    public Color mainColor;
}