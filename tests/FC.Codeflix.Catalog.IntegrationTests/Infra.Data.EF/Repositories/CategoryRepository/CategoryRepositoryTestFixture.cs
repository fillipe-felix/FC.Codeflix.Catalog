using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.IntegrationTests.Base;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

public class CategoryRepositoryTestFixture : BaseFixture
{
    public string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
        {
            categoryName = Faker.Commerce.Categories(1)[0];
        }

        if (categoryName.Length > 255)
        {
            categoryName = categoryName.Substring(0, 255);
        }
        
        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10000)
        {
            categoryDescription = categoryDescription.Substring(0, 10000);
        }
        
        return categoryDescription;
    }

    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
    
    public Category GetExampleCategory()
    {
        return new Category(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    }

    public List<Category> GetExampleCategoriesList(int length = 10)
    {
        return Enumerable
            .Range(1, length)
            .Select(_ => GetExampleCategory())
            .ToList();
    }
    
    public List<Category> GetExampleCategoriesListWithNames(IList<string> names)
    {
        return names.Select(name =>
        {
            var category = GetExampleCategory();
            category.Update(name);
            
            return category;
        }).ToList();
    }
    
    public List<Category> CloneCategoriesListOrdered(List<Category> categories, string orderBy, SearchOrder order)
    {
        var listClone = new List<Category>(categories);
        var orderedEnumerable = (orderBy.ToLower(), order) switch
        {
            ("name", SearchOrder.Asc) => listClone.OrderBy(c => c.Name),
            ("name", SearchOrder.Desc) => listClone.OrderByDescending(c => c.Name),
            ("id", SearchOrder.Asc) => listClone.OrderBy(c => c.Id),
            ("id", SearchOrder.Desc) => listClone.OrderByDescending(c => c.Id),
            ("createdat", SearchOrder.Asc) => listClone.OrderBy(c => c.CreatedAt),
            ("createdat", SearchOrder.Desc) => listClone.OrderByDescending(c => c.CreatedAt),
            _ => listClone.OrderBy(x => x.Name)
        };
        
        return orderedEnumerable.ToList();
    }
}

[CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTestFixtureCollection : ICollectionFixture<CategoryRepositoryTestFixture>{

}
