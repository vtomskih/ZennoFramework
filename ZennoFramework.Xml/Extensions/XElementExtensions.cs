using System;
using System.Linq;
using System.Xml.Linq;

namespace ZennoFramework.Xml.Extensions
{
    public static class XElementExtensions
    {
        public static bool HasNamespace(this XElement @this) => !string.IsNullOrEmpty(@this?.Name.NamespaceName);
        public static bool HasXPath(this XElement @this) => @this.ToElement().HasXPath;

        public static bool HasXPathFromAncestor(this XElement @this)
        {
            var element = @this.ToElement();
            var parent = @this.Parent?.ToElement();
            var noParent = element.NoParent || parent != null && !parent.IsCollection && !parent.IsParent;
            return !element.NoParentXPath && noParent && parent != null && 
                   (parent.HasXPath || @this.Parent.HasXPathFromAncestor());
        }

        public static string GetFullXPath(this XElement @this)
        {
            return @this.HasXPathFromAncestor()
                ? @this.Parent.GetFullXPath() + @this.ToElement().XPath
                : @this.ToElement().XPath;
        }

        public static void SetXPath(this XElement @this, string xpath)
        {
            var attr = @this.Attribute("x", true) ?? @this.Attribute("xpath", true);
            if (attr != null)
                attr.Value = xpath;
            else
                @this.Add(new XAttribute("x", xpath));
        }

        public static XAttribute Attribute(this XElement @this, string name, bool ignoreCase)
        {
            var comparison = ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
            return @this.Attributes().SingleOrDefault(a => string.Equals(a.Name.LocalName, name, comparison));
        }

        public static void AddOrUpdateAttribute(this XElement xElement, string name, object value)
        {
            var attribute = xElement.Attribute(name, true);
            if (attribute != null)
            {
                attribute.Value = value.ToString();
            }
            else
            {
                xElement.Add(new XAttribute(name, value));
            }
        }
    }
}