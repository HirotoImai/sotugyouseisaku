using UnityEngine;

public static class CardElementUtility
{
    public static CardElement GetElement(Color color)
    {
        if (Approximately(color, Color.red)) return CardElement.Red;
        if (Approximately(color, Color.green)) return CardElement.Green;
        if (Approximately(color, Color.blue)) return CardElement.Blue;
        if (Approximately(color, Color.black)) return CardElement.Black;
        if (Approximately(color, Color.white)) return CardElement.White;

        // 想定外の色は White 扱い（保険）
        return CardElement.White;
    }

    static bool Approximately(Color a, Color b)
    {
        return Mathf.Abs(a.r - b.r) < 0.01f &&
               Mathf.Abs(a.g - b.g) < 0.01f &&
               Mathf.Abs(a.b - b.b) < 0.01f;
    }
}
