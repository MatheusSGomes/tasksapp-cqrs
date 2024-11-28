using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entity;
using Domain.Enum;
using Infra.Repository.UnitOfWork;
using MediatR;

namespace Application.UserCQ.Handlers;

// IRequestHandler<TipoRequisição, TipoRetorno>
// Método Handle retorna: Task<TipoRetorno>
public class CreateUserCommandHandler(UnitOfWork unitOfWork, IMapper mapper, IAuthService authService) : IRequestHandler<CreateUserCommand, ResponseBase<RefreshTokenViewModel>>
{
    private readonly UnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IAuthService _authService = authService;

    public async Task<ResponseBase<RefreshTokenViewModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var isUniqueEmailAndUsername = _authService.UniqueEmailAndUsername(request.Email!, request.Username!);

        if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.EmailUnavailable)
            return new ResponseBase<RefreshTokenViewModel>
            {
                ResponseInfo = new()
                {
                    Title = "Email indisponível.",
                    ErrorDescription = "O email apresentado já está sendo utilizado. Tente outro.",
                    HttpStatus = 400
                },
                Value = null,
            };

        if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.UsernameUnavailable)
            return new ResponseBase<RefreshTokenViewModel>
            {
                ResponseInfo = new()
                {
                    Title = "Username indisponível.",
                    ErrorDescription = "O username apresentado já está sendo utilizado. Tente outro.",
                    HttpStatus = 400
                },
                Value = null,
            };

        if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.UsernameAndEmailUnavailable)
            return new ResponseBase<RefreshTokenViewModel>
            {
                ResponseInfo = new()
                {
                    Title = "Username e email indisponíveis.",
                    ErrorDescription = "O username e email apresentado já estão sendo utilizados. Tente outro.",
                    HttpStatus = 400
                },
                Value = null,
            };

        var user = _mapper.Map<User>(request); // Passo para o auto mapper: CreateUserCommand -> recebo -> User
        user.RefreshToken = _authService.GenerateRefreshToken();
        user.PasswordHash = _authService.HashingPassword(request.Password!);

        await _unitOfWork.UserRepository.Create(user);
        _unitOfWork.Commit();

        // Passa a ser usado com o UnitOfWork
        // await _repository.Create(user);

        // SaveChanges passa a ser usado com o UnitOfWork
        // await _context.Users.Add(users);
        // await _context.SaveChangesAsync();

        var refreshTokenViewModel = _mapper.Map<RefreshTokenViewModel>(user); // Passo User -> recebo -> UserInfoViewModel
        refreshTokenViewModel.TokenJWT = _authService.GenerateJWT(user.Email!, user.Username!);

        return new ResponseBase<RefreshTokenViewModel>
        {
            ResponseInfo = null,
            Value = refreshTokenViewModel
        };
    }
}
