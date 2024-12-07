# SliceR.Validated.Fluent

Adds FluentValidation to SliceR for model validation pipeline of validated handler requests.

## Getting Started

This library is for adding FluentValidation implementation to your `IValidatedHandler` pipeline.  It adds all the plumbing to be called before the handler that does the implementation and calls the IValidator for the request type.

Register with this call:  

    services.AddSliceRFluentValidators();

The `Insert<>` and `Update<,>` operations already have validators registered that call IValidators on the generic type that is the focus for those operations.

Search for FluentValidation for how to use that library but know that any `IValidator` that is defined in calling assembly gets registered in the above call.

Implement `AbstractValidator<RequestType>` according to FluentValidation documentation.