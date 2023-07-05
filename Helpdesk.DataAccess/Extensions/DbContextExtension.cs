using System.Reflection;
using Helpdesk.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Helpdesk.DataAccess.Extensions;

public static class DbContextExtension
{
    public static IQueryable? Set(this DbContext context, Type T)
    {
        var method = typeof(DbContext).GetMethod(nameof(DbContext.Set), BindingFlags.Public | BindingFlags.Instance,
            Array.Empty<Type>());
        method = method?.MakeGenericMethod(T);
        return method?.Invoke(context, null) as IQueryable;
    }

    public static Type? FindModelClrEntityType(this IModel model, string entityTypeName)
    {
        return Assembly
            .GetAssembly(typeof(IEntity))!
            .GetTypes()
            .FirstOrDefault(t => t.Name == $"{entityTypeName}DataModel");
    }
}