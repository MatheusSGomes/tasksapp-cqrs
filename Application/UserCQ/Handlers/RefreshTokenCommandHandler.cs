using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Abstractions;
using Infra.Repository.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UserCQ.Handlers;

// IRequestHandler<TipoDeEntrada, TipoDeRetorno>
public class RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IAuthService authService, IConfiguration configuration, IMapper mapper) : IRequestHandler<RefreshTokenCommand, ResponseBase<RefreshTokenViewModel>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAuthService _authService = authService;
    private readonly IConfiguration _configuration = configuration;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseBase<RefreshTokenViewModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // Antes do UnitOfWork
        // var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username);
        var user = await _unitOfWork.UserRepository.Get(x => x.Username == request.Username);

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

        // Antes do UnitOfWork
        // _context.Update(user);
        // await _context.SaveChangesAsync();
        await _unitOfWork.UserRepository.Update(user);
        _unitOfWork.Commit();

        RefreshTokenViewModel refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
        refreshTokenVM.TokenJWT = _authService.GenerateJWT(user.Email!, user.Username!);

        return new ResponseBase<RefreshTokenViewModel>()
        {
            ResponseInfo = null,
            Value = refreshTokenVM
        };

    }
}
