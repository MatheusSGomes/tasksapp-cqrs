using Application.Response;
using Application.WorkspaceCQ.Queries;
using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Infra.Repository.UnitOfWork;
using MediatR;

namespace Application.WorkspaceCQ.Handlers;

public class GetWorkspaceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetWorkspaceQuery, ResponseBase<WorkspaceViewModel>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseBase<WorkspaceViewModel>> Handle(GetWorkspaceQuery request, CancellationToken cancellationToken)
    {
        var workspace = await _unitOfWork.WorkspaceRepository.GetWorkspaceAndUser(request.Id);

        if (workspace is null)
            return new ResponseBase<WorkspaceViewModel>
            {
                ResponseInfo = new ResponseInfo
                {
                    Title = "Workspace não encontrado",
                    ErrorDescription = "Nenhum workspace encontrado com o 'id' informado",
                    HttpStatus = 404
                },
                Value = null
            };

        return new ResponseBase<WorkspaceViewModel>
        {
            ResponseInfo = null,
            Value = _mapper.Map<WorkspaceViewModel>(workspace)
        };
    }
}
