using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager_RT : MonoBehaviour
{
    public static BattleManager_RT Instance;

    [Header("プレイヤー / CPU")]
    public PlayerManager_RT player;
    public PlayerManager_RT cpu;

    [Header("CPU設定")]
    public float cpuThinkInterval = 2f;
    [Range(0f, 1f)]
    public float cpuPlayChance = 0.8f;
    public float cpuMinManaRate = 0.3f;

    [Header("フィールド")]
    public Transform playerField;
    public Transform cpuField;
    public GameObject unitPrefab;

    [Header("デッキ設定")]
    public int startHandSize = 5; // ★ 最初の手札5枚

    [Header("バトル状態")]
    public bool isBattleFinished = false;

    [Header("UI")]
    [SerializeField] private ResultUI resultUI;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CardDatabase.Instance.Rebuild();
        player.Initialize(GameManager.Instance.CurrentDeck);
        cpu.Initialize(GameManager.Instance.CurrentDeck);

        InvokeRepeating(nameof(CPUAction), 2f, cpuThinkInterval);
    }

    void Update()
    {
        if (isBattleFinished) return;
        HandleUnitAttacks();
    }

    // =============================
    // カード使用
    // =============================
    public void PlayCard(PlayerManager_RT owner, CardInstance card)
    {
        if (isBattleFinished) return;
        SpawnUnit(owner, card);
    }

    // =============================
    // ユニット生成
    // =============================
    private void SpawnUnit(PlayerManager_RT owner, CardInstance card)
    {
        Transform parent = owner.isPlayer ? playerField : cpuField;

        GameObject unitObj = Instantiate(unitPrefab, parent);

        Unit_RT unit = unitObj.GetComponent<Unit_RT>();
        unit.Setup(card.data, owner);

        UnitUI ui = unitObj.GetComponent<UnitUI>();
        ui.Setup(card.data, owner);

        ArrangeUnits(parent);
    }

    private void ArrangeUnits(Transform field)
    {
        float spacing = 120f;
        int count = field.childCount;

        for (int i = 0; i < count; i++)
        {
            RectTransform rt = field.GetChild(i).GetComponent<RectTransform>();
            rt.anchoredPosition =
                new Vector2((i - (count - 1) / 2f) * spacing, 0);
        }
    }

    // =============================
    // CPU 行動
    // =============================
    private void CPUAction()
    {
        if (isBattleFinished) return;

        if (UnityEngine.Random.value > cpuPlayChance)
            return;

        if (cpu.currentMana / cpu.maxMana < cpuMinManaRate)
            return;

        foreach (var card in cpu.hand)
        {
            if (cpu.CanPlayCard(card))
            {
                cpu.TryPlayCard(card);
                return;
            }
        }
    }

    // =============================
    // 前列判定（★ ここが重要）
    // =============================
    public bool IsFrontUnit(Unit_RT unit)
    {
        Transform field =
            unit.faction == Faction.Player ? playerField : cpuField;

        if (field.childCount == 0)
            return false;

        return field.GetChild(0) == unit.transform;
    }

    public Unit_RT GetFrontUnit(Faction faction)
    {
        Transform field =
            faction == Faction.Player ? playerField : cpuField;

        if (field.childCount == 0)
            return null;

        return field.GetChild(0).GetComponent<Unit_RT>();
    }

    // =============================
    // 攻撃処理
    // =============================
    private void HandleUnitAttacks()
    {
        Unit_RT playerFront = GetFrontUnit(Faction.Player);
        Unit_RT cpuFront = GetFrontUnit(Faction.CPU);

        if (playerFront != null && playerFront.CanAttack())
        {
            playerFront.ForceFillGauge();
            if (cpuFront != null)
                playerFront.TryAttack(cpuFront, cpu);
            else
                playerFront.TryAttackBase(cpu);
        }

        if (cpuFront != null && cpuFront.CanAttack())
        {
            cpuFront.ForceFillGauge();
            if (playerFront != null)
                cpuFront.TryAttack(playerFront, player);
            else
                cpuFront.TryAttackBase(player);
        }
    }

    // =============================
    // 勝敗判定
    // =============================
    public void CheckBattleResult()
    {
        if (isBattleFinished) return;

        if (player.currentHP <= 0)
        {
            EndBattle(false);
        }
        else if (cpu.currentHP <= 0)
        {
            EndBattle(true);
        }
    }

    private void EndBattle(bool playerWin)
    {
        isBattleFinished = true;

        if (resultUI != null)
            resultUI.ShowResult(playerWin);

        CancelInvoke();
        Time.timeScale = 0f;
    }
}
