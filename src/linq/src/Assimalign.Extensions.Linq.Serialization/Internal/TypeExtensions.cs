using System;
using System.Reflection;

namespace Assimalign.Extensions.Linq.Serialization.Internal;

internal static class TypeExtensions
{
    public static bool IsArray(this Type type)
    {
        return type.IsArray;
    }

    public static bool IsClass(this Type type)
    {
        return type.GetTypeInfo().IsClass;
    }

    public static bool IsSubclassOf(this Type type, Type c)
    {
        return type.GetTypeInfo().IsSubclassOf(c);
    }

    public static bool IsCustomAttributeDefined(this Type type, Type attributeType, bool inherit)
    {
        return type.GetTypeInfo().IsDefined(attributeType, inherit);
    }

    public static bool IsEnum(this Type type)
    {
        return type.GetTypeInfo().IsEnum;
    }

    public static bool IsInterface(this Type type)
    {
        return type.GetTypeInfo().IsInterface;
    }

    public static bool IsGenericType(this Type type)
    {
        return type.GetTypeInfo().IsGenericType;
    }

    public static bool IsValueType(this Type type)
    {
        return type.GetTypeInfo().IsValueType;
    }

    public static Type GetBaseType(this Type type)
    {
        return type.GetTypeInfo().BaseType;
    }

    public static TypeAttributes GetTypeAttributes(this Type type)
    {
        return type.GetTypeInfo().Attributes;
    }
}
