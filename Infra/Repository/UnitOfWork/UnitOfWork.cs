using Infra.Persistence;
using Infra.Repository.IRepositories;
using Infra.Repository.Repositories;

namespace Infra.Repository.UnitOfWork;

public class UnitOfWork(
    TasksDbContext context,
    IUserRepository userRepository,
    IWorkspaceRepository workspaceRepository) : IUnitOfWork
{
    private readonly TasksDbContext _context = context;

    public IUserRepository UserRepository => userRepository ?? new UserRepository(_context);
    public IWorkspaceRepository WorkspaceRepository => workspaceRepository ?? new WorkspaceRepository(_context);

    public void Commit()
    {
        _context.SaveChanges();
    }
}
