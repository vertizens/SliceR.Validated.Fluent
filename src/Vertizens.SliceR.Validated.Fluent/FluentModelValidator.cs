using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Vertizens.SliceR.Validated.Fluent;
internal class FluentModelValidator(IServiceProvider _serviceProvider) : IModelValidator
{
    public async Task<ValidatedResult> Validate<TModel>(TModel model, CancellationToken cancellationToken = default)
    {
        var result = new ValidatedResult();
        var validator = _serviceProvider.GetService<IValidator<TModel>>();

        if (validator != null)
        {
            var validationResult = await validator.ValidateAsync(model, cancellationToken);
            var messages = validationResult.Errors.GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage)
                );
            result = new ValidatedResult { Messages = messages };
        }

        return result;
    }

    public async Task<ValidatedResult<TResult>> Validate<TModel, TResult>(TModel model, CancellationToken cancellationToken = default)
    {
        var result = await Validate(model, cancellationToken);

        return new ValidatedResult<TResult>(result);
    }
}
