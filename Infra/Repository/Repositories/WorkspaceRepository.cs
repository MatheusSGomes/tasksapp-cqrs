using Domain.Entity;
using Infra.Persistence;
using Infra.Repository.IRepositories;
using Infra.Repository.UnitOfWork;

namespace Infra.Repository.Repositories;

public class WorkspaceRepository(TasksDbContext context) : BaseRepository<Workspace>(context), IWorkspaceRepository
{
}
