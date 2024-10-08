﻿using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.IntegrationTests.Base;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.UnitOfWork;

public class UnitOfWorkTestFixture : BaseFixture
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

    public List<Category> GetExampleCategoriesList(int length = 10)
    {
        return Enumerable
            .Range(1, length)
            .Select(_ => GetExampleCategory())
            .ToList();
    }
}


[CollectionDefinition(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTestFixtureCollection : ICollectionFixture<UnitOfWorkTestFixture>
{
    
}