using FluentValidation;

namespace Vertizens.SliceR.Validated.Fluent;
internal class FluentModelValidator<TModel>(IValidator<TModel> validator) : IModelValidator<TModel>
{
    public async Task<ValidatedResult> Validate(TModel model, CancellationToken cancellationToken = default)
    {
        var result = new ValidatedResult();

        var validationResult = await validator.ValidateAsync(model, cancellationToken);
        var messages = validationResult.Errors.GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage)
            );
        result = new ValidatedResult { Messages = messages };

        return result;
    }
}
