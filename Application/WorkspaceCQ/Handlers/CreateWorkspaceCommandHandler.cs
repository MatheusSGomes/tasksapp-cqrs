using Application.Response;
using Application.WorkspaceCQ.Commands;
using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Domain.Entity;
using Infra.Repository.UnitOfWork;
using MediatR;

namespace Application.WorkspaceCQ.Handlers;

public class CreateWorkspaceHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateWorkspaceCommand, ResponseBase<CreateWorkspaceViewModel>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseBase<CreateWorkspaceViewModel>> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.Get(x => x.Id == request.UserId);

        if (user is null)
        {
            return new ResponseBase<CreateWorkspaceViewModel>
            {
                ResponseInfo = new ResponseInfo
                {
                    ErrorDescription = "Nenhum usuário encontrado com Id informado",
                    HttpStatus = 400,
                    Title = "Usuário não encontrado"
                },
            };
        }

        var workspace = new Workspace
        {
            User = user,
            Title = request.Title
        };

        await _unitOfWork.WorkspaceRepository.Create(workspace);
        _unitOfWork.Commit();

        return new ResponseBase<CreateWorkspaceViewModel>
        {
            ResponseInfo = null,
            Value = _mapper.Map<CreateWorkspaceViewModel>(workspace)
        };
    }
}
