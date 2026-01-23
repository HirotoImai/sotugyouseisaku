using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager_RT : MonoBehaviour
{
    [Header("手札UI")]
    public Transform handArea;
    public GameObject cardButtonPrefab;

    private List<GameObject> handButtons = new();
    [Header("プレイヤー情報")]
    public bool isPlayer = true;

    [Header("ステータス")]
    public Slider hpSlider;
    public Slider manaSlider;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI manaText;
    public int maxHP = 20;
    public float maxMana = 10f;
    public int currentHP;
    public float currentMana;
    public float manaRegenPerSecond = 1f;

    [Header("デッキ")]
    public BattleDeck battleDeck;
    public List<CardInstance> hand = new();
    [SerializeField] private int initialHandCount = 5;
    void Start()
    {
        var deck = GameManager.Instance.CurrentDeck;

        Debug.Log($"[Battle] Deck card count = {deck.cardIDs.Count}");
        foreach (var id in deck.cardIDs)
        {
            Debug.Log($"CardID: {id}");
        }
        currentHP = maxHP;
        currentMana = 0;
        UpdateHPUI();
        UpdateManaUI();
        Debug.Log($"[PlayerManager] Start 呼ばれた ({gameObject.name})");
    }
    void Update()
    {
        RegenerateMana(Time.deltaTime);
    }

    public void Initialize(DeckData deckData)
    {

        battleDeck = new BattleDeck(deckData);
        hand.Clear();
        ClearHandUI();
        for (int i = 0; i < initialHandCount; i++)
            DrawCard();
    }
    public void InitializeCPU(DeckData deckData)
    {
        battleDeck = new BattleDeck(deckData);
        hand.Clear();
        ClearHandUI();
        for (int i = 0; i < initialHandCount; i++)
            DrawCard();
    }
    // =============================
    // ドロー
    // =============================
    public void DrawCard()
    {
        if (battleDeck == null)
            return;
        int cardID = battleDeck.Draw();
        if (cardID < 0) return;
        CardData data= CardDatabase.Instance.GetCardByID(cardID);
        if (data == null)
        {
            Debug.LogError($"CardData が見つかりません: {cardID}");
            return;
        }
        hand.Add(new CardInstance(data));
        UpdateHandUI();

    }
    // =============================
    // カード使用判定
    // =============================
    public bool CanPlayCard(CardInstance card)
    {
        int cost = battleDeck.GetModifiedCost(card.data);
        return currentMana >= cost;
    }

    public bool TryPlayCard(CardInstance card)
    {

        if (!hand.Contains(card))
            return false;
        if (!CanPlayCard(card))
            return false;
        int cost = battleDeck.GetModifiedCost(card.data);
        UseMana(cost);
        hand.Remove(card);
        RemoveHandButton(card);
        BattleManager_RT.Instance.PlayCard(this, card);

        DrawCard();
        return true;
    }
    private void RegenerateMana(float delta)
    {
        if (currentMana >= maxMana)

            return;

            currentMana += manaRegenPerSecond * delta;
            if (currentMana > maxMana) currentMana = maxMana;
            UpdateManaUI();
    }



    // -----------------------------
    // 手札UI更新
    // -----------------------------
    public void UpdateHandUI()
    {
        // 不要なボタンを削除
        for (int i = handButtons.Count - 1; i >= 0; i--)
        {
            var view = handButtons[i].GetComponent<CardButtonView>();
            if (!hand.Any(h => h == view.cardInstance))
            {
                Destroy(handButtons[i]);
                handButtons.RemoveAt(i);
            }

        }

        // 新しいカードを生成
        foreach (var instance in hand)
        {
            bool exists = handButtons.Any(b => b.GetComponent<CardButtonView>().cardInstance == instance);
            if (!exists)
            {
                CreateCardButton(instance);
            }
        }
    }
    public void ClearHandUI()
    {
        foreach (var go in handButtons)
            Destroy(go);

        handButtons.Clear();
    }
    private void CreateCardButton(CardInstance instance)
    {
        GameObject go = Instantiate(cardButtonPrefab, handArea);
        var cbv = go.GetComponent<CardButtonView>();
        cbv.Setup(instance);
        cbv.SetOwner(this);
        handButtons.Add(go);
    }
    void RemoveHandButton(CardInstance instance)
    {
        for (int i = handButtons.Count - 1; i >= 0; i--)
        {
            var view = handButtons[i].GetComponent<CardButtonView>();
            if (view.cardInstance == instance)
            {
                Destroy(handButtons[i]);
                handButtons.RemoveAt(i);
                return;
            }
        }
    }

    // -----------------------------
    // HP/Mana UI更新
    // -----------------------------
    public void UpdateHPUI()
    {
        if (hpSlider != null)
            hpSlider.value = Mathf.Clamp01((float)currentHP / maxHP);
        hpText.text = currentHP.ToString();
    }

    public void UpdateManaUI()
    {

        if (manaSlider != null)
            manaSlider.value = Mathf.Clamp01(currentMana / maxMana);
        int manaT = (int)currentMana;
        manaText.text = manaT.ToString();
    }

    // -----------------------------
    // HP/Mana操作
    // -----------------------------
    public void TakeDamage(int amount)
    {
        if (currentHP <= 0) return;

        currentHP = Mathf.Max(currentHP - amount, 0);
        UpdateHPUI();

        BattleManager_RT.Instance.CheckBattleResult();
    }

    public bool UseMana(float amount)
    {
        if (currentMana < amount)
            return false; // 足りない

        currentMana -= amount;
        UpdateManaUI();
        return true;
    }
}
