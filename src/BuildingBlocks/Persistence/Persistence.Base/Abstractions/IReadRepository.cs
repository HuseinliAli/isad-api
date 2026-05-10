using Core.Domain.Entities;
using System.Linq.Expressions;

namespace Persistence.Base.Abstractions;
public interface IReadRepository<T>
    where T : IEntity
{
    IQueryable<T?> FindMany(Expression<Func<T, bool>> predicate, bool trackChanges = false);
    T? FindOne(Expression<Func<T, bool>> predicate, bool trackChanges = false);
    Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false);

    Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
    int Count(Expression<Func<T, bool>> predicate = null);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    bool Any(Expression<Func<T, bool>> predicate);
}