using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Resulver.AspNetCore.Core;

namespace Resulver.AspNetCore.FastEndpoints;

public static class DependencyInjection
{
    public static IServiceCollection AddErrorProfile<TErrorProfile>(this IServiceCollection services)
        where TErrorProfile : ErrorProfile
    {
        return services.AddErrorProfile<TErrorProfile, FailureResponse>();
    }

    public static IServiceCollection AddErrorProfilesFromAssembly(
        this IServiceCollection services, Assembly assembly)
    {
        return services.AddErrorProfilesFromAssembly<FailureResponse>(assembly);
    }

    public static IServiceCollection AddErrorResponseGenerator(this IServiceCollection services)
    {
        services.AddErrorResponseGenerator<FailureResponse>();
        return services;
    }
}