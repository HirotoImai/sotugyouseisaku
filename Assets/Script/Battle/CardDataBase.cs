using UnityEngine;
using System.Collections.Generic;

public class CardDatabase : MonoBehaviour
{
    public static CardDatabase Instance;

    public List<CardData> fixedCards = new List<CardData>(); // Resources の固定カード
    public List<CardData> allCards = new List<CardData>();   // 固定 + 作成カード

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Resources から固定カードをロード
        CardData[] loaded = Resources.LoadAll<CardData>("Cards");
        fixedCards.AddRange(loaded);

        // allCards に固定カードを追加
        allCards.Clear();
        allCards.AddRange(fixedCards);

        // JSON で保存されたカードも追加
        allCards.AddRange(CardSaveManager.loadedCards);

        // デバッグ用
        Debug.Log($"カードデータ合計: {allCards.Count} 枚（固定:{fixedCards.Count}, 作成:{CardSaveManager.loadedCards.Count}）");
        //foreach (var c in allCards)
        //{
        //    Debug.Log($"カード名: {c.cardName}");
        //}
    }

    public CardData GetRandomCard()
    {
        if (allCards.Count == 0) return null;
        return allCards[Random.Range(0, allCards.Count)];
    }
}