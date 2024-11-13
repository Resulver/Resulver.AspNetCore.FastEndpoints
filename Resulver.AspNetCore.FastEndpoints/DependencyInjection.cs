using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Resulver.AspNetCore.Core;

namespace Resulver.AspNetCore.FastEndpoints;

public static class DependencyInjection
{
    public static IServiceCollection AddResulver(this IServiceCollection services , params Assembly[] assemblies)
    {
        services.AddErrorResponseGenerator();
        services.AddErrorProfilesFromAssembly(assemblies);
        
        return services;
    }
    
    public static IServiceCollection AddErrorProfile<TErrorProfile>(this IServiceCollection services)
        where TErrorProfile : ErrorProfile
    {
        return services.AddErrorProfile<TErrorProfile, FailureResponse>();
    }

    public static IServiceCollection AddErrorProfilesFromAssembly(
        this IServiceCollection services,params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            services.AddErrorProfilesFromAssembly<FailureResponse>(assembly);
        }
        
        return services;
    }

    public static IServiceCollection AddErrorResponseGenerator(this IServiceCollection services)
    {
        services.AddErrorResponseGenerator<FailureResponse>();
        return services;
    }
}