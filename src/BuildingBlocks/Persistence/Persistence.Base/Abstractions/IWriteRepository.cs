using Core.Domain.Entities;

namespace Persistence.Base.Abstractions;

public interface IWriteRepository<T>
    where T : IEntity
{
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);

    void Add(T entity);
    Task AddAsync(T entity);
    void AddRange(IEnumerable<T> entities);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}
