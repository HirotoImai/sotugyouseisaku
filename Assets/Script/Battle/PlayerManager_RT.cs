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

    [Header("プレイヤー情報")]
    public bool isPlayer = true;

    [Header("ステータス")]
    public Slider hpSlider;
    public Slider manaSlider;
    public int maxHP = 20;
    public float maxMana = 10f;
    public int currentHP = 20;
    public float currentMana;
    public float manaRegenPerSecond = 1f;

    private List<GameObject> handButtons = new List<GameObject>();
    [Header("デッキ")]
    public List<CardData> deck = new List<CardData>();
    void Start()
    {
        currentHP = maxHP;
        currentMana = 0;
        UpdateHPUI();
        UpdateManaUI();
    }
    void Update()
    {
        // 毎フレーム滑らかに回復（1秒で manaRegenPerSecond）
        RegenerateMana(Time.deltaTime);
    }
    private void RegenerateMana(float delta)
    {
        if (currentMana < maxMana)
        {
            currentMana += manaRegenPerSecond * delta;
            if (currentMana > maxMana) currentMana = maxMana;
            UpdateManaUI();
        }
    }



    // -----------------------------
    // 手札UI更新
    // -----------------------------
    public void UpdateHandUI(List<CardInstance> handList)
    {
        //Debug.Log($"[{(isPlayer ? "Player" : "CPU")}] UpdateHandUI start. handList count={handList.Count}");

        // 不要なボタンを削除
        for (int i = handButtons.Count - 1; i >= 0; i--)
        {
            var cbv = handButtons[i].GetComponent<CardButtonView>();
            if (!handList.Contains(cbv.GetCardInstance()))
            {
                Destroy(handButtons[i]);
                handButtons.RemoveAt(i);
            }

        }

        // 新しいカードを生成
        foreach (var card in handList)
        {
            if (!handButtons.Any(b =>
                b.GetComponent<CardButtonView>().GetCardInstance() == card))
            {
                CreateCardButton(card);
            }
        }


        //Debug.Log($"[{(isPlayer ? "Player" : "CPU")}] 手札生成完了: {handButtons.Count}個");
    }

    private void CreateCardButton(CardInstance card)
    {
        GameObject go = Instantiate(cardButtonPrefab, handArea);
        var cbv = go.GetComponent<CardButtonView>();
        cbv.Setup(card);
        cbv.SetOwner(this);
        handButtons.Add(go);
    }




    // -----------------------------
    // HP/Mana UI更新
    // -----------------------------
    public void UpdateHPUI()
    {
        if (hpSlider != null)
            hpSlider.value = Mathf.Clamp01((float)currentHP / maxHP);
    }

    public void UpdateManaUI()
    {
        if (manaSlider != null)
            manaSlider.value = Mathf.Clamp01(currentMana / maxMana);
    }

    // -----------------------------
    // HP/Mana操作
    // -----------------------------
    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(currentHP - amount, 0);
        UpdateHPUI();
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        UpdateHPUI();
    }

    public bool UseMana(float amount)
    {
        if (currentMana < amount)
            return false; // 足りない

        currentMana -= amount;
        UpdateManaUI();
        return true;
    }

    public void RecoverMana(float amount)
    {
        currentMana = Mathf.Min(currentMana + amount, maxMana);
        UpdateManaUI();
    }
    public bool CanPlayCard(CardInstance card)
    {
        return currentMana >= card.data.cost;
    }

    public bool TryPlayCard(CardInstance card)
    {

        if (!CanPlayCard(card))
            return false;
        UseMana(card.data.cost+1);
        BattleManager_RT.Instance.PlayCard(this, card);

        return true;
    }

}
