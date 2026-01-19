using UnityEngine;
using System.Collections;
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

    [Header("攻撃設定")]
    [SerializeField] private float attackInterval = 5f;
    private float attackTimer = 0f;
    [SerializeField] private float preAttackRatio = 0.9f;
    private bool preAttackNotified = false;
    [Header("参照")]
    public PlayerManager_RT owner;

    public CardElement element;

    private bool isDead = false;
    // =============================
    // 初期化（出撃時に必ず呼ばれる）
    // =============================
    public void Initialize(CardData data, PlayerManager_RT owner)
    {
        this.owner = owner;
        faction = owner.isPlayer ? Faction.Player : Faction.CPU;

        maxHP = data.hp;
        currentHP = maxHP;
        attack = data.attack;

        // ★ ここが重要
        element = data.element;

        attackTimer = Random.Range(0f, attackInterval);

        var ui = GetComponent<UnitUI>();
        if (ui != null)
            ui.UpdateHP(currentHP, maxHP);
    }

    public void Setup(CardData data, PlayerManager_RT owner)
    {
        this.owner = owner;
        faction = owner.isPlayer ? Faction.Player : Faction.CPU;

        maxHP = data.hp;
        currentHP = maxHP;
        attack = data.attack;
        element = data.element;
        Debug.Log($"[SETUP] {data.cardName} Element = {element}");
        attackTimer = Random.Range(0f, attackInterval); // 同時殴り防止

        var ui = GetComponent<UnitUI>();
        if (ui != null)
        {
            ui.UpdateHP(currentHP, maxHP);
        }
    }
    void Update()
    {
        if (BattleManager_RT.Instance.isBattleFinished) return;
        if (isDead) return;

        // ★ 前列でなければ何もしない
        if (!BattleManager_RT.Instance.IsFrontUnit(this))
            return;

        attackTimer += Time.deltaTime;

        float rate = attackTimer / attackInterval;

        var ui = GetComponent<UnitUI>();
        if (ui != null)
            ui.UpdateAttackGauge(rate);

        // 攻撃直前演出
        if (!preAttackNotified && rate >= preAttackRatio)
        {
            preAttackNotified = true;
            if (ui != null)
                ui.PlayPreAttackMotion();
        }
    }

    // =============================
    // 攻撃可否
    // =============================
    public bool CanAttack()
    {
        return attackTimer >= attackInterval;
    }

    public void ResetAttackTimer()
    {
        attackTimer = 0f;
        preAttackNotified = false;

        var ui = GetComponent<UnitUI>();
        if (ui != null)
            ui.UpdateAttackGauge(0f);
    }
    // =============================
    // 攻撃処理（同時ダメージ）
    // =============================
    // ユニット同士の攻撃（属性計算あり）
    public void TryAttack(Unit_RT targetUnit, PlayerManager_RT targetPlayer)
    {
        if (targetUnit == null || targetUnit.currentHP <= 0)
        {
            ResetAttackTimer();   // ★ 必ずリセット
            return;
        }

        float myMultiplier = GetElementMultiplier(this.element, targetUnit.element);
        float enemyMultiplier = GetElementMultiplier(targetUnit.element, this.element);

        int myDamage = Mathf.RoundToInt(attack * myMultiplier);
        int enemyDamage = Mathf.RoundToInt(targetUnit.attack * enemyMultiplier);

        Debug.Log(
            $"[ATTACK] {element} -> {targetUnit.element} | 倍率:{myMultiplier} ダメージ:{myDamage}"
        );

        targetUnit.TakeDamage(myDamage);
        TakeDamage(enemyDamage);

        ResetAttackTimer(); // ★ 成功時もリセット
    }



    // プレイヤー本体への攻撃（属性なし）
    public void TryAttackBase(PlayerManager_RT targetPlayer)
    {
        if (targetPlayer == null)
        {
            ResetAttackTimer();
            return;
        }

        Debug.Log($"[BASE ATTACK] {element} -> Player | ダメージ:{attack}");

        targetPlayer.TakeDamage(attack);
        ResetAttackTimer();
    }

    private float GetElementMultiplier(CardElement attacker, CardElement defender)
    {
        // 有利関係
        if (attacker == CardElement.Red && defender == CardElement.Green) return 1.5f;
        if (attacker == CardElement.Green && defender == CardElement.Blue) return 1.5f;
        if (attacker == CardElement.Blue && defender == CardElement.Red) return 1.5f;

        // 不利関係
        if (attacker == CardElement.Red && defender == CardElement.Blue) return 0.5f;
        if (attacker == CardElement.Green && defender == CardElement.Red) return 0.5f;
        if (attacker == CardElement.Blue && defender == CardElement.Green) return 0.5f;

        // 無属性（白・黒）
        return 1.0f;
    }

    // =============================
    // ダメージ処理
    // =============================
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0);

        var ui = GetComponent<UnitUI>();
        if (ui != null)
        {
            ui.UpdateHP(currentHP, maxHP);
            ui.ShowDamage(amount);
        }

        if (currentHP <= 0)
        {
            StartCoroutine(DieAfterDamage());
        }
    }

    private IEnumerator DieAfterDamage()
    {
        // ダメージポップアップが見える時間だけ待つ
        yield return new WaitForSeconds(0.6f);

        Die();
    }

    // =============================
    // 死亡処理
    // =============================
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"[{faction}] {name} が破壊されました");

        Destroy(gameObject);
    }
}
