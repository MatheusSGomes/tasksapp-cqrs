using Application.Response;
using Application.WorkspaceCQ.Commands;
using Infra.Repository.UnitOfWork;
using MediatR;

namespace Application.WorkspaceCQ.Handlers;

public class DeleteWorkspaceCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteWorkspaceCommand, ResponseBase<DeleteWorkspaceCommand>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ResponseBase<DeleteWorkspaceCommand>> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var workspace = await _unitOfWork.WorkspaceRepository.Get(x => x.Id == request.Id);

        if (workspace is null)
            return new ResponseBase<DeleteWorkspaceCommand>
            {
                ResponseInfo = new ResponseInfo
                {
                    Title = "Workspace não encontrado",
                    ErrorDescription = "Nenhum workspace encontrado com o 'id' informado",
                    HttpStatus = 404
                },
                Value = null
            };

        await _unitOfWork.WorkspaceRepository.Delete(workspace);
        _unitOfWork.Commit();

        return new ResponseBase<DeleteWorkspaceCommand>
        {
            ResponseInfo = null,
            Value = request
        };
    }
}
