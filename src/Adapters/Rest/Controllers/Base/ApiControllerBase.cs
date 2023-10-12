using MediatR;

namespace Me.Acheddir.Hexagonal.Rest.Controllers.Base;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    protected readonly ISender Sender;

    public ApiControllerBase(ISender sender)
    {
        Sender = sender;
    }
}