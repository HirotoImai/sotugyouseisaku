using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public DeckData CurrentDeck { get; private set; }
    public string PlayerName = "テスト太郎";
    public int PlayerCoins = 100;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (CurrentDeck == null)
        {
            CurrentDeck = ScriptableObject.CreateInstance<DeckData>();
            CurrentDeck.InitializeAsEmpty();
        }
        if (CurrentDeck.cardIDs.Count == 0)
        {
            InitializeDefaultDeck();
        }

    }
    private void InitializeDefaultDeck()
    {
        var cards = CardDatabase.Instance.fixedCards;

        foreach (var card in cards.Take(20))
            CurrentDeck.cardIDs.Add(card.cardID);

        Debug.Log($"[GameManager] Default deck initialized ({CurrentDeck.cardIDs.Count})");
    }

}
