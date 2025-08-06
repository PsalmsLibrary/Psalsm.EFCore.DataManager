using Microsoft.EntityFrameworkCore;
using Psalms.EFCore.DataManager.Repository;

namespace Psalms.EFCore.DataManager.UnitOfWork;

/// <summary>
/// Default implementation of the Unit of Work pattern using Entity Framework.
/// Encapsulates the DbContext and coordinates data access through the repository.
/// </summary>
/// <typeparam name="TContext">The type of DbContext to use.</typeparam>
public class PsalmsUnitOfWork<TContext> : IPsalmsUnitOfWork<TContext>, IAsyncDisposable where TContext : DbContext
{
    private readonly TContext _context;

    /// <summary>
    /// The generic repository instance for entity operations.
    /// </summary>
    public IPsalmsEfCoreRepository Repository { get; private init; }

    /// <summary>
    /// Initializes a new instance of the Unit of Work with the default repository.
    /// </summary>
    public PsalmsUnitOfWork(TContext context)
    {
        _context = context;
        Repository = new PsalmsEfCoreRepository(context);
    }

    /// <summary>
    /// Initializes a new instance of the Unit of Work with a custom repository.
    /// </summary>
    public PsalmsUnitOfWork(TContext context, IPsalmsEfCoreRepository specifiedRepository) : this(context)
        => Repository = specifiedRepository ?? throw new ArgumentNullException(nameof(specifiedRepository));

    /// <inheritdoc />
    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    /// <inheritdoc />
    public ValueTask DisposeAsync() => _context.DisposeAsync();
}
