using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CardSaveManager : MonoBehaviour
{
    private string savePath;
    public CardEditor editor;             // ← シーン上の Editor をアサイン
    public CardSaveManager saveManager;   // ← SaveManager をアサイン
    [System.Serializable]
    public class CardListWrapper
    {
        public List<JsonCardData> cards = new List<JsonCardData>();
    }

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "Cards.json");
    }
    public void OnClickSave()
    {
        JsonCardData json = editor.ToJson();  // ← UI から Json 化
        saveManager.SaveCard(json);           // ← JSON に追加保存

        Debug.Log($"保存成功: {json.cardName}");
    }
    /// <summary>
    /// 1枚のカードを追加保存する
    /// </summary>
    public void SaveCard(JsonCardData newCard)
    {
        CardListWrapper data = LoadAllCards();

        // 追加
        data.cards.Add(newCard);

        // JSONへ保存
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log($"カードを保存しました: {newCard.cardName}");
    }

    /// <summary>
    /// 全てのカードを読み込む
    /// </summary>
    public CardListWrapper LoadAllCards()
    {
        if (!File.Exists(savePath))
        {
            return new CardListWrapper();
        }

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<CardListWrapper>(json);
    }
}
