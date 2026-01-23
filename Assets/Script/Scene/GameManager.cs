using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public DeckData CurrentDeck { get; private set; }
    public DeckData CPUDeck { get; private set; }
    public string PlayerName = "テスト太郎";
    public int PlayerCoins = 100;
    private const string DeckPrefsKey = "SavedDeck";
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

        LoadDeck();
        // CPUデッキ初期化（保存なし）
        if (CPUDeck == null)
            CPUDeck = ScriptableObject.CreateInstance<DeckData>();

        if (CPUDeck.cardIDs == null)
            CPUDeck.InitializeAsEmpty();

        // CPU用の固定デッキをセット
        CPUDeck.cardIDs = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        Debug.Log("[GameManager] CPUデッキを初期化: " + CPUDeck.cardIDs.Count + " 枚");
    }
    public void SaveDeck()
    {
        if (CurrentDeck == null) return;

        string json = JsonUtility.ToJson(CurrentDeck);
        PlayerPrefs.SetString(DeckPrefsKey, json);
        PlayerPrefs.Save();
        Debug.Log("[GameManager] デッキを保存しました");
    }

    public void LoadDeck()
    {
        if (!PlayerPrefs.HasKey(DeckPrefsKey)) return;

        string json = PlayerPrefs.GetString(DeckPrefsKey);
        JsonUtility.FromJsonOverwrite(json, CurrentDeck);
        Debug.Log("[GameManager] デッキをロードしました: " + CurrentDeck.cardIDs.Count + " 枚");
    }
    private void InitializeDefaultDeck()
    {
        var cards = CardDatabase.Instance.fixedCards;

        foreach (var card in cards.Take(20))
            CurrentDeck.cardIDs.Add(card.cardID);

        Debug.Log($"[GameManager] Default deck initialized ({CurrentDeck.cardIDs.Count})");
    }
    private List<int> CreateFixedCPUDeck()
    {
        return new List<int> { 0,1,2,3,4,5,6,7,8,9,10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
    }
}
