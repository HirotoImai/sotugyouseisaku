using UnityEngine;
using System.IO;
using System.Collections.Generic;

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

        List<CardInstance> result = new List<CardInstance>();

        for (int i = 0; i < list.cards.Length; i++)
        {
            JsonCardData jsonCard = list.cards[i];

            // ★テンプレートが名前で Resources から存在するか？
            CardData template = Resources.Load<CardData>("Cards/" + jsonCard.cardName);

            if (template == null)
            {
                Debug.LogWarning("テンプレートが見つからない: " + jsonCard.cardName);
                continue; // 絶対に null を result に入れない
            }

            CardInstance instance = new CardInstance(template, jsonCard);
            result.Add(instance);
        }

        return result.ToArray();
    }
}
