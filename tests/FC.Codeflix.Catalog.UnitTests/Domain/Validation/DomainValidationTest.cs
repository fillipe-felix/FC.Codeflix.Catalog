using Bogus;

using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Validation;

using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation;

public class DomainValidationTest
{
    private Faker Faker { get; } = new();
    
    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        //Arrange
        var value = Faker.Commerce.ProductName();
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        
        //Act
        Action action = () => DomainValidation.NotNull(value, fieldName);

        //Assert
        action.Should().NotThrow();
    }
    
    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        //Arrange
        string? value = null;
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        
        //Act
        Action action = () => DomainValidation.NotNull(value, fieldName);

        //Assert
        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null.");
    }

    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        //Arrange
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        
        //Act
        Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);
        
        //Assert
        action
            .Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null or empty.");
    }
    
    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        //Arrange
        var target = Faker.Commerce.ProductName();
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        
        //Act
        Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);
        
        //Assert
        action
            .Should()
            .NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLessThanMinLength))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
    public void MinLengthThrowWhenLessThanMinLength(string target, int minLength)
    {
        //Arrange
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        
        //Act
        Action action = () => DomainValidation.MinLength(target, minLength, fieldName);
        
        //Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be at leats {minLength} characters long.");
    }
    
    [Theory(DisplayName = nameof(MinLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
    public void MinLengthOk(string target, int minLength)
    {
        //Arrange
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        
        //Act
        Action action = () => DomainValidation.MinLength(target, minLength, fieldName);
        
        //Assert
        action.Should()
            .NotThrow();
    }
    
    [Theory(DisplayName = nameof(MaxLengthThrowWhenGreaterThanMinLength))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]
    public void MaxLengthThrowWhenGreaterThanMinLength(string target, int maxLength)
    {
        //Arrange
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        
        //Act
        Action action = () => DomainValidation.MaxLength(target, maxLength, fieldName);
        
        //Assert
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be less or equal {maxLength} characters long.");
    }
    
    [Theory(DisplayName = nameof(MaxLengthOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
    public void MaxLengthOk(string target, int minLength)
    {
        //Arrange
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        
        //Act
        Action action = () => DomainValidation.MaxLength(target, minLength, fieldName);
        
        //Assert
        action.Should()
            .NotThrow();
    }

    public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOfTests = 5)
    {
        var faker = new Faker();

        for (int i = 0; i < numberOfTests; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + new Random().Next(1, 20);
            yield return new object[] { example, minLength };
        }
    }
    
    public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOfTests = 5)
    {
        var faker = new Faker();

        for (int i = 0; i < numberOfTests; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length - new Random().Next(1, 5);
            yield return new object[] { example, minLength };
        }
    }
    
    public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOfTests = 5)
    {
        var faker = new Faker();

        for (int i = 0; i < numberOfTests; i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length - new Random().Next(1, 5);
            yield return new object[] { example, maxLength };
        }
    }
    
    public static IEnumerable<object[]> GetValuesLessThanMax(int numberOfTests = 5)
    {
        var faker = new Faker();

        for (int i = 0; i < numberOfTests; i++)
        {
            var example = faker.Commerce.ProductName();
            var maxLength = example.Length + new Random().Next(0, 5);
            yield return new object[] { example, maxLength };
        }
    }
}
