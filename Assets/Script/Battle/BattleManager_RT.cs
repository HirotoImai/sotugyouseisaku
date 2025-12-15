using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager_RT : MonoBehaviour
{
    public static BattleManager_RT Instance;

    [Header("プレイヤー/CPU管理")]
    public PlayerManager_RT player;
    public PlayerManager_RT cpu;

    [Header("デッキ設定")]
    public int startHandSize = 3;

    // 各陣営の手札
    public List<CardData> playerHand = new List<CardData>();
    public List<CardData> cpuHand = new List<CardData>();
    public GameObject unitPrefab;   // 生成するユニットPrefab
    public Transform fieldArea;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeDecks();
    }

    // =============================
    // デッキ初期化
    // =============================
    public void InitializeDecks()
    {
        List<CardData> allCards = CardDatabase.Instance.allCards;
        if (allCards == null || allCards.Count == 0)
        {
            Debug.LogError("CardDatabase にカードがありません");
            return;
        }

        // --- Player ---
        if (player != null)
        {
            playerHand.Clear();

            // デッキを保存（重要）
            player.deck = allCards.OrderBy(x => Random.value).ToList();

            // 最初の手札
            for (int i = 0; i < startHandSize; i++)
                DrawCard(player, playerHand, player.deck);

            player.UpdateHandUI(playerHand);
        }

        // --- CPU ---
        if (cpu != null)
        {
            cpuHand.Clear();

            cpu.deck = allCards.OrderBy(x => Random.value).ToList();

            for (int i = 0; i < startHandSize; i++)
                DrawCard(cpu, cpuHand, cpu.deck);

            cpu.UpdateHandUI(cpuHand);
        }
    }

    // =============================
    // ドロー処理
    // =============================
    private void DrawCard(PlayerManager_RT owner, List<CardData> handList, List<CardData> deck)
    {
        if (deck.Count == 0)
        {
            Debug.Log($"{(owner.isPlayer ? "Player" : "CPU")} デッキが尽きています");
            return;
        }

        CardData drawn = deck[0];
        deck.RemoveAt(0);
        handList.Add(drawn);

        Debug.Log($"{(owner.isPlayer ? "Player" : "CPU")} が {drawn.cardName} をドロー");
    }

    // =============================
    // カード出撃
    // =============================
    public void PlayCard(PlayerManager_RT owner, CardData card)
    {
        List<CardData> handList = owner.isPlayer ? playerHand : cpuHand;

        if (!handList.Contains(card))
        {
            Debug.LogWarning("手札に存在しないカードが使われました");
            return;
        }

        // プレイヤーのみマナチェック
        if (owner.isPlayer && !owner.CanPlayCard(card))
        {
            Debug.LogWarning("Mana不足でカードを出せません");
            return;
        }

        // マナ使用
        if (owner.isPlayer)
            owner.UseMana(card.cost);

        // 手札から削除
        handList.Remove(card);
        Debug.Log($"{(owner.isPlayer ? "Player" : "CPU")} が {card.cardName} を出撃");

        // 出撃後 自動ドロー
        DrawCard(owner, handList, owner.deck);

        // UI更新
        owner.UpdateHandUI(handList);
    }
    private void SpawnUnit(CardData card, PlayerManager_RT owner)
    {
        // unitPrefab は CardData に応じて変えられるようにしても良い
        GameObject unit = Instantiate(unitPrefab, fieldArea);

        // CardData をセットしたり、ステータス反映したり
        // UnitScript unitScript = unit.GetComponent<UnitScript>();
        // unitScript.Setup(card, owner);

        Debug.Log("Unit Spawned: " + card.cardName);
    }
}

