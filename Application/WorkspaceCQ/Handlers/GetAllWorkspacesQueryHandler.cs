using Application.Response;
using Application.Utils;
using Application.WorkspaceCQ.Queries;
using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Infra.Repository.UnitOfWork;
using MediatR;

namespace Application.WorkspaceCQ.Handlers;

public class GetAllWorkspacesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllWorkspacesQuery, ResponseBase<PaginatedList<WorkspaceViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseBase<PaginatedList<WorkspaceViewModel>>> Handle(GetAllWorkspacesQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.Get(x => x.Id == request.UserId);

        if (user is null)
            return new ResponseBase<PaginatedList<WorkspaceViewModel>>
            {
                ResponseInfo = new ResponseInfo
                {
                    Title = "Usuário não encontrado",
                    ErrorDescription = "Nenhum usuário encontrado com o 'id' informado",
                    HttpStatus = 404
                },
                Value = null
            };

        var workspaces = await _unitOfWork.WorkspaceRepository.GetAllWorkspacesAndUser(user.Id);

        return new ResponseBase<PaginatedList<WorkspaceViewModel>>
        {
            ResponseInfo = null,
            Value = new PaginatedList<WorkspaceViewModel>(
                items: _mapper.Map<List<WorkspaceViewModel>>(workspaces),
                page: request.PageIndex,
                pageSize: request.PageSize)
        };
    }
}
