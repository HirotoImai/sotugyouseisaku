using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckBuildManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text deckCountText;
    [SerializeField] private Button confirmButton;

    [Header("Deck")]
    [SerializeField] private int maxDeckCount = 20;
    private DeckData editingDeck;

    [Header("Deck UI")]
    [SerializeField] private Transform deckArea;
    [SerializeField] private Transform cardListArea;
    [SerializeField] private GameObject deckCardPrefab;

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager が存在しません");
            return;
        }

        editingDeck = GameManager.Instance.CurrentDeck;
        if (editingDeck == null)
        {
            Debug.LogError("CurrentDeck が null です");
            return;
        }

        if (editingDeck.cardIDs.Count == 0)
            InitializeDefaultDeck();

        RefreshCardList();
        RefreshDeckView();
        UpdateDeckCountUI();
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
        confirmButton.interactable = count == DeckData.MaxDeckSize;
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

    public void OnConfirmDeck()
    {
        if (!editingDeck.IsComplete())
        {
            Debug.Log("デッキが未完成です");
            return;
        }

        DeckSaveManager.Save(editingDeck);
        SceneManager.LoadScene("Home");
    }
}
