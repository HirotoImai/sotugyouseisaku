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

    [Header("戦闘ユニット管理")]
    public List<Unit_RT> playerUnits = new();
    public List<Unit_RT> cpuUnits = new();
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
            player.hand.Clear();

            for (int i = 0; i < startHandSize; i++)
                DrawCard(player);

            player.UpdateHandUI(player.hand);
        }

        // CPU
        if (cpu != null)
        {
            cpu.deck = allCards.OrderBy(_ => Random.value).ToList();
            cpu.hand.Clear();

            for (int i = 0; i < startHandSize; i++)
                DrawCard(cpu);

            cpu.UpdateHandUI(cpu.hand);
        }
    }

    // =============================
    // ドロー
    // =============================
    private void DrawCard(PlayerManager_RT owner)
    {
        if (owner.deck.Count == 0) return;

        var data = owner.deck[0];
        owner.deck.RemoveAt(0);

        owner.hand.Add(new CardInstance(data));

        owner.UpdateHandUI(owner.hand);
    }

    // =============================
    // カード使用
    // =============================
    public void PlayCard(PlayerManager_RT owner, CardInstance card)
    {
        var hand = owner.hand;

        if (!hand.Contains(card))
        {
            Debug.LogWarning(
                $"[PlayCard] handに存在しない"
            );
            return;
        }

        owner.hand.Remove(card);

        SpawnUnit(owner, card);

        DrawCard(owner);
        owner.UpdateHandUI(hand);
    }

    // =============================
    // ユニット生成
    // =============================
    private void SpawnUnit(PlayerManager_RT owner, CardInstance card)
    {
        Transform parent = owner.isPlayer ? playerField : cpuField;

        GameObject unit = Instantiate(unitPrefab, parent);

        UnitUI ui = unit.GetComponent<UnitUI>();
        ui.Setup(card.data, owner);
        Unit_RT rt = unit.GetComponent<Unit_RT>();
        if (owner.isPlayer)
            playerUnits.Add(rt);
        else
            cpuUnits.Add(rt);
        ArrangeUnits(parent);
        Debug.Log($"SpawnUnit owner={owner.name} isPlayer={owner.isPlayer} parent={parent.name}");

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
    public void RemoveUnit(Unit_RT unit)
    {
        playerUnits.Remove(unit);
        cpuUnits.Remove(unit);
    }
    // =============================
    // CPU 行動
    // =============================
    private void CPUAction()
    {
        var hand = cpu.hand; // ← ここが超重要

        Debug.Log($"CPUAction start handCount={hand.Count}");

        foreach (var card in hand)
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
