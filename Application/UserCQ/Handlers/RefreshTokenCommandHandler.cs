using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Abstractions;
using Infra.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.UserCQ.Handlers;

// IRequestHandler<TipoDeEntrada, TipoDeRetorno>
public class RefreshTokenCommandHandler(TasksDbContext context, IAuthService authService, IConfiguration configuration, IMapper mapper) : IRequestHandler<RefreshTokenCommand, ResponseBase<RefreshTokenViewModel>>
{
    private readonly TasksDbContext _context = context;
    private readonly IAuthService _authService = authService;
    private readonly IConfiguration _configuration = configuration;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseBase<RefreshTokenViewModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username);

        if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpirationTime < DateTime.Now)
        {
            return new ResponseBase<RefreshTokenViewModel>()
            {
                ResponseInfo = new()
                {
                    Title = "Token inválido",
                    ErrorDescription = "Refresh token inválido ou expirado. Faça login novamente",
                    HttpStatus = 400
                },
                Value = null
            };
        }

        _ = int.TryParse(_configuration["JWT:RefreshTokenExpirationTimeInDays"],
            out int refreshTokenExpirationTimeInDays);

        // Gerar novo refresh token
        user.RefreshToken = _authService.GenerateRefreshToken();
        user.RefreshTokenExpirationTime = DateTime.Now.AddDays(refreshTokenExpirationTimeInDays);

        await _context.SaveChangesAsync();

        RefreshTokenViewModel refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
        refreshTokenVM.TokenJWT = _authService.GenerateJWT(user.Email!, user.Username!);

        return new ResponseBase<RefreshTokenViewModel>()
        {
            ResponseInfo = null,
            Value = refreshTokenVM
        };

    }
}
