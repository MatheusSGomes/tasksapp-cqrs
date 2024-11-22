using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Classe que contém os métodos action da entidade User
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("[controller]")]
public class AuthController(IMediator mediator, IConfiguration configuration) : ControllerBase
{
    public readonly IMediator _mediator = mediator;
    private readonly IConfiguration _configuration = configuration;

    /// <summary>
    /// Rota responsável pela criação de um usuário
    /// </summary>
    /// <param name="command">Objeto CreateUserCommand</param>
    /// <returns>Dados do usuário criado</returns>
    /// <remarks>
    /// Exemplo de request:
    /// ```
    /// POST /Auth/Create-User
    /// {
    ///     "name": "John",
    ///     "surname": "Doe",
    ///     "username": "JohnDoe",
    ///     "email": "JohnDoe@email.com",
    ///     "password": "12345678"
    /// }
    /// ```
    /// </remarks>
    /// <response code="200">Retorna dados de um novo usuário</response>
    /// <response code="400">Se algum dado for digitado incorretamente</response>
    [HttpPost("Create-User")]
    public async Task<ActionResult<UserInfoViewModel>> CreateUser(CreateUserCommand command)
    {
        // Enviar o command para o Mediator.
        // Mediator fará o command chegar no CreateUserCommandHandler.
        var result = await _mediator.Send(command);

        if (result.ResponseInfo is null)
        {
            var userInfo = result.Value;

            if (userInfo is not null)
            {
                var cookieOptionsToken = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                };

                _ = int.TryParse(_configuration["JWT:RefreshTokenExpirationTimeInDays"], out int refreshTokenExpirationTimeInDays);

                var cookieOptionsRefreshToken = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(refreshTokenExpirationTimeInDays)
                };

                // Adicionar cookie na response de retorno
                Response.Cookies.Append("jwt", result.Value!.TokenJWT!, cookieOptionsToken);
                Response.Cookies.Append("refreshToken", result.Value!.RefreshToken!, cookieOptionsRefreshToken);

                return Ok(result);
            }
        }

        return BadRequest(result);
    }
}
