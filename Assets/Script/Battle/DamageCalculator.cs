using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageCalculator
{
    public static float GetMultiplier(CardElement atk, CardElement def)
    {
        if (atk == def) return 1f;

        if (atk == CardElement.Red && def == CardElement.Green) return 1.5f;
        if (atk == CardElement.Green && def == CardElement.Blue) return 1.5f;
        if (atk == CardElement.Blue && def == CardElement.Red) return 1.5f;

        if (atk == CardElement.Black && def == CardElement.White) return 1.5f;
        if (atk == CardElement.White && def == CardElement.Black) return 1.5f;

        return 0.75f;
    }
}
