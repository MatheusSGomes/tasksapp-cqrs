using Application.UserCQ.Commands;
using Application.UserCQ.ViewModels;
using MediatR;

namespace Application.UserCQ.Handlers;

// IRequestHandler<TipoRequisição, TipoRetorno>
// Método Handle retorna: Task<TipoRetorno>
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserInfoViewModel>
{
    public Task<UserInfoViewModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
