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
    
    public static IEnumerable<object[]> GetInvalidInputs(int count = 12)
    {
        var fixture = new UpdateCategoryTestFixture();
        var invalidInputList = new List<object[]>();
        var totalInvalidCases = 3;

        for (int i = 0; i < count; i++)
        {
            switch (i % totalInvalidCases)
            {
                case 0:
                    invalidInputList.Add(new object[] { fixture.GetInvalidInputShortName(), "Name should be at leats 3 characters long." });
                    break;
                case 1:
                    invalidInputList.Add(new object[] { fixture.GetInvalidInputLongName(), "Name should be less or equal 255 characters long." });
                    break;
                case 2:
                    invalidInputList.Add(new object[] { fixture.GetInvalidInputTooLongDescription(), "Description should be less or equal 10000 characters long." });
                    break;
            }
        }
        
        return invalidInputList;
    }
}
