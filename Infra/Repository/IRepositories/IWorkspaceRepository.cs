using Domain.Entity;
using Infra.Repository.UnitOfWork;

namespace Infra.Repository.IRepositories;

public interface IWorkspaceRepository : IBaseRepository<Workspace>
{
    public Task<Workspace?> GetWorkspaceAndUser(Guid workspaceId);
    public Task<List<Workspace>> GetAllWorkspacesAndUser(Guid userId);
}
