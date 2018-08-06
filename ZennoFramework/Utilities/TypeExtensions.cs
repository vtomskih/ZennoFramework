using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ZennoFramework.Utilities
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetTypes(this StackTrace type) =>
            type.GetFrames()?.Select(f => f.GetMethod().ReflectedType);

        public static Type ToGenericTypeDefinitionOrDefault(this Type type) =>
            type?.IsGenericType ?? false ? type.GetGenericTypeDefinition() : type;
    }
}