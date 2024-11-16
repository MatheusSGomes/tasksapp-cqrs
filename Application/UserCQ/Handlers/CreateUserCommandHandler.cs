using Application.Response;
using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using Domain.Entity;
using Infra.Persistence;
using MediatR;

namespace Application.UserCQ.Handlers;

// IRequestHandler<TipoRequisição, TipoRetorno>
// Método Handle retorna: Task<TipoRetorno>
public class CreateUserCommandHandler(TasksDbContext context) : IRequestHandler<CreateUserCommand, ResponseBase<UserInfoViewModel?>>
{
    private readonly TasksDbContext _context = context;

    public async Task<ResponseBase<UserInfoViewModel?>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            PasswordHash = request.Password,
            Username = request.Username,
            RefreshToken = Guid.NewGuid().ToString(),
            RefreshTokenExpirationTime = DateTime.Now.AddDays(5)
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var userInfo = new UserInfoViewModel
        {
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            Username = user.Username,
            RefreshToken = user.RefreshToken,
            RefreshTokenExpirationTime = user.RefreshTokenExpirationTime,
            TokenJWT = Guid.NewGuid().ToString()
        };

        return new ResponseBase<UserInfoViewModel?>
        {
            ResponseInfo = null,
            Value = userInfo
        };
    }
}
