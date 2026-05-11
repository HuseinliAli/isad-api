using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Base.Abstractions;
using System.Linq.Expressions;

namespace Persistence.EntityFramework.Repositories;

public abstract class ReadRepository<T> : IReadRepository<T>
    where T : class, IEntity, new()
{
    protected DbContext dbContext;

    protected ReadRepository(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<T> FindOneAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false)
        => trackChanges ? await dbContext.Set<T>().FirstOrDefaultAsync(predicate) :
                          await dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);


    public IQueryable<T> FindMany(Expression<Func<T, bool>> predicate, bool trackChanges = false)
        => trackChanges ? dbContext.Set<T>().Where(predicate) :
                          dbContext.Set<T>().Where(predicate).AsNoTracking();

    public T FindOne(Expression<Func<T, bool>> predicate, bool trackChanges = false)
        => trackChanges ? dbContext.Set<T>().FirstOrDefault(predicate)
        : dbContext.Set<T>().AsNoTracking().FirstOrDefault(predicate);

    public Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        => predicate == null ? dbContext.Set<T>().AsNoTracking().CountAsync()
        : dbContext.Set<T>().AsNoTracking().CountAsync(predicate);

    public int Count(Expression<Func<T, bool>> predicate = null)
        => predicate == null ? dbContext.Set<T>().AsNoTracking().Count()
        : dbContext.Set<T>().AsNoTracking().Count(predicate);

    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        => dbContext.Set<T>().AsNoTracking().AnyAsync(predicate);

    public bool Any(Expression<Func<T, bool>> predicate)
        => dbContext.Set<T>().AsNoTracking().Any(predicate);
}
