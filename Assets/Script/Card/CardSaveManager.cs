using UnityEngine;
using System.IO;

public class CardSaveManager : MonoBehaviour
{
    public string saveFileName = "cards.json";
    public static CardInstance[] loadedCards; // ★追加


    private CardLoadManager cardLoadManager;
    private void Awake()
    {
        CardLoadManager loader = FindObjectOfType<CardLoadManager>();
        if (loader != null)
        {
            loadedCards = loader.LoadCards();
        }
        else
        {
            loadedCards = null; // または new CardInstance[0];
        }
    }
    [System.Serializable]

    public class CardDataSerializable
    {
        public string cardName;
        public int cost;
        public int attack;
        public int hp;
        public float r, g, b, a;
        public string imageName;
    }

    [System.Serializable]
    public class CardDataList
    {
        public CardDataSerializable[] cards;
    }

    public void OnClickSave(CardData[] cardDatas)
    {
        CardDataList list = new CardDataList();
        list.cards = new CardDataSerializable[cardDatas.Length];

        for (int i = 0; i < cardDatas.Length; i++)
        {
            CardData data = cardDatas[i];
            CardDataSerializable serial = new CardDataSerializable();

            serial.cardName = data.cardName;
            serial.cost = data.cost;
            serial.attack = data.attack;
            serial.hp = data.hp;

            serial.r = data.mainColor.r;
            serial.g = data.mainColor.g;
            serial.b = data.mainColor.b;
            serial.a = data.mainColor.a;

            serial.imageName = data.image != null ? data.image.name : "";

            list.cards[i] = serial;
        }

        string json = JsonUtility.ToJson(list, true);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, saveFileName), json);

        Debug.Log("カードを保存しました");
    }

    public CardData[] Load()
    {
        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        if (!File.Exists(path))
        {
            Debug.LogWarning("保存ファイルがありません");
            return new CardData[0];
        }

        string json = File.ReadAllText(path);
        CardDataList list = JsonUtility.FromJson<CardDataList>(json);

        CardData[] result = new CardData[list.cards.Length];

        for (int i = 0; i < list.cards.Length; i++)
        {
            CardDataSerializable serial = list.cards[i];

            CardData data = ScriptableObject.CreateInstance<CardData>();

            data.cardName = serial.cardName;
            data.cost = serial.cost;
            data.attack = serial.attack;
            data.hp = serial.hp;

            data.mainColor = new Color(serial.r, serial.g, serial.b, serial.a);

            data.image = Resources.Load<Sprite>("Cards/" + serial.imageName);

            result[i] = data;
        }

        return result;
    }
}
