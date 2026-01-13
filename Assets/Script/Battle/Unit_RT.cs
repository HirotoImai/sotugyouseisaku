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

    [Header("攻撃設定")]
    [SerializeField] private float attackInterval = 5f;
    private float attackTimer = 0f;

    [Header("参照")]
    public PlayerManager_RT owner;

    public CardElement element;

    private bool isDead = false;
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

        attackTimer = Random.Range(0f, attackInterval); // 同時殴り防止
    }
    void Update()
    {
        if (BattleManager_RT.Instance.isBattleFinished) return;
        if (isDead) return;

        attackTimer += Time.deltaTime;
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
    }
    // =============================
    // 攻撃処理（同時ダメージ）
    // =============================
    public void TryAttack(Unit_RT targetUnit)
    {
        if (targetUnit == null || targetUnit.currentHP <= 0) return;

        int myDamage = attack;
        int enemyDamage = targetUnit.attack;

        targetUnit.TakeDamage(myDamage);
        TakeDamage(enemyDamage);
    }
    public void TryAttackBase(PlayerManager_RT targetPlayer)
    {
        if (targetPlayer == null) return;

        targetPlayer.TakeDamage(attack);
    }
    // =============================
    // ダメージ処理
    // =============================
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHP -= amount;

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
