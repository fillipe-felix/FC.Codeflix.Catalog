using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Infra.Data.EF.Configurations.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CodeflixCatalogDbContext _context;
    private DbSet<Category> _categories => _context.Set<Category>();

    public CategoryRepository(CodeflixCatalogDbContext context)
    {
        _context = context;
    }

    public async Task Insert(Category aggregate, CancellationToken cancellationToken)
    {
        await _categories.AddAsync(aggregate, cancellationToken);
    }

    public async Task<Category?> Get(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        
        NotFoundException.ThrowIfNull(category, $"Category '{id}' not found.");
        
        return category!;
    }

    public async Task Delete(Category aggregate, CancellationToken cancellationToken)
    {
        await Task.FromResult(_categories.Remove(aggregate));
    }

    public async Task Update(Category aggregate, CancellationToken cancellationToken)
    {
        await Task.FromResult(_categories.Update(aggregate));
    }

    public async Task<SearchOutput<Category>> Search(SearchInput searchInput, CancellationToken cancellationToken)
    {
        var toSkip = (searchInput.Page - 1) * searchInput.PerPage;
        var total = await _categories
            .CountAsync(cancellationToken);
        
        var items = await _categories
            .AsNoTracking()
            .Skip(toSkip)
            .Take(searchInput.PerPage)
            .ToListAsync(cancellationToken);
        
        return new SearchOutput<Category>(searchInput.Page, searchInput.PerPage, total, items);
    }
}
