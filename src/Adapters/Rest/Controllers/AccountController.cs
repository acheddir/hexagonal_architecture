using Me.Acheddir.Hexagonal.Application.UseCases.GetAccountBalance;
using Me.Acheddir.Hexagonal.Application.UseCases.SendMoney;
using Me.Acheddir.Hexagonal.Domain.Account;
using Me.Acheddir.Hexagonal.Rest.Controllers.Base;
using MediatR;

namespace Me.Acheddir.Hexagonal.Rest.Controllers;

[Route("api/accounts")]
public class AccountController : ApiControllerBase
{
    public AccountController (ISender sender) : base(sender)
    {
    }

    [HttpGet("{sourceAccountId}/balance")]
    public async Task<IActionResult> GetAccountBalance([FromRoute] long sourceAccountId)
    {
        var query = new GetAccountBalanceQuery { AccountId = new AccountId(sourceAccountId)};
        var result = await Sender.Send(query);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpPost("{sourceAccountId}/send")]
    public async Task<IActionResult> SendMoney(
        [FromRoute] long sourceAccountId,
        [FromQuery] long targetAccountId,
        [FromQuery] long amount)
    {
        var command = new SendMoneyCommand
        {
            SourceAccountId = new AccountId(sourceAccountId),
            TargetAccountId = new AccountId(targetAccountId),
            Amount = Money.Of(amount)
        };

        var result = await Sender.Send(command);

        return result.IsSuccess ? Ok() : BadRequest(result.Errors);
    }
}