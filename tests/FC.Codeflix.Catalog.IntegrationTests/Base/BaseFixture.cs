﻿using Bogus;

namespace FC.Codeflix.Catalog.IntegrationTests.Base;

public class BaseFixture
{
    protected Faker Faker { get; set; }

    public BaseFixture(Faker faker)
    {
        Faker = faker;
    }
}
