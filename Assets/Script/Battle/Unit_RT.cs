using UnityEngine;
using UnityEngine.UI;

public class Unit_RT : MonoBehaviour
{
    // カード情報
    private CardData cardData;
    private bool isPlayer;

    // ステータス
    private int currentHP;
    private int attack;

    // UI（任意でユニット上にHPバーなどを表示する場合）
    public Slider hpBar;

    // 初期化
    public void Initialize(CardData data, bool player)
    {
        if (data == null)
        {
            Debug.LogWarning("Unit_RT.Initialize: CardData が null");
            return;
        }

        cardData = data;
        isPlayer = player;

        currentHP = data.hp;
        attack = data.attack;

        if (hpBar != null)
        {
            hpBar.maxValue = data.hp;
            hpBar.value = currentHP;
        }

        // 外見（色や画像）もカード情報から反映
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && data.image != null)
        {
            sr.sprite = data.image;
            sr.color = data.mainColor;
        }

        Debug.Log($"ユニット初期化: {cardData.cardName} (HP:{currentHP}, ATK:{attack})");
    }

    // ---------------------------
    // ダメージを受ける
    // ---------------------------
    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (hpBar != null) hpBar.value = currentHP;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    // ---------------------------
    // 攻撃処理（例：相手ユニットに攻撃）
    // ---------------------------
    public void Attack(Unit_RT target)
    {
        if (target == null) return;

        Debug.Log($"{cardData.cardName} が {target.cardData.cardName} に {attack} ダメージ！");
        target.TakeDamage(attack);
    }

    // ---------------------------
    // 死亡処理
    // ---------------------------
    private void Die()
    {
        Debug.Log($"{cardData.cardName} が死亡しました");
        Destroy(gameObject);
    }
}
