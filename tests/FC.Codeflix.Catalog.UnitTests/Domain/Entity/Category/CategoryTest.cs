using FC.Codeflix.Catalog.Domain.Exceptions;

using FluentAssertions;

using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _fixture;

    public CategoryTest(CategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        //Arrange
        var validData = _fixture.GetValidCategory();
        var datetimeBefore = DateTime.Now;

        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        //Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        category.CreatedAt.Should().BeAfter(datetimeBefore);
        category.CreatedAt.Should().BeBefore(datetimeAfter);
        category.IsActive.Should().BeTrue();
    }
    
    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        //Arrange
        var validData = _fixture.GetValidCategory();
        var datetimeBefore = DateTime.Now;

        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        //Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        category.CreatedAt.Should().BeAfter(datetimeBefore);
        category.CreatedAt.Should().BeBefore(datetimeAfter);
        category.IsActive.Should().Be(isActive);
    }
    
    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        //Arrange
        var validCategory = _fixture.GetValidCategory();
        
        //Act
        Action action = () => new DomainEntity.Category(name!, validCategory.Description);
        
        //Assert
        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be null or empty.");
    }
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        //Arrange
        var validCategory = _fixture.GetValidCategory();
        
        //Act
        Action action = () => new DomainEntity.Category(validCategory.Name, null!);
        
        //Assert
        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be null.");
    }
    
    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string? invalidName)
    {
        //Arrange
        var validCategory = _fixture.GetValidCategory();
        
        //Act
        Action action = () => new DomainEntity.Category(invalidName!, validCategory.Description);
        
        //Assert
        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 characters long.");
    }
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        //Arrange
        var invalidName = _fixture.Faker.Lorem.Letter(256);
        var validCategory = _fixture.GetValidCategory();
        
        //Act
        Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);
        
        //Assert
        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long.");
    }
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        //Arrange
        var invalidDescription = _fixture.Faker.Commerce.ProductDescription();
        var validCategory = _fixture.GetValidCategory();

        while (invalidDescription.Length <= 10000)
        {
            invalidDescription = $"{invalidDescription} {_fixture.Faker.Commerce.ProductDescription()}";
        }
        
        //Act
        Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);
        
        //Assert
        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10000 characters long.");
    }
    
    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        //Arrange
        var validData = _fixture.GetValidCategory();

        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, false);
        category.Activate();

        //Assert
        category.IsActive.Should().BeTrue();
    }
    
    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        //Arrange
        var validData = _fixture.GetValidCategory();

        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        category.Deactivate();

        //Assert
        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        //Arrange
        var category = _fixture.GetValidCategory();
        var newValues = _fixture.GetValidCategory();
        
        //Act
        category.Update(newValues.Name, newValues.Description);
        
        //Assert
        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(newValues.Description);
    }
    
    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        //Arrange
        var category = _fixture.GetValidCategory();
        var newValues = _fixture.GetValidCategory();
        var currentDescription = category.Description;
        
        //Act
        category.Update(newValues.Name);
        
        //Assert
        category.Name.Should().Be(newValues.Name);
        category.Description.Should().Be(currentDescription);
    }
    
    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        //Arrange
        var category = _fixture.GetValidCategory();
        
        //Act
        Action action = () => category.Update(name!);
        
        //Assert
        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be null or empty.");
    }
    
    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)]
    public void UpdateErrorWhenNameIsLessThan3Characters(string? invalidName)
    {
        //Arrange
        var category = _fixture.GetValidCategory();
        
        //Act
        Action action = () => category.Update(invalidName!);
        
        //Assert
        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 characters long.");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        //Arrange
        var category = _fixture.GetValidCategory();
        var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        //Act
        Action action = () => category.Update(invalidName);

        //Assert
        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long.");
    }
    
    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        //Arrange
        var category = _fixture.GetValidCategory();
        var invalidDescription = _fixture.Faker.Commerce.ProductDescription();

        while (invalidDescription.Length <= 10000)
        {
            invalidDescription = $"{invalidDescription} {_fixture.Faker.Commerce.ProductDescription()}";
        }
        
        //Act
        Action action = () => category.Update(category.Name, invalidDescription);
        
        //Assert
        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10000 characters long.");
    }

    public static IEnumerable<object[]> GetNamesWithLessThan3Characters(int numberOfTests = 6)
    {
        var fixture = new CategoryTestFixture();

        for (int i = 0; i < numberOfTests; i++)
        {
            var isOdd = i % 2 == 1;
            yield return new object[] { fixture.GetValidCategoryName().Substring(0, isOdd ? 1 : 2) };
        }
    }
}
