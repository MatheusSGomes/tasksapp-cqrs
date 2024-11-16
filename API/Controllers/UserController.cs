using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IMediator mediator) : ControllerBase
{
    public readonly IMediator _mediator = mediator;

    [HttpPost("Create-User")]
    public async Task<ActionResult<UserInfoViewModel>> CreateUser(CreateUserCommand command)
    {
        // Enviar o command para o Mediator.
        // Mediator fará o command chegar no CreateUserCommandHandler.
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
