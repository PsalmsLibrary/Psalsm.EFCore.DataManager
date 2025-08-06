using System.Linq.Expressions;

namespace Psalms.EFCore.DataManager.Repository;

/// <summary>
/// Generic repository interface for working with entities using Entity Framework.
/// Provides basic CRUD operations and common data access methods.
/// </summary>
public interface IPsalmsEfCoreRepository
{
    /// <summary>
    /// Adds a new entity to the context.
    /// </summary>
    Task CreateAsync<T>(T entity) where T : class;

    /// <summary>
    /// Deletes an entity by its identifier.
    /// </summary>
    Task DeleteAsync<T>(int id) where T : class;

    /// <summary>
    /// Retrieves an entity by its identifier.
    /// </summary>
    Task<T?> GetByIdAsync<T>(int id) where T : class;

    /// <summary>
    /// Retrieves the first entity that matches a given predicate.
    /// </summary>
    Task<T?> GetByAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

    /// <summary>
    /// Checks whether any entity matches the given predicate.
    /// </summary>
    Task<bool> ExistsAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

    /// <summary>
    /// Retrieves all entities of the specified type.
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync<T>() where T : class;

    /// <summary>
    /// Retrieves a related collection from a given entity, using a navigation expression.
    /// </summary>
    Task<List<TCollection>> GetCollectionFromEntityAsync<TEntity, TCollection>(
        int entityId,
        Expression<Func<TEntity, IEnumerable<TCollection>>> includeExpression)
        where TEntity : class
        where TCollection : class;
}
