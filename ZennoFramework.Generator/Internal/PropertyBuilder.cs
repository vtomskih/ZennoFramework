using System;
using System.Text.RegularExpressions;
using ZennoFramework.Generator.Extensions;

namespace ZennoFramework.Generator.Internal
{
    internal static class PropertyBuilder
    {
        public static string GetProperties(Element element)
        {
            var result = element.Elements.Values.Count > 0 ? Environment.NewLine : string.Empty;

            foreach (var item in element.Elements.Values)
            {
                if (!string.IsNullOrEmpty(item.Comment))
                    result = result.NewLineSafe() + Comment.GetComment(item.Comment).AddIndents("    ").NewLine();
                result += CreateProperty(item).AddIndents("    ").NewLine();
            }

            return result;
        }

        public static string GetPropertiesInCtor(Element element)
        {
            var result = string.Empty;

            foreach (var item in element.Elements.Values)
            {
                var property = CreatePropertyInCtor(item);
                result += "        " + property.NewLine();
            }

            return result;
        }

        private static string CreatePropertyInCtor(Element element)
        {
            var generic = element.IsCollection ? GetCollectionPropertyGeneric(element) : string.Empty;
            var propertyName = GetPropertyName(element);
            var propertyNamespace = GetPropertyNamespace(element);
            var propertyCtorParams = GetPropertyCtorParams(element);

            return $"{propertyName} = new {propertyNamespace}{generic}(context{propertyCtorParams});";
        }

        private static string CreateProperty(Element element)
        {
            var generic = element.IsCollection ? GetCollectionPropertyGeneric(element) : string.Empty;
            var propertyName = GetPropertyName(element);
            var propertyNamespace = GetPropertyNamespace(element);
            return $"public {propertyNamespace}{generic} {propertyName} {{ get; }}";
        }

        private static string GetPropertyName(Element element)
        {
            return element.IsCollection
                ? Regex.Replace(element.Name, @"[Ii]tem$", string.Empty) + "Collection"
                : element.Name;
        }

        private static string GetPropertyNamespace(Element element)
        {
            var propertyName = GetPropertyName(element);
            if (element.IsRoot)
                return propertyName;

            return element.IsImported && !element.IsOverridedXPath
                ? element.GetRef() + propertyName
                : $"{element.Parent.Name}Elements.{propertyName}";
        }

        private static string GetCollectionPropertyGeneric(Element element)
        {
            var generic = $"{element.Parent.Name}Elements.{element.Name}";
            if (element.IsImported && !element.IsOverridedXPath)
                generic = element.GetRef() + element.Name;
            return $"<{generic}>";
        }

        private static string GetPropertyCtorParams(Element element)
        {
            var propertyParams = string.Empty;

            if (!element.NoParent && !element.Parent.IsRoot)
            {
                if (element.Parent.IsParent || element.Parent.IsCollection)
                    propertyParams = ", this";
                else if (!element.Parent.NoParent)
                    propertyParams = ", parent";
            }

            if (element.IsImported && element.IsRoot)
            {
                propertyParams = ", null";
            }

            return propertyParams;
        }
    }
}