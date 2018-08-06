using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using ZennoFramework.Xml.Extensions;

namespace ZennoFramework.Xml
{
    public static class LocatorRenderer
    {
        private static Dictionary<string, XDocument> _documents;

        public static void Run(Dictionary<string, XDocument> documents)
        {
            _documents = documents;

            foreach (var document in _documents.Values)
                ParseElement(document.Root);

            foreach (var document in _documents.Values)
                SetXPaths(document);
        }

        private static void SetXPaths(XDocument document)
        {
            var root = new XElement(document.Root);

            var elementsFrom = document.Root.Descendants().ToList();
            var elementsTo = root.Descendants().ToList();

            for (int i = 0; i < elementsFrom.Count; i++)
            {
                var from = elementsFrom[i];
                var to = elementsTo[i];

                if (from.NodeType == XmlNodeType.Element && from.HasXPath())
                    to.SetXPath(from.GetFullXPath());
            }

            document.Root.Remove();
            document.Add(root);
        }

        private static void ParseElement(XElement xElement)
        {
            if (xElement.NodeType != XmlNodeType.Element)
                return;

            if (string.IsNullOrWhiteSpace(xElement.Attribute("Key", true)?.Value) && !xElement.HasNamespace())
                SetElementKey(xElement);
            if (xElement.Parent == null)
                xElement.Add(new XAttribute("IsRoot", "true"));

            if (xElement.HasNamespace())
            {
                var xImported = ImportElement(xElement);
                if (xImported.Parent == null)
                    xElement.Name = xElement.Name.Namespace.GetName(xImported.Name.LocalName);
                SetElementKey(xImported);
                SetElementKey(xElement);

                if (!xElement.HasXPath() && !xElement.HasXPathFromAncestor())
                    MapKey(xImported, xElement);

                MapXPath(xImported, xElement);

                var imported = xImported.ToElement();
                var element = xElement.ToElement();

                xElement.AddOrUpdateAttribute("IsParent", imported.IsParent);
                xElement.AddOrUpdateAttribute("IsCollection", imported.IsCollection);
                if (element.Comment == null && imported.Comment != null)
                    xElement.AddOrUpdateAttribute("Comment", imported.Comment);

                xElement.Add(new XAttribute("IsImported", true));
                xElement.Add(new XAttribute("ImportedKey", xImported.Attribute("Key").Value));

                if (xImported.Parent == null)
                    xElement.Add(new XAttribute("IsRoot", "true"));
            }

            foreach (var xInnerElement in xElement.Elements())
            {
                ParseElement(xInnerElement);
            }
        }

        private static void SetElementKey(XElement element)
        {
            var key = GetElementKey(element);
            var attr = element.Attribute("Key");

            if (attr == null)
            {
                element.Add(new XAttribute("Key", key));
            }
            else
            {
                attr.Value = key;
            }
        }

        private static string GetElementKey(XElement element)
        {
            var key = element.Name.LocalName;
            var parentKey = element.Parent?.ToElement().Key;

            if (!string.IsNullOrWhiteSpace(parentKey) && !element.HasXPath())
                return parentKey + "." + key;

            return element.Parent == null ? key : GetElementKey(element.Parent) + "." + key;
        }

        private static XElement ImportElement(XElement element)
        {
            var root = _documents[element.Name.NamespaceName].Root;
            var elementName = element.Name.LocalName.ToLower();

            if (root == null || elementName == "root")
                return root;

            var imported = new XElement(root);
            var names = element.Name.LocalName.Split('.');

            foreach (var name in names)
            {
                imported = imported.Elements().FirstOrDefault(x => x.Name.LocalName == name);

                if (imported == null)
                    break;

                if (imported.HasNamespace())
                {
                    var subImported = ImportElement(imported);
                    var xpath = subImported.GetFullXPath();

                    imported.SetXPath(xpath);
                }
            }

            return imported;
        }

        private static void MapKey(XElement from, XElement to)
        {
            var fromAttr = from.Attribute("Key", true);
            if (string.IsNullOrWhiteSpace(fromAttr?.Value))
                return;

            to.Attribute("Key", true)?.Remove();
            to.Add(fromAttr);
        }

        private static void MapXPath(XElement from, XElement to)
        {
            var xpath = from.ToElement().XPath;
            var xpathFull = from.GetFullXPath();
            var xpathFromAncestor = string.Empty;
            if (xpathFull != xpath)
                xpathFromAncestor = xpathFull?.Remove(xpathFull.Length - xpath?.Length ?? 0);

            if (string.IsNullOrEmpty(xpathFull))
                return;

            var elementTo = to.ToElement();
            xpath = elementTo.HasXPath ? xpathFromAncestor + elementTo.XPath : xpathFull;

            var attr = to.Attribute("x", true) ?? to.Attribute("xpath", true);
            attr?.Remove();
            to.Add(new XAttribute("x", xpath));
        }
    }
}