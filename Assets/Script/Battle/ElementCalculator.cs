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

        return 1.0f;
    }
}
