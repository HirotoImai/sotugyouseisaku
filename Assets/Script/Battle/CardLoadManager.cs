using UnityEngine;
using System.IO;

public class CardLoadManager : MonoBehaviour
{
    public string jsonFileName = "cards.json";

    public CardInstance[] LoadCards()
    {
        string path = Path.Combine(Application.persistentDataPath, jsonFileName);
        if (!File.Exists(path))
        {
            Debug.LogError("カードJSONが見つかりません: " + path);
            return new CardInstance[0];
        }

        string jsonText = File.ReadAllText(path);
        JsonCardDataList list = JsonUtility.FromJson<JsonCardDataList>(jsonText);

        CardInstance[] result = new CardInstance[list.cards.Length];

        for (int i = 0; i < list.cards.Length; i++)
        {
            JsonCardData jsonCard = list.cards[i];

            // ★テンプレート読み込み（CardData を使う）
            CardData template = Resources.Load<CardData>("Cards/" + jsonCard.cardName);
            if (template == null)
            {
                Debug.LogWarning("テンプレートが見つからない: " + jsonCard.cardName);
                continue;
            }

            // ★CardInstance の仕様に合わせて生成（template + json）
            CardInstance instance = new CardInstance(template, jsonCard);

            result[i] = instance;
        }

        return result;
    }
}
