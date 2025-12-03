using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager_RT : MonoBehaviour
{
    [Header("UI")]
    public Slider hpBar;
    public Slider manaBar;

    [Header("Hand UI")]
    public GameObject cardButtonPrefab;
    public Transform handArea;
    public int startHandSize = 3;

    [Header("Status")]
    public int maxHP = 100;
    public int currentHP;
    public int maxMana = 100;
    public float currentMana = 0f; // float
    public float manaRegenPerSecond = 5f; // 元の方式
    public bool isPlayer;

    private List<GameObject> handButtons = new List<GameObject>();

    void Start()
    {
        currentHP = maxHP;
        hpBar.maxValue = maxHP;
        hpBar.value = currentHP;

        manaBar.maxValue = maxMana;
        manaBar.value = currentMana;

        // 初期手札ドロー
        BattleManager_RT.Instance.DrawStartHand(this, startHandSize);
        UpdateHandUI();
    }

    void Update()
    {
        // マナ回復（floatで滑らか）
        currentMana += manaRegenPerSecond * Time.deltaTime;
        if (currentMana > maxMana)
            currentMana = maxMana;

        manaBar.value = currentMana;
    }

    public void UpdateHandUI()
    {
        // 既存のボタンをクリア
        foreach (var btn in handButtons)
            Destroy(btn);
        handButtons.Clear();

        List<CardData> hand = isPlayer ? BattleManager_RT.Instance.playerHand : BattleManager_RT.Instance.cpuHand;

        foreach (var card in hand)
        {
            GameObject obj = Instantiate(cardButtonPrefab, handArea);
            CardButtonView view = obj.GetComponent<CardButtonView>();
            view.Setup(card);
            view.SetOwner(this);
            handButtons.Add(obj);
        }
    }

    public void DeployCard(CardData card)
    {
        if (currentMana < card.cost)
        {
            Debug.Log("マナ不足！");
            return;
        }

        currentMana -= card.cost;
        manaBar.value = currentMana;

        BattleManager_RT.Instance.PlayCard(this, card);
        UpdateHandUI();
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        hpBar.value = currentHP;

        if (currentHP <= 0)
            Debug.Log(isPlayer ? "プレイヤーの敗北" : "CPUの敗北");
    }
}
