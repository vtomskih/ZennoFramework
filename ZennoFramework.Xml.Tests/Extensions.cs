using System.Xml.Linq;
using ZennoFramework.Xml.Extensions;

namespace ZennoFramework.Generation.Tests
{
    static partial class Extensions
    {
        public static string GetXPath(this XDocument document, string elementName) =>
            document.GetElement(elementName).Attribute("x", true).Value;

        public static string GetKey(this XDocument document, string elementName) =>
            document.GetElement(elementName).Attribute("key", true)?.Value;

        public static string GetImportedKey(this XDocument document, string elementName) =>
            document.GetElement(elementName).Attribute("ImportedKey", true)?.Value;
    }
}