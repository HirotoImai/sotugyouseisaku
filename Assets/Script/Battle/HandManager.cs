//using System.Collections.Generic;
//using UnityEngine;

//public class HandManager : MonoBehaviour
//{
//    public GameObject cardButtonPrefab;
//    public Transform handArea;
//    public int handSize = 3; // 手札枚数

//    void Start()
//    {
//        DrawRandomHand();
//    }

//    void DrawRandomHand()
//    {
//        var allCards = CardPersistenceService.loadedCards;
//        if (allCards == null || allCards.Count == 0)
//        {
//            Debug.LogWarning("カードがロードされていません。");
//            return;
//        }

//        List<CardInstance> handCards = new List<CardInstance>();
//        List<int> usedIndexes = new List<int>();

//        for (int i = 0; i < handSize && i < allCards.Count; i++)
//        {
//            int index;
//            do { index = Random.Range(0, allCards.Count); }
//            while (usedIndexes.Contains(index));

//            usedIndexes.Add(index);
//            handCards.Add(new CardInstance(allCards[index]));
//        }

//        foreach (CardInstance data in handCards)
//        {
//            GameObject cardObj = Instantiate(cardButtonPrefab, handArea);
//            var view = cardObj.GetComponent<CardButtonView>();
//            view.Setup(data);
//        }

//        Debug.Log($"手札に{handCards.Count}枚を追加しました");
//    }
//}
