using UnityEngine;

public class HandManager : MonoBehaviour
{
    public CardButtonView[] cardSlots;
    public CardData[] templateCards;

    private CardInstance[] allCards;

    void Start()
    {
        // 保存カード読み込み（CardInstance[]）
        var saved = CardSaveManager.loadedCards;

        // テンプレートカードから CardInstance を生成
        var tempInstances = new CardInstance[templateCards.Length];
        for (int i = 0; i < templateCards.Length; i++)
        {
            // JsonCardData が無いので null を渡す
            tempInstances[i] = new CardInstance(templateCards[i], null);
        }

        // 2種類のカードを結合
        int total = saved.Length + tempInstances.Length;
        allCards = new CardInstance[total];

        int index = 0;
        foreach (var c in saved)
            allCards[index++] = c;
        foreach (var c in tempInstances)
            allCards[index++] = c;

        Debug.Log($"HandManager: カードプール {total} 枚");
    }

    public void DrawHand(int drawCount)
    {
        if (allCards == null || allCards.Length == 0)
        {
            Debug.LogWarning("HandManager: カードがロードされていません。");
            return;
        }

        for (int i = 0; i < cardSlots.Length && i < drawCount; i++)
        {
            int r = Random.Range(0, allCards.Length);
            cardSlots[i].SetOwner(allCards[r]);
        }
    }
}
