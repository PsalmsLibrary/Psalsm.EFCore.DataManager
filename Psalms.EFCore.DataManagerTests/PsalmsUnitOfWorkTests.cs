using Microsoft.EntityFrameworkCore;
using Psalms.EFCore.DataManager.UnitOfWork;

namespace Psalms.EFCore.DataManagerTests;

public class PsalmsUnitOfWorkTests
{
    private static TestDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new TestDbContext(options);
    }

    [Fact]
    public void Constructor_Should_Initialize_Repository()
    {
        // Arrange
        var context = GetDbContext(nameof(Constructor_Should_Initialize_Repository));

        // Act
        var uow = new PsalmsUnitOfWork<TestDbContext>(context);

        // Assert
        Assert.NotNull(uow.Repository);
    }

    [Fact]
    public async Task SaveChangesAsync_Should_Persist_Data()
    {
        // Arrange
        var context = GetDbContext(nameof(SaveChangesAsync_Should_Persist_Data));
        var uow = new PsalmsUnitOfWork<TestDbContext>(context);

        var entity = new Product { Id = 1, Name = "Test Product" };

        // Act
        await uow.Repository.CreateAsync(entity);
        var result = await uow.SaveChangesAsync();

        // Assert
        Assert.Equal(1, result);
        Assert.Single(context.Products);
    }

    [Fact]
    public async Task DisposeAsync_Should_Dispose_DbContext()
    {
        // Arrange
        var context = GetDbContext(nameof(DisposeAsync_Should_Dispose_DbContext));
        var uow = new PsalmsUnitOfWork<TestDbContext>(context);

        // Act
        await uow.DisposeAsync();

        // Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(() => context.Products.ToListAsync());
    }
}