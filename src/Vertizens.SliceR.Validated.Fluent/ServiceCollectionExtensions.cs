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
        services.TryAddTransient(typeof(IModelValidator<>), typeof(FluentModelValidator<>));
        services.AddInterfaceTypes(typeof(IValidator<>), types: types);
        services.AddDefaultOperationValidators();
        services.AddValidatorProxy();

        return services;
    }

    private static void AddDefaultOperationValidators(this IServiceCollection services)
    {
        var validatedHandlerServices = services.GetGenericService(typeof(IValidatedHandler<,>)).ToList();
        var validatorTypes = services.GetGenericService(typeof(IValidator<>)).Select(x => x.ServiceType.GetGenericArguments().First()).ToHashSet();

        foreach (var validatedHandlerService in validatedHandlerServices)
        {
            var arguments = validatedHandlerService.ServiceType.GetGenericArguments();
            var requestType = arguments[0];

            if (requestType.IsGenericType)
            {
                var requestTypeDefinition = requestType.GetGenericTypeDefinition();

                if (requestTypeDefinition == typeof(Insert<>))
                {
                    var requestArgments = requestType.GetGenericArguments();
                    if (validatorTypes.Contains(requestArgments[0]))
                    {
                        services.TryAddTransient(typeof(IValidator<>).MakeGenericType(requestType), typeof(InsertValidator<>).MakeGenericType(requestArgments));
                    }
                }
                else if (requestTypeDefinition == typeof(Update<,>))
                {
                    var requestArgments = requestType.GetGenericArguments();
                    if (validatorTypes.Contains(requestArgments[1]))
                    {
                        services.TryAddTransient(typeof(IValidator<>).MakeGenericType(requestType), typeof(UpdateValidator<,>).MakeGenericType(requestArgments));
                    }
                }
            }
        }

    }

    private static void AddValidatorProxy(this IServiceCollection services)
    {
        var validatorTypes = services.GetGenericService(typeof(IValidator<>)).Select(x => x.ServiceType.GetGenericArguments().First()).ToHashSet();
        services.AddSliceRValidatorProxy((serviceType, implementation) => validatorTypes.Contains(serviceType.GetGenericArguments()[0]));
    }

    private static IEnumerable<ServiceDescriptor> GetGenericService(this IServiceCollection services, Type genericTypeDefinition)
    {
        return services.Where(x => x.ServiceType.IsGenericType && x.ServiceType.GetGenericTypeDefinition() == genericTypeDefinition);
    }
}
