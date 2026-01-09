using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager_RT : MonoBehaviour
{
    public static BattleManager_RT Instance;

    [Header("プレイヤー / CPU")]
    public PlayerManager_RT player;
    public PlayerManager_RT cpu;

    [Header("CPU設定")]
    public float cpuThinkInterval = 2f;

    [Header("フィールド")]
    public Transform playerField;
    public Transform cpuField;
    public GameObject unitPrefab;

    [Header("デッキ設定")]
    public int startHandSize = 3;

    // ★ 手札は CardInstance で管理
    public List<CardInstance> playerHand = new();
    public List<CardInstance> cpuHand = new();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeDecks();

        if (cpu != null)
            InvokeRepeating(nameof(CPUAction), 2f, cpuThinkInterval);
    }

    // =============================
    // デッキ初期化
    // =============================
    public void InitializeDecks()
    {
        var allCards = CardDatabase.Instance.allCards;
        if (allCards == null || allCards.Count == 0)
        {
            Debug.LogError("CardDatabase にカードがありません");
            return;
        }

        // Player
        if (player != null)
        {
            player.deck = allCards.OrderBy(_ => Random.value).ToList();
            playerHand.Clear();

            for (int i = 0; i < startHandSize; i++)
                DrawCard(player, playerHand, player.deck);

            player.UpdateHandUI(playerHand);
        }

        // CPU
        if (cpu != null)
        {
            cpu.deck = allCards.OrderBy(_ => Random.value).ToList();
            cpuHand.Clear();

            for (int i = 0; i < startHandSize; i++)
                DrawCard(cpu, cpuHand, cpu.deck);

            cpu.UpdateHandUI(cpuHand);
        }
    }

    // =============================
    // ドロー
    // =============================
    private void DrawCard(
        PlayerManager_RT owner,
        List<CardInstance> hand,
        List<CardData> deck
    )
    {
        if (deck.Count == 0) return;

        var data = deck[0];
        deck.RemoveAt(0);

        hand.Add(new CardInstance(data));

        Debug.Log($"{(owner.isPlayer ? "Player" : "CPU")} が {data.cardName} をドロー");
    }

    // =============================
    // カード使用
    // =============================
    public void PlayCard(PlayerManager_RT owner, CardInstance card)
    {
        var hand = owner.isPlayer ? playerHand : cpuHand;

        if (!hand.Contains(card))
        {
            Debug.LogWarning(
                $"[PlayCard] handに存在しない\n" +
                $"card={card}\n" +
                $"handCount={hand.Count}\n" +
                $"handRefs={string.Join(",", hand.Select(c => c.GetHashCode()))}\n" +
                $"targetRef={card.GetHashCode()}"
            );
            return;
        }

        hand.Remove(card);

        SpawnUnit(owner, card.data);

        DrawCard(owner, hand, owner.deck);
        owner.UpdateHandUI(hand);
    }

    // =============================
    // ユニット生成
    // =============================
    private void SpawnUnit(PlayerManager_RT owner, CardData card)
    {
        Transform parent = owner.isPlayer ? playerField : cpuField;

        GameObject unit = Instantiate(unitPrefab, parent);

        UnitUI ui = unit.GetComponent<UnitUI>();
        ui.Setup(card, owner);

        ArrangeUnits(parent);
    }

    private void ArrangeUnits(Transform field)
    {
        float spacing = 120f;
        int count = field.childCount;

        for (int i = 0; i < count; i++)
        {
            var rt = field.GetChild(i).GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(
                (i - (count - 1) / 2f) * spacing,
                0
            );
        }
    }

    // =============================
    // CPU 行動
    // =============================
    private void CPUAction()
    {
        Debug.Log($"CPUAction start handCount={cpuHand.Count}");

        foreach (var card in cpuHand)
        {
            Debug.Log($"check card ref={card.GetHashCode()}");

            if (cpu.CanPlayCard(card))
            {
                Debug.Log($"CPU selected card ref={card.GetHashCode()}");
                cpu.TryPlayCard(card);
                return;
            }
        }
    }

}
