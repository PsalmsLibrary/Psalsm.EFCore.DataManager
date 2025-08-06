# Psalms.DataManagement

> A clean, extensible and provider-agnostic **Repository** and **Unit of Work** library for .NET.

Psalms.DataManagement is a modular data access library designed to promote clean architecture, testability, and reusability.  
It provides a consistent API for different storage providers for **Entity Framework Core**

---

## ✨ Features

- 🔁 **Generic Repository Pattern** (`IPsalmsRepository`)
- 🔄 **Unit of Work Pattern** (`IPsalmsUnitOfWork<TContext>`)
- 📑 Full XML documentation
- 🧪 Ready for **unit testing** with EF Core InMemory
- 💡 Extendable: implement your own repository with the same contracts

---

## 📦 Packages

| Package | Description |
|---------|-------------|
| `Psalms.EFCore.DataManagement` | Entity Framework Core implementation of repository and unit of work. |

---

## 🚀 Getting Started

### 1. Install

```bash
dotnet add package Psalms.EFCore.DataManagement
```

### 2. Register in ASP.NET Core

```csharp
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("Default")));

services.AddScoped<IPsalmsUnitOfWork<AppDbContext>, PsalmsUnitOfWork<AppDbContext>>();
```

---

## 💻 Example Usage

```csharp
public class ProductService
{
    private readonly IPsalmsUnitOfWork<AppDbContext> _uow;

    public ProductService(IPsalmsUnitOfWork<AppDbContext> uow)
    {
        _uow = uow;
    }

    public async Task AddProductAsync(Product product)
    {
        await _uow.Repository.CreateAsync(product);
        await _uow.SaveChangesAsync();
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        return await _uow.Repository.GetByIdAsync<Product>(id);
    }
}
```

---

## 📚 Repository API

### Common Methods

- `CreateAsync<T>(T entity)`
- `UpdateAsync<T>(T entity)`
- `DeleteAsync<T>(int id)`
- `GetByIdAsync<T>(int id)`
- `GetByAsync<T>(Expression<Func<T, bool>> predicate)`
- `ExistsAsync<T>(Expression<Func<T, bool>> predicate)`
- `GetAllAsync<T>()`
- `GetCollectionFromEntityAsync<TEntity, TCollection>(...)`

---

## 🔄 Unit of Work API

### Common Methods

- `Repository` — Access to the generic repository
- `SaveChangesAsync()` — Commit pending changes
- `DisposeAsync()` — Dispose context and resources

---

## 🧪 Testing

Example with EF Core InMemory:

```csharp
services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TestDb"));

services.AddScoped<IPsalmsUnitOfWork<AppDbContext>, PsalmsUnitOfWork<AppDbContext>>();
```

You can then test services without a real database.

---

## 📄 License

This project is licensed under the MIT License.

You are free to use, modify, and distribute it.

## 🤝 Contributing

We welcome contributions from the community!

If you’d like to improve **Psalms.DataManagement**, fix bugs, or add new providers, please follow these steps:

### 1️⃣ Fork the repository

- Go to [this repository](https://github.com/SEU_USUARIO/SEU_REPOSITORIO)
- Click **Fork** to create your own copy.

### 2️⃣ Clone your fork

```bash
bash
git clone https://github.com/SEU_USUARIO/SEU_FORK.git
cd YOUR_FORK
```

### 3️⃣ Create a feature branch

```bash
bash
git checkout -b feature/my-awesome-feature
```

### 4️⃣ Make your changes

- Follow the **code style** used in the repository.
- Add **XML documentation** to public members.
- Write **unit tests** for new functionality.
- Keep commits **small and meaningful**.

### 5️⃣ Run tests before submitting

```bash
bash
dotnet test
```

### 6️⃣ Commit and push

```bash
bash
git add .
git commit -m "feat: add my awesome feature"
git push origin feature/my-awesome-featurE
```

### 7️⃣ Open a Pull Request

- Go to your fork on GitHub
- Click **Compare & pull request**
- Describe **what you changed** and **why**
- Link to any relevant issues

---

### 📌 Contribution Guidelines

- **One feature or bug fix per PR** — keeps reviews simple.
- **Use English** for code, documentation, and commit messages.
- Keep **public APIs backward-compatible** unless it’s a major release.
- Run `dotnet format` to ensure code style compliance.
- Add/update **unit tests** when introducing changes.
