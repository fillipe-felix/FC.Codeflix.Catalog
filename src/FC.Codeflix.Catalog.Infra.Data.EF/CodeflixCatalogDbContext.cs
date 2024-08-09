using System.Reflection;

using FC.Codeflix.Catalog.Domain.Entity;

using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Infra.Data.EF;

public class CodeflixCatalogDbContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    
    public CodeflixCatalogDbContext(DbContextOptions<CodeflixCatalogDbContext> oprions) : base(oprions)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
