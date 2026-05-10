using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Base.Abstractions;

namespace Persistence.EntityFramework.Repositories;
public abstract class WriteRepository<T> : IWriteRepository<T>
    where T : class, IEntity, new()
{
    protected DbContext dbContext;

    protected WriteRepository(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public void Add(T entity)
        => dbContext.Set<T>().Add(entity);

    public async Task AddAsync(T entity)
        => await dbContext.Set<T>().AddAsync(entity);

    public void AddRange(IEnumerable<T> entities)
        => dbContext.AddRange(entities);

    public async Task AddRangeAsync(IEnumerable<T> entities)
        => await dbContext.Set<T>().AddRangeAsync(entities);

    public void Remove(T entity)
        => dbContext.Set<T>().Remove(entity);

    public void RemoveRange(IEnumerable<T> entities)
        => dbContext.RemoveRange(entities);

    public void Update(T entity)
        => dbContext.Set<T>().Update(entity);

    public void UpdateRange(IEnumerable<T> entities)
        => dbContext.Set<T>().UpdateRange(entities);
}