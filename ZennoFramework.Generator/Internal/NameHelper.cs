using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ZennoFramework.Generator.Internal
{
    internal static class NameHelper
    {
        public static string GetCollectionName(string elementName)
        {
            var name = elementName.Trim();
            if (Regex.IsMatch(name, @"item$", RegexOptions.IgnoreCase))
                name = Regex.Replace(name, @"item$", "Collection", RegexOptions.IgnoreCase);
            if (!Regex.IsMatch(name, @"collection\d*$", RegexOptions.IgnoreCase))
                name += "Collection";
            return name;
        }

        public static string GetItemName(string elementName)
        {
            var name = elementName.Trim();
            if (Regex.IsMatch(name, @"collection\d*$", RegexOptions.IgnoreCase))
                name = Regex.Replace(name, @"collection(?=\d*$)", "Item", RegexOptions.IgnoreCase);
            return name;
        }

        public static bool ElementExists(this IEnumerable<Element> elements, string elementName)
        {
            return elements.FirstOrDefault(x => x.Name == elementName) != null;
        }
    }
}