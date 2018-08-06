using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ZennoFramework.Xml.Extensions
{
    public static class XDocumentExtensions
    {
        /// <summary>
        /// Заменяет все относительные пути у пространств имен на абсолютные для всех елементов.
        /// </summary>
        /// <param name="document">XDocument.</param>
        /// <param name="documentDirectory">
        /// Путь к директории текущего документа. Будет добавлен ко всем относительным путям, 
        /// указанным в пространствах имен всех элементов.
        /// </param>
        public static void NormalizeNamespaces(this XDocument document, string documentDirectory)
        {
            foreach (var element in document.Descendants())
            {
                foreach (var at in element.Attributes().Where(a => a.IsNamespaceDeclaration))
                    at.Value = CombinePath(documentDirectory, at.Value);

                if (element.HasNamespace())
                {
                    XNamespace ns = CombinePath(documentDirectory, element.Name.NamespaceName);
                    element.Name = ns.GetName(element.Name.LocalName);
                }
            }

            string CombinePath(string path1, string path2) =>
                Path.GetFullPath(Path.Combine(path1, path2.TrimStart('/', '\\')));
        }

        public static XElement GetElement(this XDocument document, string elementName)
        {
            var name = elementName;
            var element = document.Root ?? throw new System.Exception("Корневой элемент не найден.");

            do
            {
                var element1 = element.Elements().FirstOrDefault(x => x.Name.LocalName == name);

                if (element1 == null)
                {
                    var last = name.Split('.').Last();
                    name = name.Remove(name.Length - last.Length - 1);
                }
                else
                {
                    element = element1;

                    if (name == elementName)
                        break;

                    elementName = elementName.Remove(0, name.Length + 1);
                    name = elementName;
                }
            } while (!string.IsNullOrEmpty(name));

            return element;
        }

        public static string Save(this XDocument @this)
        {
            using (var ms = new MemoryStream())
            {
                @this.Save(ms);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}