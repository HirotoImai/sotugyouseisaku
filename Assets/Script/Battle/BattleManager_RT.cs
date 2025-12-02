using UnityEngine;

public class BattleManager_RT : MonoBehaviour
{
    public static BattleManager_RT Instance;

    public CardInstance[] allCards;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // PlayerManager を起動して手札にカードを渡す
        var player = FindObjectOfType<PlayerManager_RT>();
        if (player != null)
        {
            player.DrawHand(allCards);
        }
    }
}
