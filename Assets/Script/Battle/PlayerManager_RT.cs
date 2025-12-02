using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerManager_RT : MonoBehaviour
{
    public Slider hpBar;
    public Slider manaBar;
    public Transform handArea;
    public CardButtonView[] handButtons;
    public List<CardData> handCards = new List<CardData>();

    public int maxHP = 100;
    public int currentHP;
    public int maxMana = 100;
    public int currentMana = 0;
    public bool isPlayer;

    public List<CardData> deck = new List<CardData>();

    void Start()
    {
        currentHP = maxHP;
        hpBar.maxValue = maxHP;
        hpBar.value = currentHP;

        manaBar.maxValue = maxMana;
        manaBar.value = currentMana;

        // デッキ構築・初期手札
        BuildDeck(10);
        DrawInitialHand(5);
    }

    void Update()
    {
        // 毎フレームマナ回復
        currentMana = Mathf.Min(currentMana + 1, maxMana);
        manaBar.value = currentMana;
    }

    public void Initialize(bool player)
    {
        isPlayer = player;
        DrawInitialHand(5);
    }

    public void UpdateHandUI()
    {
        for (int i = 0; i < handButtons.Length; i++)
        {
            if (i < handCards.Count)
            {
                handButtons[i].Setup(handCards[i]);
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

        // 手札から出したカードを削除
        if (handCards.Contains(data))
            handCards.Remove(data);

        // デッキから補充（デッキが残っている場合のみ）
        if (deck.Count > 0)
        {
            var newCard = deck[0];
            deck.RemoveAt(0);
            handCards.Add(newCard);
        }
        else
        {
            // デッキが尽きた場合は補充なし
            Debug.Log("デッキが尽きました");
        }

        UpdateHandUI();

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

    // デッキ構築
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
    }

    // 初期手札をデッキから順番に引く
    public void DrawInitialHand(int count)
    {
        handCards.Clear();
        for (int i = 0; i < count && deck.Count > 0; i++)
        {
            var card = deck[0];
            deck.RemoveAt(0);
            handCards.Add(card);
        }

        UpdateHandUI();
    }
}
