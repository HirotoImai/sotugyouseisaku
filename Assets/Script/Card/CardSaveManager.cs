using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardSaveManager : MonoBehaviour
{
    [Header("Card Create UI")]
    public NameChanger nameChanger;
    public ImageColorImporter colorImporter;
    public CostManager costManager;

    [SerializeField] float saveCooldown = 0.5f;
    float lastSaveTime = -999f;

    private string savePath;

    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "cards.json");
    }

    public void OnClickSave()
    {
        if (Time.time - lastSaveTime < saveCooldown)
            return;

        lastSaveTime = Time.time;

        if (colorImporter.displayImage.sprite == null)
        {
            AudioManager.Instance.PlaySaveFail();
            return;
        }

        if (costManager.hp_dropdown.value == 0)
        {
            AudioManager.Instance.PlaySaveFail();
            return;
        }

        List<CardDataSerializable> list = LoadAll();

        CardDataSerializable newCard = new()
        {
            cardID = GetNextID(list),
            cardName = nameChanger.nameInputField.text,
            cost = costManager.cost_dropdown.value + 1,
            attack = costManager.atack_dropdown.value,
            hp = costManager.hp_dropdown.value,
            color = ColorUtility.ToHtmlStringRGB(colorImporter.backgroundColor),
            imagePath = $"image_{System.DateTime.Now:yyyyMMdd_HHmmss}.png",
            element = colorImporter.element
        };

        list.Add(newCard);

        SaveImage(colorImporter.displayImage.sprite.texture, newCard.imagePath);
        SaveJson(list);

        // ★ 保存後に全シーン共通データを更新
        CardPersistenceService.Instance.LoadAllCards();
        CardDatabase.Instance.Rebuild();
        AudioManager.Instance.PlaySaveSuccess();
        Debug.Log($"[Save] カード保存: {newCard.cardName}");
    }

    private List<CardDataSerializable> LoadAll()
    {
        if (!File.Exists(savePath))
            return new();

        string json = File.ReadAllText(savePath);
        CardDataListWrapper wrapper = JsonUtility.FromJson<CardDataListWrapper>(json);
        return wrapper.cards ?? new();
    }

    private void SaveJson(List<CardDataSerializable> list)
    {
        string json = JsonUtility.ToJson(new CardDataListWrapper(list), true);
        File.WriteAllText(savePath, json);
    }

    private void SaveImage(Texture2D tex, string name)
    {
        string path = Path.Combine(Application.persistentDataPath, name);
        File.WriteAllBytes(path, tex.EncodeToPNG());
    }

    private int GetNextID(List<CardDataSerializable> list)
    {
        int max = 0;
        foreach (var c in list)
            if (c.cardID > max) max = c.cardID;
        return max + 1;
    }
}
