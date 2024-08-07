using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

using FluentAssertions;

using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;

using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.ListCategories;

[Collection(nameof(ListCategoriesTestFixture))]
public class ListCategoriesTest
{
    private readonly ListCategoriesTestFixture _fixture;

    public ListCategoriesTest(ListCategoriesTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(List))]
    [Trait("Application", "ListCategories - Use Cases")]
    public async Task List()
    {
        //Arrange
        var categoriesExampleList = _fixture.GetExampleCategoriesList();
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = _fixture.GetExampleInput();
        
        var outputRepositorySearch = new SearchOutput<Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: categoriesExampleList,
            total: new Random().Next(50, 200)
        );

        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCases.ListCategories(repositoryMock.Object);

        //Act
        var output = await useCase.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
        output.Items.Should().BeEquivalentTo(outputRepositorySearch.Items);

        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir)
            , It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Theory(DisplayName = nameof(ListInputWithoutAllParameters))]
    [Trait("Application", "ListCategories - Use Cases")]
    [MemberData(nameof(ListCategoriesTestDataGenerator.GetInputsWithoutAllParameters), parameters: 14, MemberType = typeof(ListCategoriesTestDataGenerator))]
    public async Task ListInputWithoutAllParameters(UseCases.ListCategoriesInput input)
    {
        //Arrange
        var categoriesExampleList = _fixture.GetExampleCategoriesList();
        var repositoryMock = _fixture.GetRepositoryMock();
        
        var outputRepositorySearch = new SearchOutput<Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: categoriesExampleList,
            total: new Random().Next(50, 200)
        );

        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCases.ListCategories(repositoryMock.Object);

        //Act
        var output = await useCase.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(outputRepositorySearch.Total);
        output.Items.Should().HaveCount(outputRepositorySearch.Items.Count);
        output.Items.Should().BeEquivalentTo(outputRepositorySearch.Items);

        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir)
            , It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact(DisplayName = nameof(ListWhenEmpty))]
    [Trait("Application", "ListCategories - Use Cases")]
    public async Task ListWhenEmpty()
    {
        //Arrange
        var repositoryMock = _fixture.GetRepositoryMock();
        var input = _fixture.GetExampleInput();
        
        var outputRepositorySearch = new SearchOutput<Category>(
            currentPage: input.Page,
            perPage: input.PerPage,
            items: new List<Category>().AsReadOnly(),
            total: 0
        );

        repositoryMock.Setup(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir
            ),
            It.IsAny<CancellationToken>()
        )).ReturnsAsync(outputRepositorySearch);

        var useCase = new UseCases.ListCategories(repositoryMock.Object);

        //Act
        var output = await useCase.Handle(input, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Page.Should().Be(outputRepositorySearch.CurrentPage);
        output.PerPage.Should().Be(outputRepositorySearch.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);

        repositoryMock.Verify(x => x.Search(
            It.Is<SearchInput>(
                searchInput => searchInput.Page == input.Page &&
                               searchInput.PerPage == input.PerPage &&
                               searchInput.Search == input.Search &&
                               searchInput.OrderBy == input.Sort &&
                               searchInput.Order == input.Dir)
            , It.IsAny<CancellationToken>()), Times.Once);
    }
}
