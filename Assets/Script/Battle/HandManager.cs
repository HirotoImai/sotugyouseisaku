// HandManager.cs
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public CardLoadManager loadManager;  // ← インスペクターで設定
    private CardInstance[] loadedCards;

    void Start()
    {
        loadedCards = loadManager.LoadCards();

        if (loadedCards == null || loadedCards.Length == 0)
        {
            Debug.LogError("カードがロードされていません。");
            return;
        }

        Debug.Log("ロードされたカード数: " + loadedCards.Length);

        // 例：最初の3枚をハンドに追加
        DrawRandomHand();
    }

    public void DrawRandomHand()
    {
        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, loadedCards.Length);
            CardInstance c = loadedCards[index];

            Debug.Log($"カード: {c.template.cardName} の色: R={c.template.mainColor.r}, G={c.template.mainColor.g}, B={c.template.mainColor.b}");

            // 表示処理へ渡す
            // CardButtonView などへ渡す処理がここに来る
        }
    }
}
