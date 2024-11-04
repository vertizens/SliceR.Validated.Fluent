using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using Vertizens.ServiceProxy;
using Vertizens.SliceR.Operations;

namespace Vertizens.SliceR.Validated.Fluent;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSliceRFluentValidators(this IServiceCollection services)
    {
        return services.AddSliceRFluentValidators(Assembly.GetCallingAssembly());
    }

    public static IServiceCollection AddSliceRFluentValidators(this IServiceCollection services, Assembly assembly)
    {
        return services.AddSliceRFluentValidators(assembly.GetTypes());
    }

    public static IServiceCollection AddSliceRFluentValidators(this IServiceCollection services, params Type[] types)
    {
        services.TryAddTransient<IModelValidator, FluentModelValidator>();
        services.AddSliceRValidatorProxy();
        services.AddInterfaceTypes(typeof(IValidator<>), types: types);
        services.AddDefaultOperationValidators();

        return services;
    }

    private static void AddDefaultOperationValidators(this IServiceCollection services)
    {
        var validatedHandlerServices = services.Where(x => x.ServiceType.IsGenericType && x.ServiceType.GetGenericTypeDefinition() == typeof(IValidatedHandler<,>)).ToList();

        foreach (var validatedHandlerService in validatedHandlerServices)
        {
            var arguments = validatedHandlerService.ServiceType.GetGenericArguments();
            var requestType = arguments[0];

            if (requestType.IsGenericType)
            {
                var requestTypeDefinition = requestType.GetGenericTypeDefinition();

                if (requestTypeDefinition == typeof(Insert<>))
                {
                    services.TryAddTransient(typeof(IValidator<>).MakeGenericType(requestType), typeof(InsertValidator<>).MakeGenericType(requestTypeDefinition.GetGenericArguments()));
                }
                else if (requestTypeDefinition == typeof(Update<,>))
                {
                    services.TryAddTransient(typeof(IValidator<>).MakeGenericType(requestType), typeof(UpdateValidator<,>).MakeGenericType(requestTypeDefinition.GetGenericArguments()));
                }
            }
        }

    }
}
