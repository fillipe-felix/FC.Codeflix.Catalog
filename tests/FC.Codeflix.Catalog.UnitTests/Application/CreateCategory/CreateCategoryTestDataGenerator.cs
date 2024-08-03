﻿namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory;

public static class CreateCategoryTestDataGenerator
{
    public static IEnumerable<object[]> GetInvalidInputs(int count = 12)
    {
        var fixture = new CreateCategoryTestFixture();
        var invalidInputList = new List<object[]>();
        var totalInvalidCases = 4;

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
                    invalidInputList.Add(new object[] { fixture.GetInvalidInputDescriptionNull(), "Description should not be null." });
                    break;
                case 3:
                    invalidInputList.Add(new object[] { fixture.GetInvalidInputTooLongDescription(), "Description should be less or equal 10000 characters long." });
                    break;
            }
        }
        
        return invalidInputList;
    }
}
