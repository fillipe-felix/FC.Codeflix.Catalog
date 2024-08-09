﻿using FC.Codeflix.Catalog.Application.Interfaces;
using Entity = FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;

using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.Common;

public abstract class CategoryUseCasesBaseFixture : BaseFixture
{
    public Mock<ICategoryRepository> GetRepositoryMock() => new Mock<ICategoryRepository>();
    public Mock<IUnitOfWork> GetUnitOfWorkMock() => new Mock<IUnitOfWork>();
    
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
    
    public Entity.Category GetExampleCategory()
    {
        return new Entity.Category(GetValidCategoryName(), GetValidCategoryDescription(), GetRandomBoolean());
    }
}