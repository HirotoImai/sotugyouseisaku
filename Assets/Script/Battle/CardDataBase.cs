using UnityEngine;
using System.Linq;

public class CardDataBase : MonoBehaviour
{
    public static CardDataBase Instance;

    private void Awake()
    {
        Instance = this;
    }

    // CardInstance[] ‚©‚ç CardData[] ‚ð•Ô‚·
    public CardData[] ToCardDataList(CardInstance[] instances)
    {
        if (instances == null || instances.Length == 0)
            return new CardData[0];

        return instances.Select(ci => ci.template).ToArray();
    }
}
