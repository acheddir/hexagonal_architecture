namespace Me.Acheddir.Hexagonal.Domain.Account;

public sealed class Money : ValueObject<Money>
{
    public static readonly Money Zero = Of(0);

    private readonly BigInteger _amount;

    private Money(long? amount)
    {
        if (!amount.HasValue)
            throw new ArgumentNullException(nameof(amount));

        _amount = BigInteger.CreateChecked(amount.Value);
    }

    private Money(BigInteger? amount)
    {
        if (amount is null)
            throw new ArgumentNullException(nameof(amount));

        _amount = amount.Value;
    }

    public long Amount => (long)_amount;
    protected override bool EqualsInternal(Money other) => _amount == other._amount;
    protected override int GetHashCodeInternal() => HashCode.Combine(_amount) ^ 365;
    public override string ToString() => _amount.ToString();
    public static Money Of(long? value) => new(value);
    public bool IsPositiveOrZero() => BigInteger.IsPositive(_amount) || _amount.IsZero;
    public bool IsNegative() => BigInteger.IsNegative(_amount);
    public bool IsPositive() => BigInteger.IsPositive(_amount) && !_amount.IsZero;
    public bool IsGreaterThanOrEqual(Money comparedMoney) => _amount.CompareTo(comparedMoney._amount) >= 0;
    public bool IsGreaterThan(Money comparedMoney) => _amount.CompareTo(comparedMoney._amount) >= 1;
    public Money Plus(Money additionalMoney) => new(BigInteger.Add(_amount, additionalMoney._amount));
    public Money Minus(Money moneyToSubtract) => new(BigInteger.Subtract(_amount, moneyToSubtract._amount));
    public static Money Add(Money left, Money right) => new(BigInteger.Add(left._amount, right._amount));
    public static Money Subtract(Money left, Money right) => new(BigInteger.Subtract(left._amount, right._amount));
    public Money Negate() => new(BigInteger.Negate(_amount));
}
