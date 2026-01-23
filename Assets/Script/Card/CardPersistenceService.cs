using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardPersistenceService : MonoBehaviour
{
    public static CardPersistenceService Instance;

    private static List<CardData> loadedCards = new();
    public static IReadOnlyList<CardData> Cards => loadedCards;

    private string savePath;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "cards.json");
        LoadAllCards();
    }

    public void LoadAllCards()
    {
        loadedCards.Clear();

        if (!File.Exists(savePath))
        {
            Debug.Log("[Persistence] 保存ファイルなし");
            return;
        }

        string json = File.ReadAllText(savePath);
        CardDataListWrapper wrapper = JsonUtility.FromJson<CardDataListWrapper>(json);

        if (wrapper?.cards == null)
            return;

        foreach (var c in wrapper.cards)
        {
            loadedCards.Add(CreateCardData(c));
        }

        Debug.Log($"[Persistence] カードロード完了: {loadedCards.Count}枚");
    }

    private CardData CreateCardData(CardDataSerializable c)
    {
        Texture2D tex = LoadCardImage(c.imagePath);
        Sprite sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f)
        );

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

    private Texture2D LoadCardImage(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(path))
            return Texture2D.whiteTexture;

        byte[] bytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        return tex;
    }
}
