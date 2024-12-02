using Domain.Entity;
using Infra.Persistence;
using Infra.Repository.IRepositories;
using Infra.Repository.UnitOfWork;

namespace Infra.Repository.Repositories;

public class ListCardsRepository(TasksDbContext context) : BaseRepository<ListCard>(context), IListCardsRepository
{
    private readonly TasksDbContext _context = context;
}
