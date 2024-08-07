using System.Globalization;

using FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;

using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.GetCategory;

[Collection(nameof(GetCategoryTestFixture))]
public class GetCategoryInputValidatorTest
{
    public GetCategoryInputValidatorTest()
    {
        var cultureInfo = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
    }

    [Fact(DisplayName = nameof(ValidationOk))]
    [Trait("Application", "GetCategoryInputValidation - Use Cases")]
    public void ValidationOk()
    {
        //Arrange
        var validInput = new GetCategoryInput(Guid.NewGuid());
        var validator = new GetCategoryInputValidator();

        //Act
        var validateResult = validator.Validate(validInput);

        //Assert
        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeTrue();
        validateResult.Errors.Should().HaveCount(0);
    }
    
    [Fact(DisplayName = nameof(InvalidWhenEmptyGuiId))]
    [Trait("Application", "GetCategoryInputValidation - Use Cases")]
    public void InvalidWhenEmptyGuiId()
    {
        //Arrange
        var invalidInput = new GetCategoryInput(Guid.Empty);
        var validator = new GetCategoryInputValidator();

        //Act
        var validateResult = validator.Validate(invalidInput);

        //Assert
        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeFalse();
        validateResult.Errors.Should().HaveCount(1);
        validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
}
