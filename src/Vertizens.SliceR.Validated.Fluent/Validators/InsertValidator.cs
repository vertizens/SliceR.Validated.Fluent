using FluentValidation;
using Vertizens.SliceR.Operations;

namespace Vertizens.SliceR.Validated.Fluent;
internal class InsertValidator<TInsertRequest> : AbstractValidator<Insert<TInsertRequest>>
{
    public InsertValidator(IValidator<TInsertRequest> validatorInsertRequest)
    {
        RuleFor(x => x.Domain).Configure(x => x.PropertyName = string.Empty).SetValidator(validatorInsertRequest);
    }
}

