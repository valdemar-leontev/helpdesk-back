using System.Reflection;
using Helpdesk.WebApi.Commands;

namespace Helpdesk.WebApi;

public static class Extension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        var types = Assembly
            .GetExecutingAssembly()
            .GetTypes();

        types
            .Where(t => t.Name.EndsWith("Service"))
            .ToList()
            .ForEach(t => { services.AddTransient(t); });

        types
            .Where(t => typeof(DataCommand).IsAssignableFrom(t) && !t.IsAbstract)
            .ToList()
            .ForEach(t => { services.AddTransient(t); });

        return services;
    }
}