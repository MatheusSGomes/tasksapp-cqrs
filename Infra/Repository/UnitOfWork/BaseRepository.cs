using System.Linq.Expressions;
using Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository.UnitOfWork;

public class BaseRepository<T>(TasksDbContext context) : IBaseRepository<T> where T : class
{
    private readonly TasksDbContext _context = context;

    public async Task<T> Create(T command)
    {
        // Semelhante ao uso: _context.Users.Add(users)
        await _context.Set<T>().AddAsync(command);
        return command;
    }

    public Task Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<T?> Get(Expression<Func<T, bool>> expression)
    {
        // Semelhante ao uso: _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        return await _context.Set<T>().FirstOrDefaultAsync(expression);
    }

    public IEnumerable<T> GetAll()
    {
        // Operador spread
        return [.. _context.Set<T>().ToList()];
    }

    public async Task<T> Update(T commandUpdate)
    {
        _context.Set<T>().Update(commandUpdate);
        return commandUpdate;
    }
}
