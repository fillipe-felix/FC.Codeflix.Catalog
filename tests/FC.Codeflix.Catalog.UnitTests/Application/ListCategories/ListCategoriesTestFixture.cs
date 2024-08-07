using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.UnitTests.Common;

using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.ListCategories;

public class ListCategoriesTestFixture : BaseFixture
{

    public Mock<ICategoryRepository> GetRepositoryMock() => new Mock<ICategoryRepository>();
    
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
    
    public Category GetValidCategory()
    {
        return new Category(GetValidCategoryName(), GetValidCategoryDescription());
    }

    public List<Category> GetExampleCategoriesList(int length = 10)
    {
        var list = new List<Category>();

        for (int i = 0; i < length; i++)
        {
            var category = GetValidCategory();
            list.Add(category);
        }

        return list;
    }

    public ListCategoriesInput GetExampleInput()
    {
        var random = new Random();
        
        return new ListCategoriesInput(
            page: random.Next(1, 10),
            perPage: random.Next(15, 100),
            search: Faker.Commerce.ProductName(),
            sort: Faker.Commerce.ProductName(),
            dir: random.Next(1, 10) > 5 ? SearchOrder.Asc : SearchOrder.Desc
        );
    }
}

[CollectionDefinition(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTestFixtureCollection : ICollectionFixture<ListCategoriesTestFixture>
{
    
}
