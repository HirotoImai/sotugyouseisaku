using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction
{
    Player,
    CPU
}

public class Unit_RT : MonoBehaviour
{
    [Header("陣営")]
    public Faction faction;

    [Header("ステータス")]
    public int maxHP;
    public int currentHP;
    public int attack;

    [Header("参照")]
    public PlayerManager_RT owner;

    [Header("攻撃設定")]
    [SerializeField] private float attackInterval = 1.5f;

    private Coroutine attackCoroutine;
    private UnitUI ui;
    // =============================
    // 初期化（出撃時に必ず呼ばれる）
    // =============================
    public void Setup(CardData data, PlayerManager_RT owner)
    {
        this.owner = owner;
        ui = GetComponent<UnitUI>();
        faction = owner.isPlayer ? Faction.Player : Faction.CPU;

        maxHP = data.hp;
        currentHP = maxHP;
        attack = data.attack;

        Debug.Log(
            $"[Unit Setup] name={data.cardName}, " +
            $"faction={faction}, hp={currentHP}, atk={attack}"
        );

        attackCoroutine = StartCoroutine(AutoAttackLoop());
    }

    // =============================
    // 自動攻撃ループ
    // =============================
    private IEnumerator AutoAttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            Unit_RT target = GetAttackTarget();
            if (target == null)
                continue;

            Attack(target);
        }
    }

    // =============================
    // 攻撃対象取得
    // =============================
    private Unit_RT GetAttackTarget()
    {
        var bm = BattleManager_RT.Instance;

        List<Unit_RT> enemyUnits =
            faction == Faction.Player
            ? bm.cpuUnits
            : bm.playerUnits;

        if (enemyUnits.Count == 0)
            return null;

        return enemyUnits[0];
    }

    // =============================
    // ダメージ処理
    // =============================
    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (ui != null)
            ui.UpdateHP(currentHP);   // ★ UI反映

        Debug.Log($"[{name}] ダメージ {amount} 残HP={currentHP}");

        if (currentHP <= 0)
            Die();
    }
    // =============================
    // 攻撃（対象指定）
    // =============================
    public void Attack(Unit_RT target)
    {
        if (target == null)
            return;

        Debug.Log(
            $"[{faction}] {name} が " +
            $"[{target.faction}] {target.name} を攻撃 ({attack})"
        );

        target.TakeDamage(attack);
    }

    // =============================
    // 死亡処理
    // =============================
    private void Die()
    {
        Debug.Log($"[{faction}] {name} が破壊されました");

        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        BattleManager_RT.Instance.RemoveUnit(this);
        Destroy(gameObject);
    }
}
