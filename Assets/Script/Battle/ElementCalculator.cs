public static class ElementCalculator
{
    public static float GetDamageMultiplier(CardElement attacker, CardElement defender)
    {
        if (attacker == CardElement.Red && defender == CardElement.Green) return 1.5f;
        if (attacker == CardElement.Green && defender == CardElement.Blue) return 1.5f;
        if (attacker == CardElement.Blue && defender == CardElement.Red) return 1.5f;
        if (attacker == CardElement.Green && defender == CardElement.Red) return 0.75f;
        if (attacker == CardElement.Blue && defender == CardElement.Green) return 0.75f;
        if (attacker == CardElement.Red && defender == CardElement.Blue) return 0.75f;
        if (attacker == CardElement.Black && defender == CardElement.White) return 1.5f;
        if (attacker == CardElement.White && defender == CardElement.Black) return 1.5f;
        return 1.0f;
    }
}