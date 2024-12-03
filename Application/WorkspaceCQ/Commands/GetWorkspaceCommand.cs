using Application.Response;
using Application.WorkspaceCQ.ViewModels;
using MediatR;

namespace Application.WorkspaceCQ.Commands;

// IRequest<TipoDeRetorno> - Deve ser o mesmo retornado na segunda tipagem do IRequestHandler, exemplo:
// IRequestHandler<GetWorkspaceCommand, TipoDeRetorno>

// Essa tipagem será usada na controller, exemplo:
// ResponseBase<WorkspaceViewModel> result = await _mediator.Send(new GetWorkspaceCommand { Id = workspaceId});
public record GetWorkspaceCommand : IRequest<ResponseBase<WorkspaceViewModel>>
{
    public Guid Id { get; set; }
}
