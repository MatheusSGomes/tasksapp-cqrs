using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using AutoMapper;
using Domain.Abstractions;
using Domain.Entity;
using Infra.Persistence;
using MediatR;

namespace Application.UserCQ.Handlers;

// IRequestHandler<TipoRequisição, TipoRetorno>
// Método Handle retorna: Task<TipoRetorno>
public class CreateUserCommandHandler(TasksDbContext context, IMapper mapper, IAuthService authService) : IRequestHandler<CreateUserCommand, ResponseBase<UserInfoViewModel?>>
{
    private readonly TasksDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IAuthService _authService = authService;

    public async Task<ResponseBase<UserInfoViewModel?>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request); // Passo para o auto mapper: CreateUserCommand -> recebo -> User

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var userInfoVM = _mapper.Map<UserInfoViewModel>(user); // Passo User -> recebo -> UserInfoViewModel
        userInfoVM.TokenJWT = _authService.GenerateJWT(user.Email!, user.Username!);

        return new ResponseBase<UserInfoViewModel?>
        {
            ResponseInfo = null,
            Value = userInfoVM
        };
    }
}
