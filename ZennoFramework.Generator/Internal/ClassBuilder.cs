using ZennoFramework.Generator.Extensions;

namespace ZennoFramework.Generator.Internal
{
    internal static class ClassBuilder
    {
        public static string CreateClass(Element element)
        {
            var result = string.Empty;
            var hasParent = !element.IsRoot && !element.NoParent || element.IsCollection;

            result += GetClassName(element).NewLine();
            result += "{".NewLine();

            if (hasParent && element.HasXPath)
                result += "    private readonly Element _parent;".NewLine().NewLine();

            result += GetCtor(element, hasParent).NewLine();
            result += PropertyBuilder.GetProperties(element);

            if (element.HasXPath)
                result += GetOnFindingMethod(element).AddIndents("    ").NewLine();

            return result + "}";
        }

        private static string GetClassName(Element element)
        {
            var baseClassName = "ElementContainer<BotContext>";
            if (element.HasXPath)
            {
                baseClassName = element.IsOverridedXPath ? element.GetRef() + element.Name : "Element";
            }
            
            return $"public partial class {element.Name} : {baseClassName}";
        }

        private static string GetCtor(Element element, bool hasParent)
        {
            var result = string.Empty;
            var xpathsCtorParam = element.IsRoot ? ", Dictionary<string, string> keysAndXpaths" : string.Empty;
            var parentCtorParam = hasParent ? ", Element parent = null" : string.Empty;
            var baseCtor = element.IsImported && hasParent ? " : base(context, parent)" : " : base(context)";
            var ctorName = $"public {element.Name}(BotContext context{xpathsCtorParam}{parentCtorParam}){baseCtor}";

            result += "    " + ctorName.NewLine();
            result += "    {".NewLine();

            if (element.Parent == null)
                result += "        context.PrepareXPath(keysAndXpaths);\r\n";
            if (hasParent && element.HasXPath)
                result += "        _parent = parent;".NewLine();

            result += PropertyBuilder.GetPropertiesInCtor(element);
            result += "    }";
            return result;
        }

        private static string GetOnFindingMethod(Element element)
        {
            var hasParent = !element.IsRoot && !element.NoParent || element.IsCollection;
            var parentParam = hasParent ? ", _parent" : string.Empty;

            var result = string.Empty.NewLine();
            result += "protected override Element Find()".NewLine();
            result += "{".NewLine();
            result += $"    return Context.CreateElement(\"{element.Key}\"{parentParam});".NewLine();
            result += "}";
            return result;
        }
    }
}