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
    private float lastAttackTime;
    [Header("攻撃設定")]
    [SerializeField] private float attackInterval = 5f;
    private float attackCooldown = 0f;
    private float attackTimer = 0f;
    [Header("参照")]
    public PlayerManager_RT owner;

    private bool isDead = false;
    public CardElement element;
    // =============================
    // 初期化（出撃時に必ず呼ばれる）
    // =============================
    public void Setup(CardData data, PlayerManager_RT owner)
    {
        this.owner = owner;
        faction = owner.isPlayer ? Faction.Player : Faction.CPU;

        maxHP = data.hp;
        currentHP = maxHP;
        attack = data.attack;
        element = CardElementUtility.GetElement(data.mainColor);
        attackCooldown = Random.Range(0f, attackInterval); // 同時殴り防止

        Debug.Log(
            $"[Unit Setup] name={data.cardName}, " +
            $"faction={faction}, hp={currentHP}, atk={attack}"
        );
    }
    void Update()
    {
        if (BattleManager_RT.Instance.isBattleFinished) return;
        if (isDead) return;

        attackTimer += Time.deltaTime;
    }

    // =============================
    // 攻撃処理（同時ダメージ）
    // =============================
    public void TryAttack(Unit_RT targetUnit, PlayerManager_RT targetPlayer)
    {
        // クールタイム
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        // ユニット攻撃（相打ち）
        if (targetUnit != null && targetUnit.currentHP > 0)
        {
            int myDamage = attack;
            int enemyDamage = targetUnit.attack;

            targetUnit.TakeDamage(myDamage);
            TakeDamage(enemyDamage);
            return;
        }

        // 本体攻撃
        if (targetPlayer != null)
        {
            targetPlayer.TakeDamage(attack);
        }
    }
    public void TryAttackBase(PlayerManager_RT targetPlayer)
    {
        if (targetPlayer == null) return;
        if (isDead) return;
        if (BattleManager_RT.Instance.isBattleFinished) return;

        Debug.Log(
            $"[{faction}] {name} が本体 [{targetPlayer.name}] を攻撃 ({attack})"
        );

        BattleManager_RT.Instance.DamagePlayer(targetPlayer, attack);
    }
    public bool CanAttack()
    {
        return attackTimer >= attackInterval;
    }

    public void ResetAttackTimer()
    {
        attackTimer = 0f;
    }


    // =============================
    // ダメージ処理
    // =============================
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHP -= amount;
        Debug.Log($"[{faction}] {name} ダメージ {amount} 残HP={currentHP}");

        // UI更新（あれば）
        var ui = GetComponent<UnitUI>();
        if (ui != null)
        {
            ui.UpdateHP(currentHP);
        }

        if (currentHP <= 0)
        {
            Die();
        }
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
