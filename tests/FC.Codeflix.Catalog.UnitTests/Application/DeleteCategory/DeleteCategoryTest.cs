using FC.Codeflix.Catalog.Application.Exceptions;

using FluentAssertions;

using Moq;

using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;


namespace FC.Codeflix.Catalog.UnitTests.Application.DeleteCategory;

[Collection(nameof(DeleteCategoryTestFixture))]
public class DeleteCategoryTest
{
    private readonly DeleteCategoryTestFixture _fixture;

    public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(DeleteCategory))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public async Task DeleteCategory()
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var categoryExample = _fixture.GetExampleCategory();
        var input = new UseCases.DeleteCategoryInput(categoryExample.Id);
        var useCase = new UseCases.DeleteCategory(repositoryMock.Object, unitOfWorkMock.Object);

        repositoryMock
            .Setup(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(categoryExample);

        //Act
        await useCase.Handle(input, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.Get(categoryExample.Id, It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.Delete(categoryExample, It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact(DisplayName = nameof(ThrowWhenCategoryDoesNotExist))]
    [Trait("Application", "DeleteCategory - Use Cases")]
    public async Task ThrowWhenCategoryDoesNotExist()
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var exampleGuid = Guid.NewGuid();
        var input = new UseCases.DeleteCategoryInput(exampleGuid);
        var useCase = new UseCases.DeleteCategory(repositoryMock.Object, unitOfWorkMock.Object);

        repositoryMock
            .Setup(x => x.Get(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"Category '{exampleGuid}' not found."));

        //Act
        Func<Task> task = async () => await useCase.Handle(input, CancellationToken.None);

        //Assert
        await task
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}
