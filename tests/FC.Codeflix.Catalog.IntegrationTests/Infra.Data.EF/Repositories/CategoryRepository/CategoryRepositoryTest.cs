using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.Infra.Data.EF;

using Repository = FC.Codeflix.Catalog.Infra.Data.EF.Configurations.Repositories;

using FluentAssertions;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

[Collection(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTest
{
    private readonly CategoryRepositoryTestFixture _fixture;

    public CategoryRepositoryTest(CategoryRepositoryTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Insert))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Insert()
    {
        //Arrange
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetExampleCategory();

        var categoryRepository = new Repository.CategoryRepository(dbContext);

        //Act
        await categoryRepository.Insert(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        //Assert
        var dbCategory = await _fixture.CreateDbContext(true).Categories.FindAsync(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory.Should().BeEquivalentTo(exampleCategory);
    }
    
    [Fact(DisplayName = nameof(Get))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Get()
    {
        //Arrange
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetExampleCategory();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList();
        exampleCategoriesList.Add(exampleCategory);
        
        dbContext.Categories.AddRange(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext(true));

        //Act
        var dbCategory = await categoryRepository.Get(exampleCategory.Id, CancellationToken.None);

        //Assert
        dbCategory.Should().NotBeNull();
        dbCategory.Should().BeEquivalentTo(exampleCategory);
    }
    
    [Fact(DisplayName = nameof(GetThrowIfNotFound))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task GetThrowIfNotFound()
    {
        //Arrange
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleId = Guid.NewGuid();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList();
        
        dbContext.Categories.AddRange(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext());

        //Act
        var task = async () => await categoryRepository.Get(exampleId, CancellationToken.None);

        //Assert
        await task
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Category '{exampleId}' not found.");
    }
    
    [Fact(DisplayName = nameof(Update))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Update()
    {
        //Arrange
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetExampleCategory();
        var newCategoryValues = _fixture.GetExampleCategory();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList();
        exampleCategoriesList.Add(exampleCategory);
        
        await dbContext.Categories.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        exampleCategory.Update(newCategoryValues.Name, newCategoryValues.Description);
        
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        //Act
        await categoryRepository.Update(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        //Assert
        var dbCategory = await _fixture.CreateDbContext(true).Categories.FindAsync(exampleCategory.Id);
        dbCategory.Should().NotBeNull();
        dbCategory.Should().BeEquivalentTo(exampleCategory);
    }
    
    [Fact(DisplayName = nameof(Delete))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task Delete()
    {
        //Arrange
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategory = _fixture.GetExampleCategory();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList();
        exampleCategoriesList.Add(exampleCategory);
        
        await dbContext.Categories.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        //Act
        await categoryRepository.Delete(exampleCategory, CancellationToken.None);
        await dbContext.SaveChangesAsync();

        //Assert
        var dbCategory = await _fixture.CreateDbContext().Categories.FindAsync(exampleCategory.Id);
        dbCategory.Should().BeNull();
    }
    
    [Fact(DisplayName = nameof(SearchReturnsListAndTotal))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task SearchReturnsListAndTotal()
    {
        //Arrange
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList();
        
        await dbContext.Categories.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        //Act
        var output = await categoryRepository.Search(searchInput, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(exampleCategoriesList.Count);
        output.Items.Should().HaveCount(exampleCategoriesList.Count);
        output.Items.Should().BeEquivalentTo(exampleCategoriesList);
    }
    
    [Fact(DisplayName = nameof(SearchReturnEmptyWhenPersistenceIsEmpty))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    public async Task SearchReturnEmptyWhenPersistenceIsEmpty()
    {
        //Arrange
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var searchInput = new SearchInput(1, 20, "", "", SearchOrder.Asc);

        //Act
        var output = await categoryRepository.Search(searchInput, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(0);
        output.Items.Should().HaveCount(0);
    }
    
    [Theory(DisplayName = nameof(SearchReturnsPagineted))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    [InlineData(10, 1, 5, 5)]
    [InlineData(7, 2, 5, 2)]
    [InlineData(10, 2, 5, 5)]
    [InlineData(7, 3, 5, 0)]
    public async Task SearchReturnsPagineted(int quantityCategoriesToGenerate, int page, int perPage, int expectedQuantityItems)
    {
        //Arrange
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList(quantityCategoriesToGenerate);
        
        await dbContext.Categories.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        var categoryRepository = new Repository.CategoryRepository(dbContext);

        var searchInput = new SearchInput(page, perPage, "", "", SearchOrder.Asc);

        //Act
        var output = await categoryRepository.Search(searchInput, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(quantityCategoriesToGenerate);
        output.Items.Should().HaveCount(expectedQuantityItems);

        foreach (var outputItem in output.Items)
        {
            var exampleItem = exampleCategoriesList
                .Find(x => x.Id == outputItem.Id);
            
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }
    
    [Theory(DisplayName = nameof(SearchByText))]
    [Trait("Integration/Infra.Data", "CategoryRepository - Repositories")]
    [InlineData("Action", 1, 5, 1, 1)]
    [InlineData("Horror", 1, 5, 3, 3)]
    [InlineData("Horror", 2, 5, 0, 3)]
    [InlineData("Sci-fi", 1, 5, 4, 4)]
    [InlineData("Sci-fi", 1, 2, 2, 4)]
    [InlineData("Sci-fi", 2, 3, 1, 4)]
    [InlineData("Sci-fi Other", 1, 3, 0, 0)]
    [InlineData("Robots", 1, 5, 2, 2)]
    public async Task SearchByText(
        string search, 
        int page, 
        int perPage, 
        int expectedQuantityItemsReturned, 
        int expectedQuantityTotalItems)
    {
        //Arrange
        CodeflixCatalogDbContext dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesListWithNames(new List<string>
        {
            "Action",
            "Horror",
            "Horror - Robots",
            "Horror - Based on Real Facts",
            "Drama",
            "Sci-fi IA",
            "Sci-fi Space",
            "Sci-fi Robots",
            "Sci-fi Future"
        });
        
        await dbContext.Categories.AddRangeAsync(exampleCategoriesList);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        
        var categoryRepository = new Repository.CategoryRepository(dbContext);
        var searchInput = new SearchInput(page, perPage, search, "", SearchOrder.Asc);

        //Act
        var output = await categoryRepository.Search(searchInput, CancellationToken.None);

        //Assert
        output.Should().NotBeNull();
        output.Items.Should().NotBeNull();
        output.CurrentPage.Should().Be(searchInput.Page);
        output.PerPage.Should().Be(searchInput.PerPage);
        output.Total.Should().Be(expectedQuantityTotalItems);
        output.Items.Should().HaveCount(expectedQuantityItemsReturned);

        foreach (var outputItem in output.Items)
        {
            var exampleItem = exampleCategoriesList
                .Find(x => x.Id == outputItem.Id);
            
            outputItem.Should().NotBeNull();
            outputItem.Name.Should().Be(exampleItem.Name);
            outputItem.Description.Should().Be(exampleItem.Description);
            outputItem.IsActive.Should().Be(exampleItem.IsActive);
            outputItem.CreatedAt.Should().Be(exampleItem.CreatedAt);
        }
    }
}
