namespace Tests;

public class MoneyTests
{
    [Fact]
    public void GivenNullAmountOfMoney_ShouldThrowException()
    {
        var action = () => Money.Of(null);

        action.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void GivenAmountOfMoney_WhenIsZero_ThenIsPositiveOrZeroShouldBeTrue()
    {
        var money = Money.Of(0);

        money.IsPositiveOrZero().Should().Be(true);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenIsPositive_ThenIsPositiveOrZeroShouldBeTrue()
    {
        var money = Money.Of(1000);

        money.IsPositiveOrZero().Should().Be(true);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenIsNegative_ThenIsPositiveOrZeroShouldBeFalse()
    {
        var money = Money.Of(-10);

        money.IsPositiveOrZero().Should().Be(false);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenIsZero_ThenIsNegativeShouldBeFalse()
    {
        var money = Money.Of(0);

        money.IsNegative().Should().Be(false);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenIsPositive_ThenIsNegativeShouldBeFalse()
    {
        var money = Money.Of(1000);

        money.IsNegative().Should().Be(false);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenIsNegative_ThenIsNegativeShouldBeTrue()
    {
        var money = Money.Of(-10);

        money.IsNegative().Should().Be(true);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenIsZero_ThenIsPositiveShouldBeFalse()
    {
        var money = Money.Of(0);

        money.IsPositive().Should().Be(false);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenIsPositive_ThenIsPositiveShouldBeTrue()
    {
        var money = Money.Of(1000);

        money.IsPositive().Should().Be(true);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenIsNegative_ThenIsPositiveShouldBeFalse()
    {
        var money = Money.Of(-10);

        money.IsPositive().Should().Be(false);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenComparedAmountIsLower_ThenIsGreaterThanOrEqualShouldBeTrue()
    {
        var money = Money.Of(1000);
        var comparedMoney = Money.Of(10);

        money.IsGreaterThanOrEqual(comparedMoney).Should().Be(true);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenComparedAmountIsEqual_ThenIsGreaterThanOrEqualShouldBeTrue()
    {
        var money = Money.Of(1000);
        var comparedMoney = Money.Of(1000);

        money.IsGreaterThanOrEqual(comparedMoney).Should().Be(true);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenComparedAmountIsGreater_ThenIsGreaterThanOrEqualShouldBeFalse()
    {
        var money = Money.Of(10);
        var comparedMoney = Money.Of(1000);

        money.IsGreaterThanOrEqual(comparedMoney).Should().Be(false);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenComparedAmountIsLower_ThenIsGreaterThanShouldBeTrue()
    {
        var money = Money.Of(1000);
        var comparedMoney = Money.Of(10);

        money.IsGreaterThan(comparedMoney).Should().Be(true);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenComparedAmountIsEqual_ThenIsGreaterThanShouldBeFalse()
    {
        var money = Money.Of(1000);
        var comparedMoney = Money.Of(1000);

        money.IsGreaterThan(comparedMoney).Should().Be(false);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenComparedAmountIsGreater_ThenIsGreaterThanShouldBeFalse()
    {
        var money = Money.Of(10);
        var comparedMoney = Money.Of(1000);

        money.IsGreaterThan(comparedMoney).Should().Be(false);
    }

    [Fact]
    public void GivenAmountOfMoney_WhenAddingAdditionalAmount_ThenTotalAmountShouldBeCorrect()
    {
        var money = Money.Of(1000);
        var additionalMoney = Money.Of(10);

        var totalMoney = money.Plus(additionalMoney);

        totalMoney.Should().BeEquivalentTo(Money.Of(1010));
    }

    [Fact]
    public void GivenAmountOfMoney_WhenSubtractingAmount_ThenRemainingAmountShouldBeCorrect()
    {
        var money = Money.Of(1000);
        var additionalMoney = Money.Of(10);

        var totalMoney = money.Minus(additionalMoney);

        totalMoney.Should().BeEquivalentTo(Money.Of(990));
    }

    [Fact]
    public void GivenTwoAmountsOfMoney_WhenSummingThemUp_ThenTotalShouldBeCorrect()
    {
        var money1 = Money.Of(1000);
        var money2 = Money.Of(10);

        var sum = Money.Add(money1, money2);

        sum.Should().BeEquivalentTo(Money.Of(1010));
    }

    [Fact]
    public void GivenTwoAmountsOfMoney_WhenSubtractingOneFromTheOther_ThenRemainingShouldBeCorrect()
    {
        var money1 = Money.Of(1000);
        var money2 = Money.Of(10);

        var sum = Money.Subtract(money1, money2);

        sum.Should().BeEquivalentTo(Money.Of(990));
    }

    [Fact]
    public void GivenAmountOfMoney_WhenNegatingValueOfTheAmount_ThenResultShouldHaveTheCorrectSign()
    {
        var money = Money.Of(1000);

        var negation = money.Negate();

        negation.Should().BeEquivalentTo(Money.Of(-1000));
    }
}