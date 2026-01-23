using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeckBuildManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text deckCountText;
    [SerializeField] private Button confirmButton;

    [Header("Deck")]
    [SerializeField] private int maxDeckCount = 20;
    [SerializeField] private int minDeckCount = 10;
    private DeckData editingDeck;

    [Header("Deck UI")]
    [SerializeField] private Transform deckArea;
    [SerializeField] private Transform cardListArea;
    [SerializeField] private GameObject deckCardPrefab;
    public SaveSoundPlayer soundPlayer;
    private void Start()
    {
        CardDatabase.Instance.Rebuild();
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager が存在しません");
            return;
        }

        editingDeck = ScriptableObject.CreateInstance<DeckData>();
        editingDeck.cardIDs = new List<int>(
            GameManager.Instance.CurrentDeck.cardIDs
        );


        if (editingDeck.cardIDs.Count == 0)
            InitializeDefaultDeck();

        RefreshCardList();
        RefreshDeckView();
        UpdateDeckCountUI();
        Debug.Log($"[Deck] 編集後枚数: {editingDeck.cardIDs.Count}");
    }

    // =========================
    // 所持カード一覧
    // =========================
    private void RefreshCardList()
    {
        foreach (Transform c in cardListArea)
            Destroy(c.gameObject);

        foreach (var card in CardDatabase.Instance.allCards)
        {
            GameObject go = Instantiate(deckCardPrefab, cardListArea);
            DeckCardView view = go.GetComponent<DeckCardView>();
            view.Setup(card, TryAddCard);
        }
    }

    // =========================
    // デッキ表示
    // =========================
    private void RefreshDeckView()
    {
        foreach (Transform c in deckArea)
            Destroy(c.gameObject);

        foreach (int id in editingDeck.cardIDs)
        {
            CardData card = CardDatabase.Instance.GetCardByID(id);
            if (card == null) continue;

            GameObject go = Instantiate(deckCardPrefab, deckArea);
            DeckCardView view = go.GetComponent<DeckCardView>();
            view.Setup(card, RemoveCard);
        }
    }

    // =========================
    // デッキ操作
    // =========================
    public void TryAddCard(CardData card)
    {
        Debug.Log($"TryAddCard: {card.cardName}");
        if (editingDeck.cardIDs.Count >= maxDeckCount)
            return;

        if (!editingDeck.CanAddCard(card))
        {
            Debug.Log("CanAddCard = false");
            return;
        }

        editingDeck.AddCard(card);
        RefreshDeckView();
        UpdateDeckCountUI();
    }

    public void RemoveCard(CardData card)
    {
        editingDeck.RemoveCard(card.cardID);
        RefreshDeckView();
        UpdateDeckCountUI();
    }

    // =========================
    // UI
    // =========================
    private void UpdateDeckCountUI()
    {
        int count = editingDeck.cardIDs.Count;
        deckCountText.text = $"{count} / {maxDeckCount}";
        confirmButton.interactable = count >= minDeckCount;
    }

    // =========================
    // 初期デッキ
    // =========================
    private void InitializeDefaultDeck()
    {
        var db = CardDatabase.Instance;
        if (db == null) return;

        editingDeck.cardIDs.Clear();
        for (int i = 0; i < DeckData.MaxDeckSize && i < db.allCards.Count; i++)
        {
            editingDeck.cardIDs.Add(db.allCards[i].cardID);
        }
    }

    // =========================
    // シーン遷移
    // =========================
    public void BackToHome()
    {
        SceneManager.LoadScene("Home");
    }

    public void OnConfirmDeck(int min)
    {
        int count = editingDeck.cardIDs.Count;
        if (count < minDeckCount)
        {
            soundPlayer?.PlayFail();
            Debug.Log("デッキ枚数が不足しています");
            return;
        }
        if (!editingDeck.IsSavable(min))
        {
            soundPlayer?.PlayFail();
            Debug.Log("デッキが未完成です");
            return;
        }
        soundPlayer?.PlaySuccess();
        GameManager.Instance.CurrentDeck.cardIDs =
                new List<int>(editingDeck.cardIDs);
        SceneManager.LoadScene("Home");
    }
}
