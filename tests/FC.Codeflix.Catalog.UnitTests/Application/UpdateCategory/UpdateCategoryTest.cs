using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Exceptions;

using FluentAssertions;

using Moq;

using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory;

[Collection(nameof(UpdateCategoryTestFixture))]
public class UpdateCategoryTest
{
    private readonly UpdateCategoryTestFixture _fixture;

    public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Theory(DisplayName = nameof(UpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task UpdateCategory(Category exampleCategory, UseCases.UpdateCategoryInput input)
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
       var useCase = new UseCases.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        
        repositoryMock
            .Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleCategory);

        //Act
        var output = await useCase.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be((bool)input.IsActive!);
        
        repositoryMock.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(exampleCategory, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    public async Task ThrowWhenCategoryNotFound()
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = _fixture.GetValidInput();
        var useCase = new UseCases.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        
        repositoryMock
            .Setup(x => x.Get(input.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Category '{input.Id}' not found"));

        //Act
        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        //Assert
        await task
            .Should()
            .ThrowAsync<NotFoundException>();
        
        repositoryMock.Verify(x => x.Get(input.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Theory(DisplayName = nameof(UpdateCategoryWithoutProvidingIsActive))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task UpdateCategoryWithoutProvidingIsActive(Category exampleCategory, UseCases.UpdateCategoryInput exampleInput)
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        var input = new UseCases.UpdateCategoryInput(exampleInput.Id, exampleInput.Name, exampleInput.Description);
        
        repositoryMock
            .Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleCategory);

        //Act
        var output = await useCase.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(input.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        
        repositoryMock.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(exampleCategory, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Theory(DisplayName = nameof(UpdateCategoryOnlyName))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate), parameters: 10, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task UpdateCategoryOnlyName(Category exampleCategory, UseCases.UpdateCategoryInput exampleInput)
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        var input = new UseCases.UpdateCategoryInput(exampleInput.Id, exampleInput.Name);
        
        repositoryMock
            .Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleCategory);

        //Act
        var output = await useCase.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.Description.Should().Be(exampleCategory.Description);
        output.IsActive.Should().Be(exampleCategory.IsActive);
        
        repositoryMock.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Update(exampleCategory, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Theory(DisplayName = nameof(ThrowWhenCantUpdateCategory))]
    [Trait("Application", "UpdateCategory - Use Cases")]
    [MemberData(nameof(UpdateCategoryTestDataGenerator.GetInvalidInputs), parameters: 12, MemberType = typeof(UpdateCategoryTestDataGenerator))]
    public async Task ThrowWhenCantUpdateCategory(UseCases.UpdateCategoryInput input, string expectedExceptionMessage)
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var useCase = new UseCases.UpdateCategory(repositoryMock.Object, unitOfWorkMock.Object);
        var exampleCategory = _fixture.GetValidCategory();
        input.Id = exampleCategory.Id;
        
        repositoryMock
            .Setup(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exampleCategory);

        //Act
        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        //Assert
        await task
            .Should()
            .ThrowAsync<EntityValidationException>()
            .WithMessage(expectedExceptionMessage);
        
        repositoryMock.Verify(x => x.Get(exampleCategory.Id, It.IsAny<CancellationToken>()), Times.Once);
    }
}
