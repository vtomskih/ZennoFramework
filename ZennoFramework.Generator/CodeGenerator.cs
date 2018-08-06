using System.Xml.Linq;
using ZennoFramework.Api.Common.Messages;
using ZennoFramework.Generator.Extensions;
using ZennoFramework.Generator.Internal;

namespace ZennoFramework.Generator
{
    public static class CodeGenerator
    {
        public static string Generate(GenerationData data)
        {
            data.Namespace = string.IsNullOrWhiteSpace(data.Namespace) ? "ZennoFramework.Generated" : data.Namespace;
            
            var result = Header.Create().NewLine();
            result += "namespace " + data.Namespace.NewLine();
            result += "{".NewLine();

            foreach (var xml in data.XmlDocuments)
            {
                var document = XDocument.Parse(xml.Trim('\uFEFF', '\u200B'));
                var root = document.ToGenerationElements();
                result = result.NewLineSafe();
                result += WriteElements(root, "    ");
            }

            return result + "}".NewLine();
        }

        private static string WriteElements(Element element, string indent)
        {
            var result = string.Empty;
            var tempIndent = indent;

            var createClass = !element.IsImported || element.IsOverridedXPath;
            var createNamespace = createClass && element.Elements.Values.Count > 0;

            if (createClass)
            {
                var codeBuilder = new CodeBuilder().With(element).AddElementCollection().AddClass();
                result = codeBuilder.Build().AddIndents(indent).NewLine();

                if (createNamespace)
                {
                    result = result.NewLineSafe();
                    result += indent + $"namespace {element.Name}Elements".NewLine();
                    result += indent + "{".NewLine();
                    
                    indent += "    ";
                }
            }

            foreach (var innerElement in element.Elements.Values)
            {
                result = result.NewLineSafe();
                result += WriteElements(innerElement, indent);
            }

            if (createNamespace)
                result += tempIndent + "}".NewLine();
            return result;
        }
    }
}