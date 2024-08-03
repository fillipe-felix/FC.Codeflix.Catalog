using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;

using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;

public class CreateCategoryTestFixture : BaseFixture
{
    public CreateCategoryTestFixture() : base()
    {
    }
    
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
    public Mock<ICategoryRepository> GetRepositoryMock() => new Mock<ICategoryRepository>();
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new Mock<IUnitOfWork>();

    public CreateCategoryInput GetValidCategoryInput()
    {
        return new CreateCategoryInput(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    }

    public CreateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetValidCategoryInput();
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
        return invalidInputShortName;
    }

    public CreateCategoryInput GetInvalidInputLongName()
    {
        var invalidInputLongName = GetValidCategoryInput();
        var tooLongNameForCategory = Faker.Commerce.ProductName();
        
        while (tooLongNameForCategory.Length <= 255)
        {
            tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";
        }
        
        invalidInputLongName.Name = tooLongNameForCategory;

        return invalidInputLongName;
    }

    public CreateCategoryInput GetInvalidInputDescriptionNull()
    {
        var invalidInputLongDescriptionNull = GetValidCategoryInput();
        invalidInputLongDescriptionNull.Description = null;
        return invalidInputLongDescriptionNull;
    }

    public CreateCategoryInput GetInvalidInputTooLongDescription()
    {
        var invalidInputLongDescription = GetValidCategoryInput();
        var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();
        
        while (tooLongDescriptionForCategory.Length < 10000)
        {
            tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";
        }
        
        invalidInputLongDescription.Description = tooLongDescriptionForCategory;
        
        return invalidInputLongDescription;
    }
}

[CollectionDefinition(nameof(CreateCategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CreateCategoryTestFixture>
{
}