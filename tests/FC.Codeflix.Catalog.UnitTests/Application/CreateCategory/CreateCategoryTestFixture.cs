using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.UnitTests.Application.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;

public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
{
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