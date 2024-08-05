using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory;

public static class UpdateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int count = 12)
    {
        var fixture = new UpdateCategoryTestFixture();

        for (int i = 0; i < count; i++)
        {
            var exampleCategory = fixture.GetValidCategory();
            var input = fixture.GetValidInput(exampleCategory.Id);
            
            yield return new object[] { exampleCategory, input };
        }
    }
    
}
