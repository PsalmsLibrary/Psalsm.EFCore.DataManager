using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Psalms.EFCore.DataManager.Repository;

/// <summary>
/// Generic implementation of a repository for Entity Framework.
/// Does not handle SaveChanges. It is expected to be used within a Unit of Work.
/// </summary>
public class PsalmsEfCoreRepository(DbContext context) : IPsalmsEfCoreRepository
{
    #region IRepository Methods
    public async Task CreateAsync<T>(T entity) where T : class
        => await context.Set<T>().AddAsync(entity); 

    public async Task DeleteAsync<T>(int id) where T : class
    {
        var entityToDelete = await GetByIdAsync<T>(id)
            ?? throw new InvalidOperationException($"{typeof(T).Name} com ID {id} não encontrado."); ;

        context.Set<T>().Remove(entityToDelete);
    }

    public Task UpdateAsync<T>(T model) where T : class
    {
        ArgumentNullException.ThrowIfNull(model);

        var entry = context.Entry(model);
        if (entry.State == EntityState.Detached)
        {
            context.Attach(model);
        }

        entry.State = EntityState.Modified;

        return Task.CompletedTask;
    }

    public async Task<T?> GetByIdAsync<T>(int id) where T : class
        => await context.Set<T>().FindAsync(id);
    public async Task<T?> GetByAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        => await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
    public async Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        => await GetByAsync(predicate) is not null;
    public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        => await context.Set<T>().ToListAsync();

    public async Task<List<TCollection>> GetCollectionFromEntityAsync<TEntity, TCollection>(
    int entityId,
    Expression<Func<TEntity, IEnumerable<TCollection>>> includeExpression)

    where TEntity : class
    where TCollection : class

    {
        var entity = await context.Set<TEntity>()
            .Include(includeExpression)
            .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == entityId)
            ?? throw new Exception($"{typeof(TEntity).Name} com ID {entityId} não encontrado.");

        var collection = includeExpression.Compile().Invoke(entity);

        return [.. collection];
    }
    #endregion
}