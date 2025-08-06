using Psalms.EFCore.DataManager.Repository;

namespace Psalms.EFCore.DataManager.UnitOfWork;

/// <summary>
/// Represents a unit of work pattern, which coordinates repositories and commits changes.
/// </summary>
/// <typeparam name="TContext">The type of DbContext in use.</typeparam>
public interface IPsalmsUnitOfWork<TContext> 
{
    /// <summary>
    /// The generic repository used for data access.
    /// </summary>
    IPsalmsEfCoreRepository Repository { get; }

    /// <summary>
    /// Commits all pending changes to the database.
    /// </summary>
    Task<int> SaveChangesAsync();

    /// <inheritdoc />
    public ValueTask DisposeAsync();
}
