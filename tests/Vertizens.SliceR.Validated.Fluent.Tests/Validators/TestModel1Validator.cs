using FluentValidation;

namespace Vertizens.SliceR.Validated.Fluent.Tests;
internal class TestModel1Validator : AbstractValidator<TestModel1>
{
    public TestModel1Validator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(20);
    }
}
