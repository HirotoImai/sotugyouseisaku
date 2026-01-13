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

    [Header("バトル状態")]
    public bool isBattleFinished = false;

    [Header("UI")]
    [SerializeField] private ResultUI resultUI;

    public List<Unit_RT> GetAllUnits()
    {
        return playerField.GetComponentsInChildren<Unit_RT>()
            .Concat(cpuField.GetComponentsInChildren<Unit_RT>())
            .ToList();
    }
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
    void Update()
    {
        if (isBattleFinished) return;

        HandleUnitAttacks();
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
    public Unit_RT GetFrontEnemyUnit(Unit_RT attacker)
    {
        Transform enemyField =
            attacker.faction == Faction.Player ? cpuField : playerField;

        if (enemyField.childCount == 0)
            return null;

        return enemyField.GetChild(0).GetComponent<Unit_RT>();
    }
    public List<Unit_RT> GetPlayerUnits()
    {
        return playerField.GetComponentsInChildren<Unit_RT>().ToList();
    }

    public List<Unit_RT> GetCpuUnits()
    {
        return cpuField.GetComponentsInChildren<Unit_RT>().ToList();
    }

    void HandleUnitAttacks()
    {
        foreach (var unit in GetAllUnits())
        {
            if (unit == null) continue;
            if (!unit.CanAttack()) continue;

            Transform enemyField =
                unit.faction == Faction.Player ? cpuField : playerField;

            if (enemyField.childCount > 0)
            {
                Unit_RT targetUnit = enemyField.GetChild(0).GetComponent<Unit_RT>();
                PlayerManager_RT targetPlayer = GetEnemyPlayer(unit);
                unit.TryAttack(targetUnit,targetPlayer);
            }
            else
            {
                PlayerManager_RT targetPlayer = GetEnemyPlayer(unit);
                unit.TryAttackBase(targetPlayer);
            }


            unit.ResetAttackTimer();
        }
    }



    public PlayerManager_RT GetEnemyPlayer(Unit_RT attacker)
    {
        return attacker.faction == Faction.Player ? cpu : player;
    }
    public void CheckBattleResult()
    {
        if (isBattleFinished) return;

        if (player.currentHP <= 0)
        {
            isBattleFinished = true;
            Debug.Log("CPU 勝利");
            OnBattleEnd(false);
        }
        else if (cpu.currentHP <= 0)
        {
            isBattleFinished = true;
            Debug.Log("Player 勝利");
            OnBattleEnd(true);
        }
    }
    private void OnBattleEnd(bool playerWin)
    {
        Debug.Log(playerWin ? "=== PLAYER WIN ===" : "=== CPU WIN ===");

        isBattleFinished = true;

        // 勝敗UI表示
        if (resultUI != null)
        {
            resultUI.ShowResult(playerWin);
        }

        // バトル停止
        CancelInvoke();
        Time.timeScale = 0f;
    }


}
