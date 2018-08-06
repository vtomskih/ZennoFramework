using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using ZennoFramework.Xml.Extensions;

namespace ZennoFramework.Xml.Validation
{
    public class Validator
    {
        private readonly Dictionary<string, XDocument> _documents;
        private string _currentPath;

        private Validator(Dictionary<string, XDocument> documents)
        {
            _documents = documents ?? throw new ArgumentNullException(nameof(documents));
        }

        public static void CheckDocuments(Dictionary<string, XDocument> documents)
        {
            var validator = new Validator(documents);
            validator.CheckRootNames();
            validator.CheckElements();
        }

        private void CheckRootNames()
        {
            var names = new Dictionary<string, string>();

            foreach (var document in _documents)
            {
                var path = document.Key;
                var name = document.Value.Root.Name.LocalName;

                if (names.ContainsKey(name))
                {
                    var message = $"Корневой элемент '{name}' в файле '{path}' содержит повторяющееся имя " +
                                  $"с корневым элементом '{name}' в файле '{names[name]}'. " +
                                  $"Имена корневых элементов не должны совпадать.";
                    throw new XmlException(message, 0, 0);
                }

                names.Add(name, path);
            }
        }

        private void CheckElements()
        {
            foreach (var document in _documents)
            {
                _currentPath = document.Key;
                CheckElements(document.Value.Root);
            }
        }

        private Element CheckElements(XElement xElement, Element parent = null)
        {
            if (xElement.NodeType != XmlNodeType.Element)
                return null;

            var element = xElement.ToElement();
            element.Parent = parent;
            element.Namespace = string.IsNullOrEmpty(element.Parent?.Namespace)
                ? element.Parent?.Name
                : element.Parent.Namespace + "." + element.Parent.Name;

            var elementName = string.IsNullOrEmpty(element.Namespace)
                ? element.Name
                : $"{element.Namespace}.{element.Name}";

            AttributeChecker.Check(xElement, elementName, _currentPath);

            if (xElement.HasNamespace())
            {
                var xImported = ImportElement(xElement);
                var importedName = xImported?.Name.LocalName;

                if (importedName == null)
                {
                    var message = $"Не удалось подключить элемент '{elementName}' в файле '{_currentPath}'. " +
                                  $"Элемент не найден во внешнем файле '{xElement.Name.NamespaceName}'.";
                    // TODO: добавить вывод строки и столбца
                    throw new XmlException(message);
                }

                var elementHasChildren = xElement.Elements().Any(x => x.NodeType == XmlNodeType.Element);

                var overrideError = elementHasChildren ? "У элемента были добавлены дочерние элементы" :
                    element.HasXPath ? "У элемента был задан новый XPath" :
                    element.IsParent ? $"У элемента указан атрибут {nameof(element.IsParent)}" :
                    element.IsCollection ? $"У элемента указан атрибут {nameof(element.IsCollection)}" : string.Empty;

                if (overrideError != string.Empty)
                {
                    var message = $"Ошибка подключения элемента '{elementName}' в файле '{_currentPath}'. " +
                                  $"{overrideError}. Переопределение подключаемых элементов недоступно.";
                    // TODO: добавить вывод строки и столбца
                    throw new XmlException(message);
                }

                element.Name = importedName;
                elementName = string.IsNullOrEmpty(element.Namespace)
                    ? element.Name
                    : $"{element.Namespace}.{element.Name}";
            }

            if (element.Name == element.Parent?.Name)
            {
                var message = $"Элемент не может содержать имя как у родительского элемента. " +
                              $"Элемент '{elementName}' в файле '{_currentPath}'.";
                // TODO: добавить вывод строки и столбца
                throw new XmlException(message);
            }

            var names = new HashSet<string>();

            foreach (var item in xElement.Elements())
            {
                var child = CheckElements(item, element);
                if (child == null)
                    continue;

                if (names.Contains(child.Name))
                {
                    elementName = string.IsNullOrEmpty(child.Namespace)
                        ? child.Name
                        : $"{child.Namespace}.{child.Name}";

                    var message = $"Повторяющийся элемент '{elementName}' в файле '{_currentPath}'. " +
                                  $"Элементы на одном уровне не могут содержать одинаковые имена.";
                    // TODO: добавить вывод строки и столбца
                    throw new XmlException(message);
                }

                names.Add(child.Name);
            }

            return element;
        }

        private XElement ImportElement(XElement element)
        {
            var imported = _documents[element.Name.NamespaceName].Root;
            if (imported == null)
                return null;

            var elementName = element.Name.LocalName.ToLower();

            if (elementName == "root")
                return imported;

            return element.Name.LocalName.Split('.').Aggregate(imported,
                (current, name) => current?.Elements().FirstOrDefault(x => x.Name.LocalName == name));
        }
    }
}