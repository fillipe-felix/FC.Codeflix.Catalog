using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory;

public class UpdateCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public Catalog.Domain.Entity.Category GetValidCategory() => new Catalog.Domain.Entity.Category(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());

    public UpdateCategoryInput GetValidInput(Guid? id = null)
    {
        return new UpdateCategoryInput(
            id ?? Guid.NewGuid(), 
            GetValidCategoryName(), 
            GetValidCategoryDescription(), 
            !GetRandomBoolean());
    }
    
    public UpdateCategoryInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetValidInput();
        invalidInputShortName.Name = invalidInputShortName.Name.Substring(0, 2);
        return invalidInputShortName;
    }

    public UpdateCategoryInput GetInvalidInputLongName()
    {
        var invalidInputLongName = GetValidInput();
        var tooLongNameForCategory = Faker.Commerce.ProductName();
        
        while (tooLongNameForCategory.Length <= 255)
        {
            tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";
        }
        
        invalidInputLongName.Name = tooLongNameForCategory;

        return invalidInputLongName;
    }

    public UpdateCategoryInput GetInvalidInputTooLongDescription()
    {
        var invalidInputLongDescription = GetValidInput();
        var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();
        
        while (tooLongDescriptionForCategory.Length < 10000)
        {
            tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";
        }
        
        invalidInputLongDescription.Description = tooLongDescriptionForCategory;
        
        return invalidInputLongDescription;
    }
}

[CollectionDefinition(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTestFixtureCollection : ICollectionFixture<UpdateCategoryTestFixture>
{
}
