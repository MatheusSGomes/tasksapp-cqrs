using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Classe que contém os métodos action da entidade User
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("[controller]")]
public class AuthController(IMediator mediator, IConfiguration configuration, IMapper mapper) : ControllerBase
{
    public readonly IMediator _mediator = mediator;
    private readonly IConfiguration _configuration = configuration;
    private readonly IMapper _mapper = mapper;

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
    [HttpPost("CreateUser")]
    public async Task<ActionResult<ResponseBase<UserInfoViewModel>>> CreateUser(CreateUserCommand command)
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

                var res = result.Value;
                return Ok(_mapper.Map<UserInfoViewModel>(res));
            }
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Rota responsável pelo login do usuário
    /// </summary>
    /// <param name="command">Objeto LoginUserCommand</param>
    /// <returns>Dados do usuário logado</returns>
    /// <remarks>
    /// Exemplo de request:
    /// ```
    /// POST /Auth/Create-User
    /// {
    ///     "email": "JohnDoe@email.com",
    ///     "password": "12345678"
    /// }
    /// ```
    /// </remarks>
    /// <response code="200">Retorna dados de um novo usuário</response>
    /// <response code="400">Se algum dado for digitado incorretamente</response>
    [HttpPost("Login")]
    public async Task<ActionResult<ResponseBase<UserInfoViewModel>>> Login(LoginUserCommand command)
    {
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

                var res = result.Value;
                return Ok(_mapper.Map<UserInfoViewModel>(res));
            }
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Rota responsável pelo refresh token do login
    /// </summary>
    /// <param name="command">Objeto LoginUserCommand</param>
    /// <returns>Dados do usuário logado</returns>
    /// <remarks>
    /// Exemplo de request:
    /// ```
    /// POST /Auth/Create-User
    /// {
    ///     "email": "JohnDoe@email.com",
    ///     "password": "12345678"
    /// }
    /// ```
    /// </remarks>
    /// <response code="200">Retorna dados de um novo usuário</response>
    /// <response code="400">Se algum dado for digitado incorretamente</response>
    [HttpPost("RefreshToken")]
    public async Task<ActionResult<ResponseBase<UserInfoViewModel>>> RefreshToken(RefreshTokenCommand command)
    {
        var request = await _mediator.Send(new RefreshTokenCommand(
            Username: command.Username,
            RefreshToken: Request.Cookies["refreshToken"]
        ));

        if (request.ResponseInfo is null)
        {
            var userInfo = request.Value;

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
                Response.Cookies.Append("jwt", request.Value!.TokenJWT!, cookieOptionsToken);
                Response.Cookies.Append("refreshToken", request.Value!.RefreshToken!, cookieOptionsRefreshToken);

                var res = request.Value;
                return Ok(_mapper.Map<UserInfoViewModel>(res));
            }
        }

        return BadRequest(request);
    }
}
