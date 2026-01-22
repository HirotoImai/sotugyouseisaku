using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public DeckData CurrentDeck { get; private set; }
    public string PlayerName = "テスト太郎";
    public int PlayerCoins = 100;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (CurrentDeck == null)
        {
            CurrentDeck = ScriptableObject.CreateInstance<DeckData>();
            CurrentDeck.InitializeAsEmpty();
        }

        // 保存データがあればここで上書き（DB不要）
        DeckSaveManager.LoadInto(CurrentDeck);
    }

}
