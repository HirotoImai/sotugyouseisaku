//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections.Generic;
//using TMPro;

//public class BattleManager : MonoBehaviour
//{
//    public Player player1 = new Player();
//    public Player player2 = new Player();

//    [Header("Deck")]
//    public DeckData player1DeckData;
//    public DeckData player2DeckData;

//    [Header("カード生成用")]
//    public GameObject cardPrefab;
//    public Transform player1HandArea;
//    public Transform player2HandArea;

//    [Header("UI")]
//    public TMP_Text player1LifeText;
//    public TMP_Text player2LifeText;
//    public TMP_Text turnText;

//    private int turn = 1;

//    void Start()
//    {
//        // DeckData からコピーして Player.deck に設定
//        player1.deck = new System.Collections.Generic.List<CardData>(player1DeckData.cards);
//        player2.deck = new System.Collections.Generic.List<CardData>(player2DeckData.cards);

//        DrawCard(player1, player1HandArea);
//        DrawCard(player2, player2HandArea);

//        UpdateUI();
//    }

//    void UpdateUI()
//    {
//        player1LifeText.text = "Player1 Life: " + player1.life;
//        player2LifeText.text = "Player2 Life: " + player2.life;
//        turnText.text = "Turn: " + turn;
//    }

//    public void DrawCard(Player player, Transform handArea)
//    {
//        if (player.deck.Count == 0) return;

//        CardData cardData = player.deck[0];
//        player.deck.RemoveAt(0);
//        player.hand.Add(cardData);

//        GameObject cardObj = Instantiate(cardPrefab, handArea);
//        cardObj.GetComponent<Card>().Init(cardData);

//        cardObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => PlayCard(player, cardData, cardObj));
//    }

//    public void PlayCard(Player player, CardData cardData, GameObject cardObj)
//    {
//        if (player.currentCost < cardData.cost)
//        {
//            Debug.Log("コスト不足でプレイできません");
//            return;
//        }

//        player.currentCost -= cardData.cost;
//        player.hand.Remove(cardData);
//        Destroy(cardObj);

//        if (cardData.kind == CardKind.Creature)
//        {
//            Player opponent = (player == player1) ? player2 : player1;
//            opponent.life -= cardData.power;
//            Debug.Log(player + " が " + cardData.power + " ダメージ");
//        }

//        CheckWin();
//        UpdateUI();
//    }

//    void CheckWin()
//    {
//        if (player1.life <= 0)
//        {
//            Debug.Log("Player2 Wins!");
//        }
//        else if (player2.life <= 0)
//        {
//            Debug.Log("Player1 Wins!");
//        }
//    }

//    public void NextTurn()
//    {
//        turn++;
//        player1.maxCost++;
//        player2.maxCost++;
//        player1.currentCost = player1.maxCost;
//        player2.currentCost = player2.maxCost;

//        DrawCard(player1, player1HandArea);
//        DrawCard(player2, player2HandArea);

//        UpdateUI();
//    }
//}
