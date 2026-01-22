using UnityEngine;
using System.Collections.Generic;

public class CardDatabase : MonoBehaviour
{
    public static CardDatabase Instance;

    [Header("カードデータ")]
    public List<CardData> fixedCards = new();
    public List<CardData> allCards = new();

    private Dictionary<int, CardData> cardDict;

    private void Awake()
    {
        // Singleton
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Initialize();
    }
    void Start()
    {
        CardData[] loaded = Resources.LoadAll<CardData>("Cards");
        fixedCards.AddRange(loaded);

        allCards.Clear();
        allCards.AddRange(fixedCards);
        allCards.AddRange(CardPersistenceService.LoadedCards);

        BuildDictionary();
    }
    private void Initialize()
    {
        fixedCards.Clear();
        allCards.Clear();

        // 固定カード（Resources）
        CardData[] loaded = Resources.LoadAll<CardData>("Cards");
        fixedCards.AddRange(loaded);

        // 固定 + 作成カード
        allCards.AddRange(fixedCards);
        allCards.AddRange(CardSaveManager.loadedCards);

        BuildDictionary();

        Debug.Log(
            $"カードデータ合計: {allCards.Count} 枚 " +
            $"（固定:{fixedCards.Count}, 作成:{CardSaveManager.loadedCards.Count}）"
        );
    }

    private void BuildDictionary()
    {
        cardDict = new Dictionary<int, CardData>();
        foreach (var card in allCards)
        {
            if (card == null) continue;
            cardDict[card.cardID] = card;
        }
    }

    public CardData GetCardByID(int cardID)
    {
        if (cardDict != null && cardDict.TryGetValue(cardID, out var card))
            return card;

        Debug.LogWarning($"CardID {cardID} が存在しません");
        return null;
    }

    public CardData GetRandomCard()
    {
        if (allCards.Count == 0) return null;
        return allCards[Random.Range(0, allCards.Count)];
    }
}
