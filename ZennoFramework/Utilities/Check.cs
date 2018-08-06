using System;
using ZennoFramework.Infrastructure.Elements.Internal;

namespace ZennoFramework.Utilities
{
    internal static class Check
    {
        public static T NotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static T? NotNull<T>(T? value, string parameterName) where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static string NotEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"Argument {parameterName} is empty.", parameterName);
            }

            return value;
        }

        public static void ElementCollectionItem(Type itemType, out int ctorParametersCount)
        {
            if (itemType == typeof(Element))
            {
                ctorParametersCount = 1;
                return;
            }
            
            var checker = new ElementCollectionChecker(itemType);
            checker.ItemDescendantOfElement();
            checker.ItemRequiredCtor(out ctorParametersCount);
        }
    }
}