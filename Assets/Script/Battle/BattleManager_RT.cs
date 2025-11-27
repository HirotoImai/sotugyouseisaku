using UnityEngine;

public class BattleManager_RT : MonoBehaviour
{
    public static BattleManager_RT Instance;
    public PlayerManager_RT player1;
    public PlayerManager_RT player2;
    public Transform fieldPlayer1;
    public Transform fieldPlayer2;
    public GameObject cardViewPrefab;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // カードプール更新
        CardDatabase.Instance.allCards.Clear();
        CardDatabase.Instance.allCards.AddRange(CardDatabase.Instance.fixedCards);
        CardDatabase.Instance.allCards.AddRange(CardSaveManager.loadedCards);

        // プレイヤー初期化（手札生成）
        player1.Initialize(true);
        player2.Initialize(false);
    }

    public void SpawnCard(CardData data, bool isPlayer)
    {
        Transform field = isPlayer ? fieldPlayer1 : fieldPlayer2;
        GameObject cardObj = Instantiate(cardViewPrefab, field);
        CardView view = cardObj.GetComponent<CardView>();
        view.Setup(data, isPlayer);

        // 攻撃処理（簡易版）
        PlayerManager_RT opponent = isPlayer ? player2 : player1;
        opponent.TakeDamage(data.attack);
    }
}
