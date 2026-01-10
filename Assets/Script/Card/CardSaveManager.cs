using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardSaveManager : MonoBehaviour
{
    public NameChanger nameChanger;
    public ImageColorImporter colorImporter;
    public CostManager costManager;

    public static List<CardData> loadedCards = new List<CardData>(); // 全カードデータ
    public static CardData lastCreatedCard; // 直近のカード

    private string savePath;

    // カードロード完了イベント
    public delegate void OnCardsLoaded();
    public static event OnCardsLoaded CardsLoadedEvent;

    void Awake()
    {
        Debug.Log("CardSaveManager Awake");
        savePath = Path.Combine(Application.persistentDataPath, "cards.json");
        Debug.Log($"カードデータ保存先: {savePath}");
        LoadAllCards();
    }

    public void OnClickSave()
    {
        if (colorImporter.displayImage.sprite == null)
        {
            Debug.LogWarning("画像が選択されていません");
            return;
        }

        CardDataSerializable newCard = new CardDataSerializable
        {
            cardName = nameChanger.nameInputField.text,
            cost = costManager.cost_dropdown.value + 1,
            attack = costManager.atack_dropdown.value,
            hp = costManager.hp_dropdown.value,
            color = ColorUtility.ToHtmlStringRGB(colorImporter.displayImage.color),
            imagePath = "image_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png"
        };

        SaveCardImage(colorImporter.displayImage.sprite.texture, newCard.imagePath);

        List<CardDataSerializable> allCards = LoadAllCardSerializable();
        allCards.Add(newCard);

        string json = JsonUtility.ToJson(new CardDataListWrapper(allCards), true);
        File.WriteAllText(savePath, json);

        lastCreatedCard = ConvertToCardData(newCard);

        Debug.Log($"カードを追加保存しました: {newCard.cardName}");
    }

    public void LoadAllCards()
    {
        loadedCards.Clear();
        List<CardDataSerializable> loadedList = LoadAllCardSerializable();

        foreach (var c in loadedList)
        {
            Texture2D tex = LoadCardImage(c.imagePath);
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            ColorUtility.TryParseHtmlString("#" + c.color, out Color color);

            // ScriptableObject で生成
            CardData card = ScriptableObject.CreateInstance<CardData>();
            card.cardName = c.cardName;
            card.cost = c.cost;
            card.attack = c.attack;
            card.hp = c.hp;
            card.image = sprite;
            card.mainColor = color;

            loadedCards.Add(card);
        }

        Debug.Log($"カードロード完了: {loadedCards.Count}枚");

        // ロード完了イベント発火
        CardsLoadedEvent?.Invoke();
    }

    List<CardDataSerializable> LoadAllCardSerializable()
    {
        if (!File.Exists(savePath))
            return new List<CardDataSerializable>();

        string json = File.ReadAllText(savePath);
        CardDataListWrapper wrapper = JsonUtility.FromJson<CardDataListWrapper>(json);
        return wrapper.cards ?? new List<CardDataSerializable>();
    }

    void SaveCardImage(Texture2D texture, string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        byte[] pngData = texture.EncodeToPNG();
        File.WriteAllBytes(path, pngData);
    }

    Texture2D LoadCardImage(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(path))
        {
            Debug.LogWarning("カード画像が見つかりません: " + path);
            return Texture2D.whiteTexture;
        }

        byte[] bytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        return tex;
    }

    CardData ConvertToCardData(CardDataSerializable c)
    {
        Texture2D tex = LoadCardImage(c.imagePath);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        ColorUtility.TryParseHtmlString("#" + c.color, out Color color);

        CardData card = ScriptableObject.CreateInstance<CardData>();
        card.cardName = c.cardName;
        card.cost = c.cost;
        card.attack = c.attack;
        card.hp = c.hp;
        card.image = sprite;
        card.mainColor = color;

        return card;
    }
}

[System.Serializable]
public class CardDataListWrapper
{
    public List<CardDataSerializable> cards;
    public CardDataListWrapper(List<CardDataSerializable> cards)
    {
        this.cards = cards;
    }
}

[System.Serializable]
public class CardDataSerializable
{
    public string cardName;
    public int cost;
    public int attack;
    public int hp;
    public string color;
    public string imagePath;
}
