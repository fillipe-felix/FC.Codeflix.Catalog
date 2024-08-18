using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using UnitOfWorkInfra = FC.Codeflix.Catalog.Infra.Data.EF;

namespace FC.Codeflix.Catalog.IntegrationTests.Infra.Data.EF.Repositories.UnitOfWork;

[Collection(nameof(UnitOfWorkTestFixture))]
public class UnitOfWorkTest
{
    private readonly UnitOfWorkTestFixture _fixture;

    public UnitOfWorkTest(UnitOfWorkTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact(DisplayName = nameof(Commit))]
    [Trait("Integration/Infra.Data", "UnitofWork - Persistence")]
    public async Task Commit()
    {
        //Arrange
        var dbContext = _fixture.CreateDbContext();
        var exampleCategoriesList = _fixture.GetExampleCategoriesList();

        await dbContext.AddRangeAsync(exampleCategoriesList);
        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

        //Act
        await unitOfWork.Commit(CancellationToken.None);

        //Assert
        var assertDbContext = _fixture.CreateDbContext(true);
        var saveCategories = await assertDbContext
            .Categories
            .AsNoTracking()
            .ToListAsync();
        
        saveCategories.Should().HaveCount(exampleCategoriesList.Count);
    }
    
    [Fact(DisplayName = nameof(Rollback))]
    [Trait("Integration/Infra.Data", "UnitofWork - Persistence")]
    public async Task Rollback()
    {
        //Arrange
        var dbContext = _fixture.CreateDbContext();
        var unitOfWork = new UnitOfWorkInfra.UnitOfWork(dbContext);

        //Act
        var task = async () => await unitOfWork.Rollback(CancellationToken.None);

        //Assert
        await task
            .Should()
            .NotThrowAsync();
    }
    
}
