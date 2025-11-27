using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerManager_RT : MonoBehaviour
{
    public Slider hpBar;
    public Slider manaBar;
    public Transform handArea;
    public CardButtonView[] handButtons;
    public List<CardData> handCards;

    public int maxHP = 100;
    public int currentHP;
    public int maxMana = 100;
    public int currentMana = 0;
    public bool isPlayer;

    void Start()
    {
        currentHP = maxHP;
        hpBar.maxValue = maxHP;
        hpBar.value = currentHP;

        manaBar.maxValue = maxMana;
        manaBar.value = currentMana;

        // カード関連
        BuildDeck(10); // デッキ10枚
        DrawHand(5);  // 手札5枚
    }

    void Update()
    {
        // 毎フレーム少しずつマナ回復（仮）
        currentMana = Mathf.Min(currentMana + 1, maxMana);
        manaBar.value = currentMana;
    }

    public void Initialize(bool player)
    {
        isPlayer = player;

        // ここで手札をランダムに引く
        DrawHand(5);
    }
    public void UpdateHandUI()
    {
        for (int i = 0; i < handButtons.Length; i++)
        {
            if (i < handCards.Count)
            {
                // ← 修正：1引数でセット
                handButtons[i].Setup(handCards[i]);

                // ← このプレイヤーをセット
                handButtons[i].SetOwner(this);

                handButtons[i].gameObject.SetActive(true);
            }
            else
            {
                handButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void DeployCard(CardData data)
    {
        if (currentMana < data.cost)
        {
            Debug.Log("マナ不足！");
            return;
        }

        currentMana -= data.cost;
        Debug.Log($"{data.cardName}（ATK:{data.attack}）を出撃！");

        BattleManager_RT.Instance.SpawnCard(data, isPlayer);
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        hpBar.value = currentHP;
        if (currentHP <= 0)
        {
            Debug.Log(isPlayer ? "プレイヤーの敗北" : "敵の敗北");
        }
    }
    public List<CardData> deck = new List<CardData>();

    public void BuildDeck(int count = 10)
    {
        deck.Clear();

        var pool = CardDatabase.Instance.allCards;

        if (pool.Count == 0)
        {
            Debug.LogError("デッキ生成用カードがありません！");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            int r = Random.Range(0, pool.Count);
            deck.Add(pool[r]);
        }

        DrawHand(5); // 手札に配る
    }
    public void DrawHand(int count)
    {
        var pool = CardDatabase.Instance.allCards;
        if (pool.Count == 0)
        {
            Debug.LogWarning("カードがロードされていません");
            return;
        }

        handCards.Clear();
        for (int i = 0; i < count; i++)
        {
            int r = Random.Range(0, pool.Count);
            handCards.Add(pool[r]);
        }

        UpdateHandUI();
    }
}