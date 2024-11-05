using Microsoft.Extensions.DependencyInjection;

namespace Vertizens.SliceR.Validated.Fluent.Tests;

public class FluentModelValidatorTests
{
    [Fact]
    public async Task InvalidName()
    {
        var services = new ServiceCollection();
        services.AddSliceRFluentValidators();
        var provider = services.BuildServiceProvider();
        var validator = provider.GetRequiredService<IModelValidator<TestModel1>>();

        var testModel = new TestModel1 { Name = "Too Long Name of which is not valid." };
        var results = await validator.Validate(testModel);

        Assert.NotNull(results);
        Assert.False(results.IsSuccessful);
    }

    [Fact]
    public async Task ValidName()
    {
        var services = new ServiceCollection();
        services.AddSliceRFluentValidators();
        var provider = services.BuildServiceProvider();
        var validator = provider.GetRequiredService<IModelValidator<TestModel1>>();

        var testModel = new TestModel1 { Name = "Short Name." };
        var results = await validator.Validate(testModel);

        Assert.NotNull(results);
        Assert.True(results.IsSuccessful);
    }
}