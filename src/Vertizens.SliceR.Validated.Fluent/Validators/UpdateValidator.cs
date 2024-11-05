using FluentValidation;
using Vertizens.SliceR.Operations;

namespace Vertizens.SliceR.Validated.Fluent;
internal class UpdateValidator<TKey, TUpdateRequest> : AbstractValidator<Update<TKey, TUpdateRequest>>
{
    public UpdateValidator(IValidator<TUpdateRequest> validatorUpdateRequest)
    {
        RuleFor(x => x.Domain).Configure(x => x.PropertyName = string.Empty).SetValidator(validatorUpdateRequest);
    }
}

