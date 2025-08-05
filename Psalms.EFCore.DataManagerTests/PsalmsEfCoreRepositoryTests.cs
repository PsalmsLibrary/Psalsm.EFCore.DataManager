using Microsoft.EntityFrameworkCore;
using Psalms.EFCore.DataManager.Repository;

namespace Psalms.EFCore.DataManagerTests;

public class PsalmsEfCoreRepositoryTests
{
    private static TestDbContext GetDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new TestDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_Should_Add_Entity()
    {
        // Arrange
        var context = GetDbContext(nameof(CreateAsync_Should_Add_Entity));
        var repository = new PsalmsEfCoreRepository(context);
        var entity = new Product { Id = 1, Name = "Test Product" };

        // Act
        await repository.CreateAsync(entity);
        await context.SaveChangesAsync();

        // Assert
        Assert.Equal(1, context.Products.Count());
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Entity_Properties()
    {
        // Arrange
        var context = GetDbContext(nameof(UpdateAsync_Should_Update_Entity_Properties));
        var repository = new PsalmsEfCoreRepository(context);

        var product = new Product { Id = 1, Name = "Old Name" };
        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Act
        product.Name = "New Name";
        await repository.UpdateAsync(product);
        await context.SaveChangesAsync();

        // Assert
        var updated = await context.Products.FirstOrDefaultAsync(p => p.Id == 1);
        Assert.NotNull(updated);
        Assert.Equal("New Name", updated!.Name);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Entity()
    {
        var context = GetDbContext(nameof(GetByIdAsync_Should_Return_Entity));
        var entity = new Product { Id = 2, Name = "Test 2" };
        context.Products.Add(entity);
        await context.SaveChangesAsync();

        var repository = new PsalmsEfCoreRepository(context);

        var result = await repository.GetByIdAsync<Product>(2);

        Assert.NotNull(result);
        Assert.Equal("Test 2", result?.Name);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Entity()
    {
        var context = GetDbContext(nameof(DeleteAsync_Should_Remove_Entity));
        var entity = new Product { Id = 3, Name = "Delete Me" };
        context.Products.Add(entity);
        await context.SaveChangesAsync();

        var repository = new PsalmsEfCoreRepository(context);

        await repository.DeleteAsync<Product>(3);
        await context.SaveChangesAsync();

        Assert.Empty(context.Products);
    }

    [Fact]
    public async Task ExistsAsync_Should_Return_True_If_Entity_Exists()
    {
        var context = GetDbContext(nameof(ExistsAsync_Should_Return_True_If_Entity_Exists));
        context.Products.Add(new Product { Id = 4, Name = "Exists" });
        await context.SaveChangesAsync();

        var repository = new PsalmsEfCoreRepository(context);

        var exists = await repository.ExistsAsync<Product>(p => p.Id == 4);

        Assert.True(exists);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Entities()
    {
        var context = GetDbContext(nameof(GetAllAsync_Should_Return_All_Entities));
        context.Products.AddRange(
            new Product { Id = 5, Name = "P1" },
            new Product { Id = 6, Name = "P2" }
        );
        await context.SaveChangesAsync();

        var repository = new PsalmsEfCoreRepository(context);

        var all = await repository.GetAllAsync<Product>();

        Assert.Equal(2, all.Count());
    }
}

public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
