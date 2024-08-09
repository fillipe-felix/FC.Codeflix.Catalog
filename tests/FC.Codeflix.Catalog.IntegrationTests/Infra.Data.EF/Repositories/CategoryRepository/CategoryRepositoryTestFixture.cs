﻿using Bogus;

using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.IntegrationTests.Base;

using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.CategoryRepository;

public class CategoryRepositoryTestFixture : BaseFixture
{
    public string GetValidCategoryName()
    {
        var categoryName = "";

        while (categoryName.Length < 3)
        {
            categoryName = Faker.Commerce.Categories(1)[0];
        }

        if (categoryName.Length > 255)
        {
            categoryName = categoryName.Substring(0, 255);
        }
        
        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescription = Faker.Commerce.ProductDescription();

        if (categoryDescription.Length > 10000)
        {
            categoryDescription = categoryDescription.Substring(0, 10000);
        }
        
        return categoryDescription;
    }

    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
    
    public Category GetExampleCategory()
    {
        return new Category(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    }

    public CodeflixCatalogDbContext CreateDbContext()
    {
        var dbContext = new CodeflixCatalogDbContext(new DbContextOptionsBuilder<CodeflixCatalogDbContext>()
            .UseInMemoryDatabase("integration-tests-db")
            .Options
        );

        return dbContext;
    }
}

[CollectionDefinition(nameof(CategoryRepositoryTestFixture))]
public class CategoryRepositoryTestFixtureCollection : ICollectionFixture<CategoryRepositoryTestFixture>{

}