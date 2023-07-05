﻿namespace Helpdesk.WebApi.Helpers;

public static class TypeExtension
{
    public static bool IsAssignableFromGenericType(this Type genericType, Type givenType)
    {
        if (genericType.IsAssignableFrom(givenType))
        {
            return true;
        }

        var interfaceTypes = givenType.GetInterfaces();

        foreach (var it in interfaceTypes)
        {
            if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                return true;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        var baseType = givenType.BaseType;

        if (baseType == null)
        {
            return false;
        }

        return genericType.IsAssignableFromGenericType(baseType);
    }
}