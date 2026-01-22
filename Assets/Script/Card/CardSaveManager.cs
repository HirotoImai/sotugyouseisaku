using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardSaveManager : MonoBehaviour
{
    public static CardSaveManager Instance;

    [Header("Card Create UI (Card Scene Only)")]
    public NameChanger nameChanger;
    public ImageColorImporter colorImporter;
    public CostManager costManager;
    public SaveSoundPlayer soundPlayer;

    [Header("Runtime Data")]
    public static List<CardData> loadedCards = new();
    public static CardData lastCreatedCard;

    // ロード完了イベント
    public delegate void OnCardsLoaded();
    public static event OnCardsLoaded CardsLoadedEvent;

    [SerializeField] float saveCooldown = 0.5f;
    float lastSaveTime = -999f;

    private string savePath;

    void Awake()
    {
        // シングルトン
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "cards.json");
        Debug.Log($"カードデータ保存先: {savePath}");

        LoadAllCards();
    }

    // =========================
    // 保存（Cardシーン専用）
    // =========================
    public void OnClickSave()
    {
        if (Time.time - lastSaveTime < saveCooldown)
            return;

        lastSaveTime = Time.time;

        // UI が存在しないシーン対策
        if (colorImporter == null || costManager == null)
            return;

        if (colorImporter.displayImage.sprite == null)
        {
            Debug.LogWarning("画像が選択されていません");
            soundPlayer?.PlayFail();
            return;
        }

        if (costManager.hp_dropdown.value == 0)
        {
            Debug.LogWarning("体力が0のカードは保存できません");
            soundPlayer?.PlayFail();
            return;
        }

        List<CardDataSerializable> allCards = LoadAllCardSerializable();

        int newID = GetNextCardID(allCards);

        CardDataSerializable newCard = new()
        {
            cardID = newID,
            cardName = nameChanger.nameInputField.text,
            cost = costManager.cost_dropdown.value + 1,
            attack = costManager.atack_dropdown.value,
            hp = costManager.hp_dropdown.value,
            color = ColorUtility.ToHtmlStringRGB(colorImporter.backgroundColor),
            imagePath = "image_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png",
            element = colorImporter.element
        };

        allCards.Add(newCard);

        SaveCardImage(colorImporter.displayImage.sprite.texture, newCard.imagePath);

        string json = JsonUtility.ToJson(new CardDataListWrapper(allCards), true);
        File.WriteAllText(savePath, json);

        lastCreatedCard = ConvertToCardData(newCard);
        loadedCards.Add(lastCreatedCard);

        soundPlayer?.PlaySuccess();
        CardsLoadedEvent?.Invoke();

        Debug.Log($"カードを追加保存しました: {newCard.cardName}");
    }

    // =========================
    // ロード（全シーン共通）
    // =========================
    public void LoadAllCards()
    {
        loadedCards.Clear();

        List<CardDataSerializable> list = LoadAllCardSerializable();

        foreach (var c in list)
        {
            loadedCards.Add(ConvertToCardData(c));
        }

        Debug.Log($"カードロード完了: {loadedCards.Count}枚");
        CardsLoadedEvent?.Invoke();
    }

    List<CardDataSerializable> LoadAllCardSerializable()
    {
        if (!File.Exists(savePath))
            return new();

        string json = File.ReadAllText(savePath);
        CardDataListWrapper wrapper = JsonUtility.FromJson<CardDataListWrapper>(json);
        return wrapper.cards ?? new();
    }

    void SaveCardImage(Texture2D texture, string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(path, texture.EncodeToPNG());
    }

    Texture2D LoadCardImage(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(path))
            return Texture2D.whiteTexture;

        Texture2D tex = new(2, 2);
        tex.LoadImage(File.ReadAllBytes(path));
        return tex;
    }

    CardData ConvertToCardData(CardDataSerializable c)
    {
        Texture2D tex = LoadCardImage(c.imagePath);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        ColorUtility.TryParseHtmlString("#" + c.color, out Color color);

        CardData card = ScriptableObject.CreateInstance<CardData>();
        card.cardID = c.cardID;
        card.cardName = c.cardName;
        card.cost = c.cost;
        card.attack = c.attack;
        card.hp = c.hp;
        card.image = sprite;
        card.mainColor = color;
        card.element = c.element;
        return card;
    }

    int GetNextCardID(List<CardDataSerializable> cards)
    {
        int max = 0;
        foreach (var c in cards)
            if (c.cardID > max) max = c.cardID;
        return max + 1;
    }

}
