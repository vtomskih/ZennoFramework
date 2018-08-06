using System.Linq;
using System.Xml.Linq;

namespace ZennoFramework.Xml.Validation
{
    internal static class AttributeChecker
    {
        public static void Check(XElement xElement, string elementFullName, string documentPath)
        {
            CheckNotAllowed(xElement, elementFullName, documentPath);
            CheckDuplicates(xElement, elementFullName, documentPath);
        }

        private static void CheckNotAllowed(XElement xElement, string elementFullName, string documentPath)
        {
            var allowed = new[] {"x", "xpath", "isparent", "iscollection", "noparent", "noparentxpath", "comment"};
            var notAllowed = xElement.Parent != null
                ? xElement.Attributes().FirstOrDefault(x => !allowed.Contains(x.Name.LocalName.ToLower()))
                : null;

            if (notAllowed == null)
                return;

            var message = $"Указан неподдерживаемый атрибут '{notAllowed.Name.LocalName}' " +
                          $"у элемента '{elementFullName}' в файле '{documentPath}'.";
            // TODO: добавить вывод строки и столбца
            throw new XmlException(message);
        }

        private static void CheckDuplicates(XElement xElement, string elementFullName, string documentPath)
        {
            var duplicateAttr = xElement.Attributes().GroupBy(x => x.Name.LocalName.ToLower())
                .Where(group => group.Count() > 1).Select(group => group.Key).FirstOrDefault();
            if (duplicateAttr == null)
                return;

            var attr = xElement.Attributes().First(x => x.Name.LocalName.ToLower() == duplicateAttr);
            var message = $"Обнаружено дублирование атрибута '{attr.Name.LocalName}' " +
                          $"у элемента '{elementFullName}' в файле '{documentPath}'. " +
                          $"Элемент не может содержать несколько атрибутов с одним и тем же именем " +
                          $"(регистр не важен).";
            // TODO: добавить вывод строки и столбца
            throw new XmlException(message);
        }

    }
}