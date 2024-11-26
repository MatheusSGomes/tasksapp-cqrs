using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Abstractions;
using Infra.Repository.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UserCQ.Handlers;

// IRequestHandler<Entrada, TipoDeResposta>
public class LoginUserCommandHandler(IUnitOfWork unitOfWork, IAuthService authService, IConfiguration configuration, IMapper mapper) : IRequestHandler<LoginUserCommand, ResponseBase<RefreshTokenViewModel>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IAuthService _authService = authService;
    private readonly IConfiguration _configuration = configuration;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseBase<RefreshTokenViewModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // Antes do UnitOfWork
        // var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        var user = await _unitOfWork.UserRepository.Get(x => x.Email == request.Email);

        if (user is null)
        {
            return new ResponseBase<RefreshTokenViewModel>()
            {
                ResponseInfo = new()
                {
                    Title = "Usuário não encontrado",
                    ErrorDescription = "Nenhum usuário encontrado com o email informado",
                    HttpStatus = 404
                },
                Value = null
            };
        }

        var hashPasswordRequest = _authService.HashingPassword(request.Password!);

        if (hashPasswordRequest != user.PasswordHash)
        {
            return new ResponseBase<RefreshTokenViewModel>
            {
                ResponseInfo = new()
                {
                    Title = "Senha incorreta",
                    ErrorDescription = "Senha informada está incorreta",
                    HttpStatus = 404
                },
                Value = null
            };
        }

        _ = int.TryParse(_configuration["JWT:RefreshTokenExpirationTimeInDays"], out int refreshTokenValidityInDays);

        user.RefreshToken = _authService.GenerateRefreshToken();
        user.RefreshTokenExpirationTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

        // Antes do UnitOfWork
        // _context.Update(user);
        // await _context.SaveChangesAsync();
        await _unitOfWork.UserRepository.Update(user); // Salva em memória
        _unitOfWork.Commit(); // Salva no banco de dados

        RefreshTokenViewModel refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
        refreshTokenVM.TokenJWT = _authService.GenerateJWT(user.Email!, user.Username!);

        return new ResponseBase<RefreshTokenViewModel>
        {
            ResponseInfo = null,
            Value = refreshTokenVM
        };
    }
}
