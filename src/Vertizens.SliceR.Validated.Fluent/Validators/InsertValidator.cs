using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Vertizens.SliceR.Operations;

namespace Vertizens.SliceR.Validated.Fluent;
internal class InsertValidator<TInsertRequest> : AbstractValidator<Insert<TInsertRequest>>
{
    public InsertValidator(IServiceProvider serviceProvider)
    {
        var validator = serviceProvider.GetService<IValidator<TInsertRequest>>();
        if (validator != null)
        {
            RuleFor(x => x.Domain).Configure(x => x.PropertyName = string.Empty).SetValidator(validator);
        }
    }
}

