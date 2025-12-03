using System.Collections.Generic;
using UnityEngine;

public class BattleManager_RT : MonoBehaviour
{
    public static BattleManager_RT Instance;

    [Header("デッキ＆手札")]
    public List<CardData> playerDeck = new List<CardData>();
    public List<CardData> cpuDeck = new List<CardData>();
    public List<CardData> playerHand = new List<CardData>();
    public List<CardData> cpuHand = new List<CardData>();

    void Awake()
    {
        Instance = this;

        // デッキ生成（ランダム、重複なし）
        InitializeDecks();
    }

    private void InitializeDecks()
    {
        var pool = new List<CardData>(CardDatabase.Instance.allCards);

        if (pool.Count == 0)
        {
            Debug.LogWarning("カードプールが空です");
            return;
        }

        // プレイヤーデッキ
        playerDeck.Clear();
        var tempPool = new List<CardData>(pool);
        while (tempPool.Count > 0 && playerDeck.Count < 10) // 任意の枚数
        {
            int r = Random.Range(0, tempPool.Count);
            playerDeck.Add(tempPool[r]);
            tempPool.RemoveAt(r); // 同じカードが来ないように削除
        }

        // CPUデッキ
        cpuDeck.Clear();
        tempPool = new List<CardData>(pool);
        while (tempPool.Count > 0 && cpuDeck.Count < 10)
        {
            int r = Random.Range(0, tempPool.Count);
            cpuDeck.Add(tempPool[r]);
            tempPool.RemoveAt(r);
        }

        Debug.Log($"プレイヤーデッキ: {playerDeck.Count}枚, CPUデッキ: {cpuDeck.Count}枚");
    }

    // --------------------------
    // 初期手札
    // --------------------------
    public void DrawStartHand(PlayerManager_RT owner, int count)
    {
        List<CardData> hand = owner.isPlayer ? playerHand : cpuHand;
        List<CardData> deck = owner.isPlayer ? playerDeck : cpuDeck;

        for (int i = 0; i < count; i++)
        {
            if (deck.Count == 0)
                break;

            hand.Add(deck[0]);
            deck.RemoveAt(0);
        }

        if (hand.Count == 0)
            Debug.LogWarning($"{(owner.isPlayer ? "プレイヤー" : "CPU")}の手札が空です");
    }

    // --------------------------
    // 1枚ドロー
    // --------------------------
    public void DrawCard(PlayerManager_RT owner)
    {
        List<CardData> hand = owner.isPlayer ? playerHand : cpuHand;
        List<CardData> deck = owner.isPlayer ? playerDeck : cpuDeck;

        if (deck.Count == 0)
        {
            Debug.Log($"{(owner.isPlayer ? "Player" : "CPU")}のデッキが尽きています");
            return;
        }

        hand.Add(deck[0]);
        deck.RemoveAt(0);
        Debug.Log($"{(owner.isPlayer ? "Player" : "CPU")} が {hand[hand.Count - 1].cardName} をドロー！");
    }

    // --------------------------
    // 出撃
    // --------------------------
    public void PlayCard(PlayerManager_RT owner, CardData card)
    {
        List<CardData> hand = owner.isPlayer ? playerHand : cpuHand;

        if (!hand.Contains(card))
        {
            Debug.LogWarning("手札に存在しないカードを出撃しようとしています。");
            return;
        }

        hand.Remove(card);
        Debug.Log($"{(owner.isPlayer ? "Player" : "CPU")} が {card.cardName} を出撃！");

        // 出撃後に1枚ドロー
        DrawCard(owner);
    }
}
