using Application.Response;
using Application.WorkspaceCQ.Commands;
using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Infra.Repository.UnitOfWork;
using MediatR;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Application.WorkspaceCQ.Handlers;

public class EditWorkspaceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<EditWorkspaceCommand, ResponseBase<WorkspaceViewModel>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseBase<WorkspaceViewModel>> Handle(EditWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var workspace = await _unitOfWork.WorkspaceRepository.GetWorkspaceAndUser(request.Id);

        if (workspace is null)
        {
            return new ResponseBase<WorkspaceViewModel>
            {
                ResponseInfo = new()
                {
                    Title = "Workspace não encontrado",
                    ErrorDescription = "Nenhum workspace encontrado com 'Id' informado",
                    HttpStatus = 404
                },
                Value = null
            };
        }

        if (request.Title != null)
            workspace.Title = request.Title;

        if (request.Status != null)
            workspace.Status = request.Status;

        _unitOfWork.Commit();

        return new ResponseBase<WorkspaceViewModel>
        {
            ResponseInfo = null,
            Value = _mapper.Map<WorkspaceViewModel>(workspace)
        };
    }
}
