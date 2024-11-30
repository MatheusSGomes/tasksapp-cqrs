using Application.WorkspaceCQ.ViewModels;
using AutoMapper;
using Domain.Entity;

namespace Application.Mappings;

public class WorkspaceMappings : Profile
{
    public WorkspaceMappings()
    {
        CreateMap<Workspace, WorkspaceViewModel>()
            .ForMember(x => x.UserId, x => x.MapFrom(x => x.User!.Id));
    }
}
