using UnityEngine;

public class PlayerManager_RT : MonoBehaviour
{
    [Header("UI")]
    public CardButtonView[] handSlots;   // 手札表示用スロット

    private CardInstance[] currentHand;

    private void Start()
    {
        // BattleManager からカードを受け取る
        CardInstance[] allCards = FindObjectOfType<BattleManager_RT>().allCards;

        DrawHand(allCards);
    }

    /// <summary>
    /// BattleManager から渡されたカードを元に手札を作る
    /// </summary>
    public void DrawHand(CardInstance[] allCards)
    {
        if (allCards == null || allCards.Length == 0)
        {
            Debug.LogWarning(" PlayerManager_RT: カードがロードされていません。");
            return;
        }

        int count = Mathf.Min(handSlots.Length, allCards.Length);
        currentHand = new CardInstance[count];

        for (int i = 0; i < count; i++)
        {
            currentHand[i] = allCards[i];  // ランダムにしたければシャッフル処理へ変更
            handSlots[i].SetOwner(currentHand[i]);
        }
    }
}