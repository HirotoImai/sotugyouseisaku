using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardPersistenceService : MonoBehaviour
{
    public static CardPersistenceService Instance;
    public static List<CardData> LoadedCards = new();

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
        LoadedCards.Clear();

        if (!File.Exists(savePath))
            return;

        string json = File.ReadAllText(savePath);
        CardDataListWrapper wrapper = JsonUtility.FromJson<CardDataListWrapper>(json);

        foreach (var c in wrapper.cards)
        {
            CardData card = CreateCardData(c);
            LoadedCards.Add(card);
        }

        Debug.Log($"カードロード完了: {LoadedCards.Count}枚");
    }

    CardData CreateCardData(CardDataSerializable c)
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

    Texture2D LoadCardImage(string fileName)
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
