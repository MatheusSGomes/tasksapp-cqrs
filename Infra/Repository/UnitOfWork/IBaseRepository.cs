using System.Linq.Expressions;

namespace Infra.Repository.UnitOfWork;

// T é o genéric. Adicionei a condição para que T sempre seja uma classe
// Não pode ser interface, record...
public interface IBaseRepository<T> where T : class
{
    /*
     * Expression - Faz um encapsulamento da expressão lambda
     * Func<> - É a expressão lambda
     * Func<TipoRecebido, TipoRetornado> - Recebe o tipo genérico T, retorna boolean
     * O uso será: Get(x => x.Id == variavel);
     */
    Task<T?> Get(Expression<Func<T, bool>> expression);
    IEnumerable<T> GetAll();
    Task<T> Create(T command);
    Task<T> Update(T commandUpdate);
    Task Delete(Guid id);
}
