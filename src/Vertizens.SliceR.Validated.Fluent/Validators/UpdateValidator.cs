using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Vertizens.SliceR.Operations;

namespace Vertizens.SliceR.Validated.Fluent;
internal class UpdateValidator<TKey, TUpdateRequest> : AbstractValidator<Update<TKey, TUpdateRequest>>
{
    public UpdateValidator(IServiceProvider serviceProvider)
    {
        var validator = serviceProvider.GetService<IValidator<TUpdateRequest>>();
        if (validator != null)
        {
            RuleFor(x => x.Domain).Configure(x => x.PropertyName = string.Empty).SetValidator(validator);
        }
    }
}

