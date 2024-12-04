using Application.Response;
using Application.WorkspaceCQ.ViewModels;
using MediatR;

namespace Application.WorkspaceCQ.Queries;

// IRequest<TipoDeRetorno> - Deve ser o mesmo retornado na segunda tipagem do IRequestHandler, exemplo:
// IRequestHandler<GetWorkspaceQuery, TipoDeRetorno>

// Essa tipagem será usada na controller, exemplo:
// ResponseBase<WorkspaceViewModel> result = await _mediator.Send(new GetWorkspaceQuery { Id = workspaceId});
public record GetWorkspaceQuery : IRequest<ResponseBase<WorkspaceViewModel>>
{
    public Guid Id { get; set; }
}
