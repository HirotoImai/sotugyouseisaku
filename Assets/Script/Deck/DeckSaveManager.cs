using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class DeckSaveManager
{
    private static string SavePath =>
        Path.Combine(Application.persistentDataPath, "deck.json");

    public static void Save(DeckData deck)
    {
        DeckSaveData data = new DeckSaveData
        {
            cardIDs = new List<int>(deck.cardIDs)
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public static bool LoadInto(DeckData deck)
    {
        if (deck == null)
        {
            Debug.LogError("LoadInto: deck is null");
            return false;
        }

        if (!File.Exists(SavePath))
            return false;
        string json = File.ReadAllText(SavePath);
        DeckSaveData data = JsonUtility.FromJson<DeckSaveData>(json);

        deck.cardIDs.Clear();
        deck.cardIDs.AddRange(data.cardIDs);
        return true;
    }

}
