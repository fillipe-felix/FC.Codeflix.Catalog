﻿using FC.Codeflix.Catalog.Application.Exceptions;
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
        var dbCategory = await _fixture.CreateDbContext().Categories.FindAsync(exampleCategory.Id);
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
        
        var categoryRepository = new Repository.CategoryRepository(_fixture.CreateDbContext());

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
        var dbCategory = await _fixture.CreateDbContext().Categories.FindAsync(exampleCategory.Id);
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
}
