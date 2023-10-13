namespace Me.Acheddir.Hexagonal.Rest.Controllers;

[Route("api/accounts")]
public class AccountController : ApiControllerBase
{
    public AccountController(ISender sender) : base(sender) { }

    [HttpGet("{sourceAccountId}/balance")]
    public async Task<IActionResult> GetAccountBalance([FromRoute] long sourceAccountId, [FromQuery] DateTime baselineDate)
    {
        var query = new GetAccountBalanceQuery(new AccountId(sourceAccountId), baselineDate);
        var result = await Sender.Send(query);

        return result.IsSuccess
            ? Ok(new { balance = result.Value.Amount })
            : BadRequest(result.Errors);
    }

    [HttpPost("{sourceAccountId}/send")]
    public async Task<IActionResult> SendMoney([FromRoute] long sourceAccountId, [FromQuery] long targetAccountId,
        [FromQuery] long amount)
    {
        var command = new SendMoneyCommand(
            new AccountId(sourceAccountId),
            new AccountId(targetAccountId),
            Money.Of(amount));

        var result = await Sender.Send(command);

        return result.IsSuccess ? Ok() : BadRequest(result.Errors);
    }
}