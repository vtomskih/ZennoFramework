using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ZennoFramework.Generator.Extensions
{
    public static class MappingExtensions
    {
        internal static Element ToGenerationElements(this XDocument @this) => @this.Root.ToElements();

        private static Element ToElements(this XElement @this, Element parent = null)
        {
            if (@this.NodeType != XmlNodeType.Element)
                return null;

            var element = @this.ToElement();
            element.Parent = parent;
            element.Namespace = string.IsNullOrEmpty(parent?.Namespace)
                ? parent?.Name
                : parent.Namespace + "." + parent.Name;

            foreach (var child in @this.Elements())
            {
                var mapped = child.ToElements(element);
                if (mapped != null)
                    element.Elements.Add(mapped.Namespace + "." + mapped.Name, mapped);
            }

            return element;
        }

        private static Element ToElement(this XElement xElement)
        {
            var element = new Element {Name = xElement.Name.LocalName.Split('.').Last()};

            foreach (var attr in xElement.Attributes())
            {
                var name = attr.Name.LocalName.ToLower();
                var valueIsTrue = attr.Value == "1" || attr.Value.ToLower() == "true";

                if (name == "key")
                    element.Key = attr.Value;
                if (name == "importedkey")
                    element.ImportedKey = attr.Value;
                if (name == "isroot")
                    element.IsRoot = valueIsTrue;
                if (name == "isimported")
                    element.IsImported = valueIsTrue;
                if (name == "comment")
                    element.Comment = attr.Value;
                if (name == "xpath" || name == "x")
                    element.XPath = attr.Value;
                if (name == "noparentxpath")
                    element.NoParentXPath = valueIsTrue;
                if (name == "iscollection")
                    element.IsCollection = valueIsTrue;
                if (name == "isparent")
                    element.IsParent = valueIsTrue;
                if (name == "noparent")
                    element.NoParent = valueIsTrue;
            }

            return element;
        }
    }
}