using System.Globalization;

using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryInputValidatorTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryInputValidatorTest(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
        
        var cultureInfo = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
    }

    [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
    [Trait("Application", "UpdateCategoryInputValidatorTest - Use Cases")]
    public void DontValidateWhenEmptyGuid()
    {
        //Arrange
        var input = _fixture.GetValidInput(Guid.Empty);
        var validator = new UpdateCategoryInputValidator();

        //Act
        var validateResult = validator.Validate(input);

        //Assert
        validator.Should().NotBeNull();
        validateResult.IsValid.Should().BeFalse();
        validateResult.Errors.Should().HaveCount(1);
        validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
    }
    
    [Fact(DisplayName = nameof(ValidateWhenValid))]
    [Trait("Application", "UpdateCategoryInputValidatorTest - Use Cases")]
    public void ValidateWhenValid()
    {
        //Arrange
        var input = _fixture.GetValidInput(Guid.NewGuid());
        var validator = new UpdateCategoryInputValidator();

        //Act
        var validateResult = validator.Validate(input);

        //Assert
        validator.Should().NotBeNull();
        validateResult.IsValid.Should().BeTrue();
        validateResult.Errors.Should().BeEmpty();
    }
}
