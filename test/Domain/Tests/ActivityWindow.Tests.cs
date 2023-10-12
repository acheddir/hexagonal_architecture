using Me.Acheddir.Hexagonal.Domain.Exceptions;

namespace Tests;

public class ActivityWindowTests
{
    [Fact]
    public void GivenAnActivityWindow_ThenStartTimestampShouldBeCorrect()
    {
        var activityWindow = new ActivityWindow(
            DefaultActivity().WithTimestamp(StartDate()).Build(),
            DefaultActivity().WithTimestamp(InBetweenDate()).Build(),
            DefaultActivity().WithTimestamp(EndDate()).Build());

        activityWindow.GetStartTimestamp().Should().Be(StartDate());
    }

    [Fact]
    public void GivenAnActivityWindow_ThenEndTimestampShouldBeCorrect()
    {
        var activityWindow = new ActivityWindow(
            DefaultActivity().WithTimestamp(StartDate()).Build(),
            DefaultActivity().WithTimestamp(InBetweenDate()).Build(),
            DefaultActivity().WithTimestamp(EndDate()).Build());

        activityWindow.GetEndTimestamp().Should().Be(EndDate());
    }
[Fact]
    public void GivenAnActivityWindowWithNoActivities_WhenGettingStartTimestamp_ThenExceptionShouldBeThrown()
    {
        var activityWindow = new ActivityWindow(Array.Empty<Activity>());

        activityWindow.Invoking(aw => aw.GetStartTimestamp())
            .Should().Throw<NoActivityException>()
            .WithMessage("No activity was found.");
    }

    [Fact]
    public void GivenAnActivityWindowWithNoActivities_WhenGettingEndTimestamp_ThenExceptionShouldBeThrown()
    {
        var activityWindow = new ActivityWindow(Array.Empty<Activity>());

        activityWindow.Invoking(aw => aw.GetEndTimestamp())
            .Should().Throw<NoActivityException>()
            .WithMessage("No activity was found.");
    }
    [Fact]
    public void GivenAnActivityWindow_ThenAccountBalanceShouldBeCorrect()
    {
        var sourceAccountId = new AccountId(1);
        var targetAccountId = new AccountId(2);
        
        var activityWindow = new ActivityWindow(
            DefaultActivity()
                .WithSourceAccount(sourceAccountId)
                .WithTargetAccount(targetAccountId)
                .WithMoney(Money.Of(999)).Build(),
            DefaultActivity()
                .WithSourceAccount(sourceAccountId)
                .WithTargetAccount(targetAccountId)
                .WithMoney(Money.Of(1)).Build(),
            DefaultActivity()
                .WithSourceAccount(targetAccountId)
                .WithTargetAccount(sourceAccountId)
                .WithMoney(Money.Of(500)).Build());

        activityWindow.CalculateBalance(sourceAccountId).Should().Be(Money.Of(-500));
        activityWindow.CalculateBalance(targetAccountId).Should().Be(Money.Of(500));
    }

    private DateTime StartDate()
    {
        return new DateTime(2023, 10, 3);
    }

    private DateTime InBetweenDate()
    {
        return new DateTime(2023, 10, 4);
    }

    private DateTime EndDate()
    {
        return new DateTime(2023, 10, 5);
    }
}