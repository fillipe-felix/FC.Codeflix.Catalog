using FC.Codeflix.Catalog.Domain.Exceptions;

using FluentAssertions;

using Moq;

using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.CreateCategory;

[Collection(nameof(CreateCategoryTestFixture))]
public class CreateCategoryTest
{
    private readonly CreateCategoryTestFixture _fixture;

    public CreateCategoryTest(CreateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategory()
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = _fixture.GetValidCategoryInput();
        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        //Act
        var output = await useCase.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(input.IsActive);
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        
        repositoryMock.Verify(x => x.Insert(It.IsAny<Catalog.Domain.Entity.Category>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact(DisplayName = nameof(CreateCategoryWithOnlyName))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlyName()
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = new UseCases.CreateCategoryInput(_fixture.GetValidCategoryInput().Name);
        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        //Act
        var output = await useCase.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().BeEmpty();
        output.IsActive.Should().BeTrue();
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        
        repositoryMock.Verify(x => x.Insert(It.IsAny<Catalog.Domain.Entity.Category>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact(DisplayName = nameof(CreateCategoryWithOnlyNameAndDescription))]
    [Trait("Application", "CreateCategory - Use Cases")]
    public async void CreateCategoryWithOnlyNameAndDescription()
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = new UseCases.CreateCategoryInput(_fixture.GetValidCategoryInput().Name, _fixture.GetValidCategoryInput().Description);
        var useCase = new UseCases.CreateCategory(repositoryMock.Object, unitOfWorkMock.Object);

        //Act
        var output = await useCase.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Id.Should().NotBeEmpty();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().BeTrue();
        output.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        
        repositoryMock.Verify(x => x.Insert(It.IsAny<Catalog.Domain.Entity.Category>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
    [Trait("Application", "CreateCategory - Use Cases")]
    [MemberData(nameof(CreateCategoryTestDataGenerator.GetInvalidInputs), parameters: 24, MemberType = typeof(CreateCategoryTestDataGenerator))]
    public async void ThrowWhenCantInstantiateAggregate(UseCases.CreateCategoryInput input, string exceptionMessage)
    {
        //Arrange
        var useCase = new UseCases.CreateCategory(_fixture.GetRepositoryMock().Object, _fixture.GetUnitOfWorkMock().Object);

        //Act
        Func<Task> action = async () => await useCase.Handle(input, CancellationToken.None);

        //Assert
        await action
            .Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(exceptionMessage);
    }
}
