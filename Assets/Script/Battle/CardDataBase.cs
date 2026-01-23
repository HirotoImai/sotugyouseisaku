using UnityEngine;
using System.Collections.Generic;

public class CardDatabase : MonoBehaviour
{
    public static CardDatabase Instance;

    [Header("カード一覧")]
    public List<CardData> fixedCards = new();
    public List<CardData> allCards = new();

    private Dictionary<int, CardData> cardDict = new();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        fixedCards.Clear();
        allCards.Clear();
        cardDict.Clear();

        // 固定カード
        CardData[] loaded = Resources.LoadAll<CardData>("Cards");
        fixedCards.AddRange(loaded);

        // 作成カード（Persistence が正）
        allCards.AddRange(fixedCards);
        allCards.AddRange(CardPersistenceService.Cards);

        foreach (var card in allCards)
        {
            if (card == null) continue;
            cardDict[card.cardID] = card;
        }

        Debug.Log(
            $"[CardDatabase] 合計:{allCards.Count} " +
            $"(固定:{fixedCards.Count}, 作成:{CardPersistenceService.Cards.Count})"
        );
    }

    public void Rebuild()
    {
        fixedCards.Clear();
        allCards.Clear();
        cardDict.Clear();

        // 固定カード
        CardData[] fixedLoaded = Resources.LoadAll<CardData>("Cards");
        fixedCards.AddRange(fixedLoaded);

        // 固定 + 作成
        allCards.AddRange(fixedCards);
        allCards.AddRange(CardPersistenceService.Cards);

        foreach (var card in allCards)
        {
            if (card == null) continue;
            cardDict[card.cardID] = card;
        }

        Debug.Log(
            $"[CardDatabase] 再構築完了 合計:{allCards.Count} " +
            $"(固定:{fixedCards.Count}, 作成:{CardPersistenceService.Cards.Count})"
        );
    }

    public CardData GetCardByID(int id)
    {
        return cardDict.TryGetValue(id, out var card) ? card : null;
    }

    public List<CardData> GetAllCards()
    {
        return allCards;
    }
}
