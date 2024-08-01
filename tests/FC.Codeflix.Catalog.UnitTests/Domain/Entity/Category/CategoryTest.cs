using FC.Codeflix.Catalog.Domain.Exceptions;

using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        //Arrange
        var validData = new
        {
            Name = "Category name",
            Description = "Category description"
        };
        var datetimeBefore = DateTime.Now;

        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var datetimeAfter = DateTime.Now;

        //Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.True(category.IsActive);
    }
    
    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        //Arrange
        var validData = new
        {
            Name = "Category name",
            Description = "Category description"
        };
        var datetimeBefore = DateTime.Now;

        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        var datetimeAfter = DateTime.Now;

        //Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }
    
    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        //Arrange and Act
        Action action = () => new DomainEntity.Category(name!, "Category description");
        
        //Assert
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        //Arrange and Act
        Action action = () => new DomainEntity.Category("Category name", null!);
        
        //Assert
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be empty or null", exception.Message);
    }
    
    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("Fi")]
    [InlineData("F")]
    [InlineData("1")]
    [InlineData("12")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string? invalidName)
    {
        //Arrange and Act
        Action action = () => new DomainEntity.Category(invalidName!, "Category description");
        
        //Assert
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at leats 3 chatacters long", exception.Message);
    }
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        //Arrange
        var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        
        //Act
        Action action = () => new DomainEntity.Category(invalidName, "Category description");
        
        //Assert
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 chatacters long", exception.Message);
    }
    
    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        //Arrange
        var invalidDescription = string.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
        
        //Act
        Action action = () => new DomainEntity.Category("Category name", invalidDescription);
        
        //Assert
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10.000 chatacters long", exception.Message);
    }
    
    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        //Arrange
        var validData = new
        {
            Name = "Category name",
            Description = "Category description"
        };

        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, false);
        category.Activate();

        //Assert
        Assert.True(category.IsActive);
    }
    
    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        //Arrange
        var validData = new
        {
            Name = "Category name",
            Description = "Category description"
        };

        //Act
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        category.Deactivate();

        //Assert
        Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        //Arrange
        var category = new DomainEntity.Category("Category name", "Category description");
        var newValues = new { Name = "New name", Description = "New description" };
        
        //Act
        category.Update(newValues.Name, newValues.Description);
        
        //Assert
        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(newValues.Description, category.Description);
    }
    
    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        //Arrange
        var category = new DomainEntity.Category("Category name", "Category description");
        var newValues = new { Name = "New name" };
        var currentDescription = category.Description;
        
        //Act
        category.Update(newValues.Name);
        
        //Assert
        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(currentDescription, category.Description);
    }
    
    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        //Arrange
        var category = new DomainEntity.Category("Category name", "Category description");
        
        //Act
        Action action = () => category.Update(name!);
        
        //Assert
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }
    
    [Theory(DisplayName = nameof(UpdateeErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("Fi")]
    [InlineData("F")]
    [InlineData("1")]
    [InlineData("12")]
    public void UpdateeErrorWhenNameIsLessThan3Characters(string? invalidName)
    {
        //Arrange
        var category = new DomainEntity.Category("Category name", "Category description");
        
        //Act
        Action action = () => category.Update(invalidName!);
        
        //Assert
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at leats 3 chatacters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        //Arrange
        var category = new DomainEntity.Category("Category name", "Category description");
        var invalidName = string.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        //Act
        Action action = () => category.Update(invalidName);

        //Assert
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 chatacters long", exception.Message);
    }
    
    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        //Arrange
        var category = new DomainEntity.Category("Category name", "Category description");
        var invalidDescription = string.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
        
        //Act
        Action action = () => category.Update("Category new name", invalidDescription);
        
        //Assert
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10.000 chatacters long", exception.Message);
    }
}
