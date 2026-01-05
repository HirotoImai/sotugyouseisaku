using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager_RT : MonoBehaviour
{
    public static BattleManager_RT Instance;

    [Header("プレイヤー/CPU管理")]
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

    // 各陣営の手札
    public List<CardData> playerHand = new List<CardData>();
    public List<CardData> cpuHand = new List<CardData>();

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
    private void DrawCard(PlayerManager_RT owner,List<CardInstance> handList,List<CardData> deck)
    {
        if (deck.Count == 0) return;

        CardData baseData = deck[0];
        deck.RemoveAt(0);

        // ★ ここがポイント
        handList.Add(new CardInstance(baseData));

        Debug.Log($"{(owner.isPlayer ? "Player" : "CPU")} が {baseData.cardName} をドロー");
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
        //Debug.Log($"{(owner.isPlayer ? "Player" : "CPU")} が {card.cardName} を出撃");
        SpawnUnit(owner, card);
        // 出撃後 自動ドロー
        DrawCard(owner, handList, owner.deck);

        // UI更新
        owner.UpdateHandUI(handList);
    }
    public void SpawnUnit(PlayerManager_RT owner, CardData card)
    {

        Transform parent = owner.isPlayer ? playerField : cpuField;

        GameObject unit = Instantiate(unitPrefab, parent);
        Debug.Log($"生成されたUnit: {unit.name}, 親: {parent.name}");


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
            RectTransform rt = field.GetChild(i).GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(
                (i - (count - 1) / 2f) * spacing,
                0
            );
        }
    }
    private void CPUAction()
    {
        if (cpuHand.Count == 0)
            return;

        // 出せるカードだけ抽出
        var playableCards = cpuHand
            .Where(card => cpu.CanPlayCard(card))
            .ToList();

        if (playableCards.Count == 0)
            return;

        // 今回はランダムに1枚
        CardData selected = playableCards[Random.Range(0, playableCards.Count)];

        cpu.TryPlayCard(selected);

        Debug.Log($"[CPU] hand={cpuHand.Count}, mana={cpu.currentMana}");

    }

}

