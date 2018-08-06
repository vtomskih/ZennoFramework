using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ZennoFramework.Xml.Extensions;

namespace ZennoFramework.Xml
{
    public class Loader
    {
        private readonly Dictionary<string, XDocument> _documents = new Dictionary<string, XDocument>();

        private Loader(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Не найден файл: " + path);
        }

        public static Dictionary<string, XDocument> Load(string path)
        {
            var loader = new Loader(path);
            loader.LoadDocuments(path, "");
            return loader._documents;
        }

        private void LoadDocuments(string path, string senderPath)
        {
            if (!File.Exists(path))
            {
                var message = $"Ошибка парсинга XML. Не удалось подключить ресурс в файле '{senderPath}'. " +
                              $"Файл ресурса не найден: {path}";
                throw new FileNotFoundException(message, path);
            }

            var document = LoadDocument(path);
            if (document?.Root != null)
            {
                foreach (var attribute in document.Root.Attributes()
                    .Where(a => a.IsNamespaceDeclaration && a.Name.LocalName != "xmlns"))
                    LoadDocuments(attribute.Value, path);
            }
        }

        private XDocument LoadDocument(string path)
        {
            XDocument doc;
            if (_documents.ContainsKey(path) || (doc = XDocument.Load(path)).Root == null)
                return null;

            doc.NormalizeNamespaces(Path.GetDirectoryName(path));
            _documents.Add(path, doc);
            return doc;
        }
    }
}