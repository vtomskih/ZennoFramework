using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using ZennoFramework.Extensions;
using ZennoFramework.Infrastructure;
using ZennoFramework.Interception;

namespace ZennoFramework.Utilities
{
    internal static class StackTraceHelper
    {
        internal static SenderInfo GetInfo()
        {
            var types = GetTypesFromStackTrace();
            var name = types.Select(t => t.Name).Reverse().Join(".");
            name = Regex.Replace(name, @"`\d+", string.Empty);
            return new SenderInfo {FullName = name, StackTraceTypes = types};
        }

        private static List<Type> GetTypesFromStackTrace()
        {
            var types = new StackTrace().GetTypes().ToList();
            types.RemoveAt(0);
            types.RemoveAt(0);

            RemoveBaseTypes(types);
            return ExtractTypesWithInterface(types);
        }

        private static void RemoveBaseTypes(IList<Type> types)
        {
            while (true)
            {
                var type0 = types[0].ToGenericTypeDefinitionOrDefault();
                var type1 = types[1].BaseType.ToGenericTypeDefinitionOrDefault();

                if (type0 != type1 && type0.BaseType.ToGenericTypeDefinitionOrDefault() != typeof(object))
                    break;
                types.RemoveAt(0);
            }
        }

        private static List<Type> ExtractTypesWithInterface(IEnumerable<Type> types)
        {
            return types.Where(t => t.GetInterface(nameof(ILoggable)) != null).ToList();
        }
    }
}